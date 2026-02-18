import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { AlertService } from '../../services/alert.service';
import { Role, RolePermissionService } from '../../services/role-permission.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  standalone: false
})
export class RegisterComponent implements OnInit {

  registerForm!: FormGroup;
  roles: Role[] = [];
  isSubmitting = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private rolepermissionService: RolePermissionService,
    private router: Router,
    private alert: AlertService
  ) { }

  ngOnInit(): void {
    this.buildForm();
    this.loadRoles();
  }

  private buildForm(): void {
    this.registerForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      roleId: ['', Validators.required]
    });
  }

  private loadRoles(): void {
    this.authService.getRoles().subscribe({
      next: (res) => this.roles = res,
      error: () => this.alert.showError('Failed to load roles')
    });
  }

  get f() {
    return this.registerForm.controls;
  }

  onRegister(): void {
    if (this.registerForm.invalid || this.isSubmitting) {
      this.registerForm.markAllAsTouched();
      this.alert.showError("Please enter all required fields!");
      return;
    }

    this.alert.confirm('Do you want to register this user?', () => {
      this.isSubmitting = true;

      this.authService.register(this.registerForm.value).subscribe({
        next: () => {
          this.alert.showInfo('User created successfully!', () => {
            this.router.navigate(['/login']);
          });
          this.isSubmitting = false;
        },
        error: (err) => {
          this.alert.showError(err?.error || 'Something went wrong');
          this.isSubmitting = false;
        }
      });
    });
  }
}
