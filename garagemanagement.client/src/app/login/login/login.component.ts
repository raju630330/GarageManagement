import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginForm!: FormGroup;
  error = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      usernameOrEmail: ['', [Validators.required,
        Validators.minLength(2), Validators.pattern("^[^\\s]+$")]],
      password: ['', [Validators.required, Validators.pattern("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,15}$")]]
    });
  }

  submit() {

    if (this.loginForm.invalid) {
      this.error = 'Enter username/email and password';
      return;
    }

    this.authService.login(this.loginForm.value).subscribe({

      next: () => {
        this.router.navigate(['/']);
      },

      error: (err) => {

        if (err.error) {
          this.error = err.error;   // message from backend
        } else {
          this.error = 'Login failed. Please try again.';
        }

      }

    });

  }
}
