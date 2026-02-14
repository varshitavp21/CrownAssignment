using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeModels.Models
{
    public class EmployeeDto
    {
        public string tblEmployeeId {  get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentId { get; set; }
        public int Salary { get; set; }
    }

    public class Employee
    {
        public string tblEmployeeId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Salary { get; set; }
    }

    public class EmployeeList
    {
        public int TotalCount { get; set; }
        public List<Employee> Employees { get; set; }
    }

    public class UpdateDepartmentDto
    {
        public string tblDepartmentId { get; set; }   
        public string DepartmentName { get; set; }
    }


    public class DepartmentDto
    {
        public string DepartmentName { get; set; }
    }

    public class Department
    {
        public string tblDepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
