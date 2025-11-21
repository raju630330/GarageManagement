import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm!: FormGroup;
  error = "";
  role = ['Driver', 'Admin', 'Technician','Cashier','Manager','Supervisor'];
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.pattern(/^[A-Za-z ]+$/)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$/)
      ]
      ],
      role: ['', [Validators.required]]
    });
  }

  onRegister() {
    if (this.registerForm.invalid) { this.error = 'Enter credentials'; return; }

    this.authService.register(this.registerForm.value).subscribe({
      next: (res) => {
        alert(res?.message || 'Registered successfully');
        this.router.navigate(['/login']); 
      },
      error: (err) => {
        alert(err.error?.message || 'Something went wrong');
      }
    });

  }
}
