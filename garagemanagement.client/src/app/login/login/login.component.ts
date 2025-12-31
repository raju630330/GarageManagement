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
      password: ['', [Validators.required, Validators.pattern("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")]]
    });
  }

submit() {
  if (this.loginForm.invalid) { this.error = 'Enter credentials'; return; }

  this.authService.login(this.loginForm.value).subscribe({
    next: () => this.router.navigate(['//settings']),
    error: () => this.error = 'Invalid username/email or password'
  });
  
}
}
