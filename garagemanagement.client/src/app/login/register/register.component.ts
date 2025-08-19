import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../auth.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  userName = '';
  email = '';
  password = '';

  constructor(private authService: AuthService, private router: Router) { }

  onRegister() {
    this.authService.register({
      username: this.userName,
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        alert('Registered successfully. Please login.');
        this.router.navigate(['/login']);
      },
      error: (err) => alert(err.error)
    });
  }

}
