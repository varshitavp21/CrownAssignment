import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginService } from '../services/login.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})


export class LoginComponent {

   loginForm: FormGroup;
  constructor(private fb: FormBuilder ,private loginService : LoginService , private router: Router ,private toasterService : ToastrService) {

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],

      password: ['', [
        Validators.required,
        Validators.minLength(3)
      ]]
    });
  }

  onLogin() {

  if (this.loginForm.invalid) {
    return;
  }

  const loginData = {
    email: this.loginForm.value.email,
    password: this.loginForm.value.password
  };
  console.log("kjabs")

  this.loginService.loginUser(loginData).subscribe({
    next: (response: any) => {
      localStorage.setItem("token", response?.response?.token);
      this.toasterService.success("Login successful");
       this.loginForm.reset();
      this.router.navigate(['/employee']);
    },
    error: (err: any) => {
      this.toasterService.error("Something went wrong!")
      console.log(err);
    }
  });
}

}
