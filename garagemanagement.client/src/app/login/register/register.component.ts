import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AlertService } from '../../services/alert.service';

@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  registerForm!: FormGroup;
  error = "";


  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private alert: AlertService) { }

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
      roleId: ['', [Validators.required]]
    });
  }

  onRegister() {
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.alert.showError('Please enter all required fields');    
      return;
    }

    this.alert.confirm('Do you want to register this user?', () => {
      this.authService.register(this.registerForm.value).subscribe({
        next: (res) => {
          this.alert.showInfo('User created successfully! <br> Click OK to login', () => {
            this.router.navigate(['/login']);
          });
        },
        error: (err) => {
          this.alert.showError(err?.error || 'Something went wrong');
        }
      });
    });
  }
}
