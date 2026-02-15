Crown Assignment 

Login Cred
Email : varshita@gmail.com
passowrd : admin 

I have used my local server sqk machine to connect to the server ,
In the Angular also , I have used the local api links for the API integration perpose.
IF you want to clone the project then in your local code , the sps and the database as EmployeeManagement should be present and if u have connecting to a server in the appsetting.json the server name also needs to be changed to ur local server . And in the local server all the below tables and sps should be present if u want to run the code locally . 

Similary , for the running the angular code as well. If the local apis links should be changed to your local apis endpoint and the visual studito should be running while the angular code is also running.

Employee - contains the Backend code 
EmployeeAngular - contains the Frontend code .

Sql Scehma and tables ans sps 


CREATE TABLE tblEmployee
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    tblEmployeeId NVARCHAR(36) 
        DEFAULT REPLACE(NEWID(), '-', ''),

    EmployeeId NVARCHAR(50),

    EmployeeName NVARCHAR(300),

    DepartmentId INT,
    
    Salary INT,

    IsActive BIT DEFAULT 1,
	CreatedOn datetime default getutcdate(),

    CONSTRAINT FK_tblEmployee_tblDepartment
        FOREIGN KEY (DepartmentId)
        REFERENCES tblDepartment(Id)
);


Create table tblDepartment(
Id int primary key identity(1,1) ,
tblDepartmentId nvarchar(36) default replace(newid(),'-',''),
DepartmentName nvarchar(300),
CreatedOn datetime default getutcdate(),
IsActive bit default 1
)


//Insert Employee


ALTER   PROCEDURE [dbo].[usp_InsertEmployee]
(
    @EmployeeId NVARCHAR(50),
    @EmployeeName NVARCHAR(200),
    @DepartmentId nvarchar(36),
    @Salary INT)
AS
BEGIN
    SET NOCOUNT ON;

     IF EXISTS (
        SELECT 1
        FROM tblEmployee
        WHERE EmployeeId = @EmployeeId
          AND IsActive = 1
    )
    BEGIN
        THROW 51000, 'EmployeeId already exists', 1;
        RETURN;
    END

    Declare @DepId int ;
    Select @DepId = Id from tblDepartment where tblDepartmentId = @DepartmentId ; 

IF @DepId IS NULL
    BEGIN
        THROW 51001, 'DepartmentId not found or inactive', 1;
        RETURN;
    END

    INSERT INTO tblEmployee
    (
        EmployeeId,
        EmployeeName,
        DepartmentId,
        Salary,
        CreatedOn,
        IsActive
    )
    VALUES
    (
        @EmployeeId,
        @EmployeeName,
        @DepId,
        @Salary,
        GETUTCDATE(),
        1
    );

    SELECT *
    FROM tblEmployee
    WHERE Id = SCOPE_IDENTITY();
END;










//gET ALL EMPLOYEES 


ALTER PROCEDURE [dbo].[usp_GetAllEmployees] --2,10
(
    @PageNumber INT = 1,
    @PageSize   INT = 10
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Offset INT;
    SET @Offset = (@PageNumber - 1) * @PageSize;

    SELECT COUNT(*) AS TotalCount
    FROM tblEmployee e
    WHERE e.IsActive = 1;

    SELECT 
        e.EmployeeId,
        e.tblEmployeeId,
        e.EmployeeName,
        e.Salary,
        d.tblDepartmentId AS DepartmentId,
        d.DepartmentName
    FROM tblEmployee e
    INNER JOIN tblDepartment d 
        ON d.Id = e.DepartmentId
        AND d.IsActive = 1
    WHERE e.IsActive = 1
    ORDER BY e.Id DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

END;
GO





// GET EMPLOYEE BY GUID


ALTER PROCEDURE [dbo].[usp_GetEmployeeById]
(
    @tblEmployeeId NVARCHAR(36)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM tblEmployee
        WHERE tblEmployeeId = @tblEmployeeId
          AND IsActive = 1
    )
    BEGIN
        THROW 51001, 'Employee not found or inactive', 1;
        RETURN;
    END

   SELECT e.EmployeeId ,e.tblEmployeeId ,e.EmployeeName, e.Salary , d.tblDepartmentId as DepartmentId
    , d.DepartmentName
    FROM tblEmployee e 
    inner join tblDepartment d on d.Id = e.DepartmentId and d.IsActive = 1
WHERE e.IsActive = 1 and e.tblEmployeeId = @tblEmployeeId
ORDER BY e.Id DESC;
END;




// update Employee


ALTER   PROCEDURE [dbo].[usp_UpdateEmployee]
(
    @tblEmployeeId nvarchar(36) ,
    @EmployeeId nvarchar(50),
    @EmployeeName NVARCHAR(200),
    @DepartmentId nvarchar(36),
    @Salary INT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM tblEmployee
        WHERE tblEmployeeId = @tblEmployeeId
          AND IsActive = 1
    )
    BEGIN
        THROW 51001, 'Employee not found or inactive', 1;
        RETURN;
    END

    Declare @DepId int ;
    Select @DepId = Id from tblDepartment where tblDepartmentId = @DepartmentId and IsActive = 1

IF @DepId IS NULL
    BEGIN
        THROW 51001, 'DepartmentId not found or inactive', 1;
        RETURN;
    END

    UPDATE tblEmployee
    SET
        EmployeeId = @EmployeeId ,
        EmployeeName = @EmployeeName,
        DepartmentId = @DepId,
        Salary       = @Salary
    WHERE tblEmployeeId = @tblEmployeeId
      AND IsActive = 1;

       SELECT *
    FROM tblEmployee
    WHERE tblEmployeeId = @tblEmployeeId;

END;




// delete the employee 

USE [EmployeeManagement]
GO
/****** Object:  StoredProcedure [dbo].[usp_DeleteEmployee]    Script Date: 14-02-2026 10:20:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [dbo].[usp_DeleteEmployee]
(
    @tblEmployeeId nvarchar(36)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE tblEmployee
    SET
        IsActive  = 0
        WHERE tblEmployeeId = @tblEmployeeId;

      SELECT *
FROM tblEmployee
WHERE tblEmployeeId = @tblEmployeeId;

END;



// insert Department

ALTER OR CREATE PROCEDURE usp_InsertDepartment
(
    @DepartmentName NVARCHAR(300)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM tblDepartment
        WHERE DepartmentName = @DepartmentName
          AND IsActive = 1
    )
    BEGIN
        THROW 51010, 'Department already exists', 1;
        RETURN;
    END

    INSERT INTO tblDepartment
    (
        DepartmentName,
        CreatedOn,
        IsActive
    )
    VALUES
    (
        @DepartmentName,
        GETUTCDATE(),
        1
    );

SELECT *
    FROM tblDepartment
    WHERE Id = SCOPE_IDENTITY();
END;
GO


// UPDATE THE DEPARTMENT

CREATE PROCEDURE usp_UpdateDepartment
(
    @tblDepartmentId NVARCHAR(36),
    @DepartmentName  NVARCHAR(300)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM tblDepartment
        WHERE tblDepartmentId = @tblDepartmentId
          AND IsActive = 1
    )
    BEGIN
        THROW 51020, 'Department not found or inactive', 1;
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM tblDepartment
        WHERE DepartmentName = @DepartmentName
          AND tblDepartmentId <> @tblDepartmentId
          AND IsActive = 1
    )
    BEGIN
        THROW 51021, 'Department name already exists', 1;
        RETURN;
    END

    UPDATE tblDepartment
    SET DepartmentName = @DepartmentName
    WHERE tblDepartmentId = @tblDepartmentId;


	 SELECT *
    FROM tblDepartment
    WHERE tblDepartmentId = @tblDepartmentId;

    END;
    GO


// Delete the Department 


ALTER PROCEDURE usp_DeleteDepartment
(
    @tblDepartmentId NVARCHAR(36)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM tblDepartment
        WHERE tblDepartmentId = @tblDepartmentId
          AND IsActive = 1
    )
    BEGIN
        THROW 51030, 'Department not found or already inactive', 1;
        RETURN;
    END

    UPDATE tblDepartment
    SET IsActive = 0
    WHERE tblDepartmentId = @tblDepartmentId;

    SELECT *
    FROM tblDepartment
    WHERE tblDepartmentId = @tblDepartmentId;
END;




// get all departments 


create PROCEDURE usp_GetAllActiveDepartments
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM tblDepartment
    WHERE IsActive = 1
    ORDER BY CreatedOn DESC;
END;



//get the department by id 

ALTER PROCEDURE usp_GetDepartmentById
(
    @tblDepartmentId NVARCHAR(36)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM tblDepartment
        WHERE tblDepartmentId = @tblDepartmentId
          AND IsActive = 1
    )
    BEGIN
        THROW 51000, 'Department Not Found or Inactive', 1;
        RETURN;
    END

    SELECT *
    FROM tblDepartment
    WHERE tblDepartmentId = @tblDepartmentId
      AND IsActive = 1;
END;


// User table 

CREATE TABLE tblUsers
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    tblUserId NVARCHAR(36) DEFAULT REPLACE(NEWID(), '-', ''),

    Email NVARCHAR(100) UNIQUE NOT NULL,
    Name NVARCHAR(200)  NOT NULL,

    PasswordHash NVARCHAR(256) NOT NULL,

    IsActive BIT DEFAULT 1,
    CreatedOn DATETIME DEFAULT GETUTCDATE()
);


// Register the User 


alter PROCEDURE usp_RegisterUser
(
    @Name NVARCHAR(200),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if email already exists
    IF EXISTS (
        SELECT 1 FROM tblUsers
        WHERE Email = @Email
    )
    BEGIN
        THROW 51000, 'Email already exists', 1;
        RETURN;
    END

    -- Insert user


    INSERT INTO tblUsers
    (
        Name,
        Email,
        PasswordHash,
        IsActive,
        CreatedOn
    )
    VALUES
    (
        @Name,
        @Email,
        @PasswordHash,
        1,
        GETUTCDATE()
    );

    -- Return inserted user
    SELECT *
    FROM tblUsers
    WHERE Id = SCOPE_IDENTITY();
END;





// Login for the User 


ALTER PROCEDURE usp_LoginUser
(
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1 
        FROM tblUsers
        WHERE Email = @Email
    )
    BEGIN
        THROW 51001, 'Email does not exist', 1;
        RETURN;
    END

    IF EXISTS (
        SELECT 1
        FROM tblUsers
        WHERE Email = @Email
          AND IsActive = 0
    )
    BEGIN
        THROW 51002, 'User is inactive', 1;
        RETURN;
    END

    IF NOT EXISTS (
        SELECT 1
        FROM tblUsers
        WHERE Email = @Email
          AND PasswordHash = @PasswordHash
          AND IsActive = 1
    )
    BEGIN
        THROW 51003, 'Invalid password', 1;
        RETURN;
    END

    SELECT TOP 1
        Id,
        tblUserId,
        Email,
        Name,
        CreatedOn
    FROM tblUsers
    WHERE Email = @Email
      AND PasswordHash = @PasswordHash
      AND IsActive = 1;

END;

Things implemented 

Backend : 
.NET , C#

I have added appsetting.json in the .gitignore

Implemented Employee , Department get apis , edit , add , delete apis 
Implemented a api to add the User into the table where I have hashed the password and stored in the table 
While logining the user is validated and checks if the user is valid and is presnet in the table , validate the password and if correct then generate the token 
Implemented JWT token and authorization.

Frontend :

Implemented a Login page to login into the listing page 
used the login cred 
varshita@gmail.com
amdin

I am attaching the screenshot of the login page 

<img width="1868" height="875" alt="image" src="https://github.com/user-attachments/assets/55668837-9c9f-4843-8bf7-8c913c002202" />

on successfull login of the page token is generated and is navigated to the listing page of the Employee screen .
To validate the user I have implemented the Authorization for the Employee , Department APis , so I have implemented the interceptors for the Bearer token 



<img width="1884" height="879" alt="image" src="https://github.com/user-attachments/assets/c9681855-22ed-4395-af6b-e43234670995" />

while clicking on the add of the Employee a add popup is created 
<img width="1843" height="844" alt="image" src="https://github.com/user-attachments/assets/3183f341-bdbd-4f18-9e2e-cb403c4d18a1" />

similarly while editing also a edit popup is created 

<img width="1783" height="762" alt="image" src="https://github.com/user-attachments/assets/2d875e2c-d327-4830-a0da-86dabc2d8b75" />

while deleting of the employee also a confirmation popup to confirm are you sure you want to delete the Employee popup is created with the EmployeeName

<img width="707" height="416" alt="image" src="https://github.com/user-attachments/assets/41a57053-cb0d-48f3-b34f-c8e38e1858b3" />

I have also integrated the Add APi for the Department also so that a newly added Department can be mapped to the employee .
On the click of Add of Department a popup of add is created .
<img width="687" height="327" alt="image" src="https://github.com/user-attachments/assets/0970e124-e994-43eb-88e3-e0b64e621e7a" />

In the frontend I have implemented the Pagination as well for the GetEmployee listing page 

<img width="1574" height="73" alt="image" src="https://github.com/user-attachments/assets/1bc39c32-91f4-4548-85c6-3d6e6a4f218e" />
<img width="1669" height="461" alt="image" src="https://github.com/user-attachments/assets/ff2a5b33-e07d-48b5-a718-e9c446d23623" />

By defult 10 records will come . The pagination is handled in SP level (Backend)



I have implemented API intergration of Employee Add , Edit ,Delete (soft delete ),Get (with Pagination) ,Department (Add) 











