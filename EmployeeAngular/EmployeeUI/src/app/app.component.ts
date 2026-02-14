import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'EmployeeUI';

  constructor(private router: Router) {}
 logout() {
  localStorage.removeItem("token");
  this.router.navigate(['/login']);

}
}
