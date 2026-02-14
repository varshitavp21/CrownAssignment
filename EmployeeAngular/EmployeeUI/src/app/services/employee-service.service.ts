import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { URLS } from '../constant';
import { Department, Employee } from '../interface';

@Injectable({
  providedIn: 'root'
})
export class EmployeeServiceService {

    constructor(private http: HttpClient) {}

  // getAllEmployees(): Observable<Employee[]> {
  //   return this.http.get<Employee[]>(
  //     `${URLS.ApiEndPoint}/${URLS.Employee.getAllEmployee}`
  //   );
  // }
  getAllEmployees(pageNumber: number = 1, pageSize: number = 10): Observable<any> {

  return this.http.get<any>(
    `${URLS.ApiEndPoint}/${URLS.Employee.getAllEmployee}?pageNumber=${pageNumber}&pageSize=${pageSize}`
  );

}


  getAllDepartment(): Observable<Department[]> {
    return this.http.get<Department[]>(
      `${URLS.ApiEndPoint}/${URLS.Department.getAllDepartment}`
    );
  }

  insertEmployee(data: any): Observable<any> {
    return this.http.post(
      `${URLS.ApiEndPoint}/${URLS.Employee.insertEmployee}`,
      data
    );
  }

  insertDepartment(data: any): Observable<any> {
    return this.http.post(
      `${URLS.ApiEndPoint}/${URLS.Department.insertDepartment}`,
      data
    );
  }

  updateEmployee(data: Employee): Observable<any> {
    return this.http.post(
      `${URLS.ApiEndPoint}/${URLS.Employee.updateEmployee}`,
      data
    );
  }

  deleteEmployee(tblEmployeeId: string): Observable<any> {
    return this.http.delete(
      `${URLS.ApiEndPoint}/${URLS.Employee.deleteEmployee}?employeeId=${tblEmployeeId}`
    );
  }
}
