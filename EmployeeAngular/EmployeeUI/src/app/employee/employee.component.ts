import { Component } from '@angular/core';
import { Department, Employee } from '../interface';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EmployeeServiceService } from '../services/employee-service.service';

@Component({
  selector: 'app-employee',
  standalone: false,
  templateUrl: './employee.component.html',
  styleUrl: './employee.component.css'
})
export class EmployeeComponent {

  employees: Employee[] = [];
  departments: Department[] = [];
  currentPage: number = 1;
pageSize: number = 10;
totalEmployees: number = 0;
totalPages: number = 0;


  // Employee Popup
  showPopup = false;
  isEditMode = false;

  // Delete Popup
  showDeletePopup = false;
  employeeToDelete: Employee | null = null;

  // Department Popup
  showDepartmentPopup = false;

  employeeForm!: FormGroup;
  departmentForm!: FormGroup;

  selectedEmployeeId: string | null = null;

  constructor(
    private empService: EmployeeServiceService,
    private fb: FormBuilder,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {

    this.loadEmployees();
    this.loadDepartments();

    // Employee Form
    this.employeeForm = this.fb.group({
      employeeId: ["", Validators.required],
      employeeName: ["", Validators.required],
      departmentId: ["", Validators.required],
      salary: ["", Validators.required]
    });

    // Department Form
    this.departmentForm = this.fb.group({
      departmentName: ["", Validators.required]
    });
  }

  // loadEmployees() {
  //   this.empService.getAllEmployees().subscribe({
  //     next: (res: any) => {
  //       this.employees = res?.response;
  //     },
  //     error: () => this.toastr.error("Failed to load employees")
  //   });
  // }
  loadEmployees() {
  this.empService.getAllEmployees(this.currentPage, this.pageSize)
.subscribe({
  next: (res: any) => {
    this.employees = res.response.employees;
    this.totalEmployees = res.response.totalCount;
    this.totalPages = Math.ceil(this.totalEmployees / this.pageSize);
  }
});

}

goToPage(page: number) {
  if (page < 1 || page > this.totalPages) return;
  this.currentPage = page;
  this.loadEmployees();
}

nextPage() {
  if (this.currentPage < this.totalPages) {
    this.currentPage++;
    this.loadEmployees();
  }
}

prevPage() {
  if (this.currentPage > 1) {
    this.currentPage--;
    this.loadEmployees();
  }
}


  loadDepartments() {
    this.empService.getAllDepartment().subscribe({
      next: (res: any) => {
        this.departments = res?.response;
      },
      error: () => this.toastr.error("Failed to load departments")
    });
  }

  openAddPopup() {
    this.showPopup = true;
    this.isEditMode = false;
    this.employeeForm.reset();
  }

  openEditPopup(emp: Employee) {

    this.showPopup = true;
    this.isEditMode = true;
    this.selectedEmployeeId = emp.tblEmployeeId;

    this.employeeForm.patchValue({
      employeeId: emp.employeeId,
      employeeName: emp.employeeName,
      departmentId: emp.departmentId,
      salary: emp.salary
    });
  }

  // Close Employee Popup
  closePopup() {
    this.showPopup = false;
  }

  saveEmployee() {

    if (this.employeeForm.invalid) {
      this.toastr.warning("Please fill all fields");
      return;
    }

    if (this.isEditMode) {

      const updatedPayload = {
        tblEmployeeId: this.selectedEmployeeId,
        ...this.employeeForm.value
      };

      this.empService.updateEmployee(updatedPayload).subscribe({
        next: () => {
          this.toastr.success("Employee Updated Successfully");
          this.loadEmployees();
          this.closePopup();
        },
        error: () => this.toastr.error("Update Failed")
      });

    }

    else {

      const payload = {
        tblEmployeeId: "",
        ...this.employeeForm.value
      };

      this.empService.insertEmployee(payload).subscribe({
        next: () => {
          this.toastr.success("Employee Added Successfully");
          this.loadEmployees();
          this.closePopup();
        },
        error: () => this.toastr.error("Insert Failed")
      });
    }
  }

  openDeletePopup(emp: Employee) {
    this.employeeToDelete = emp;
    this.showDeletePopup = true;
  }

  closeDeletePopup() {
    this.showDeletePopup = false;
    this.employeeToDelete = null;
  }

  confirmDelete() {

    if (!this.employeeToDelete) return;

    this.empService.deleteEmployee(this.employeeToDelete.tblEmployeeId)
      .subscribe({
        next: () => {
          this.toastr.success("Employee Deleted Successfully");
          this.loadEmployees();
          this.closeDeletePopup();
        },
        error: () => this.toastr.error("Delete Failed")
      });
  }
  openDepartmentPopup() {
    this.showDepartmentPopup = true;
    this.departmentForm.reset();
  }

  closeDepartmentPopup() {
    this.showDepartmentPopup = false;
  }

  saveDepartment() {

    if (this.departmentForm.invalid) {
      this.toastr.warning("Please enter Department Name");
      return;
    }

    const payload = {
      // tblDepartmentId: "",
      departmentName: this.departmentForm.value.departmentName
    };

    this.empService.insertDepartment(payload).subscribe({
      next: () => {
        this.toastr.success("Department Added Successfully");
        this.loadDepartments();
        this.closeDepartmentPopup();
      },
      error: () => this.toastr.error("Department Insert Failed")
    });
  }
}
