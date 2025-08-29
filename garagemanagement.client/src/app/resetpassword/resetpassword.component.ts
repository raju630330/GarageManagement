import { Component,OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-resetpassword',
  standalone: false,
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.css']
})
export class ResetpasswordComponent implements OnInit {
  form!: FormGroup;
  token: string = '';
  email: string = '';
  msg = '';
  error = '';

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) { }

  showPassword = false;
  showConfirmPassword = false;

  togglePassword() {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPassword() {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    });

    this.route.queryParams.subscribe(params => {
      this.token = params['token'] || '';
      this.email = params['email'] || '';
    });
  }

  submit(): void {
    this.error = '';
    this.msg = '';

    if (this.form.invalid) {
      this.error = 'Please enter a valid password (min 8 characters).';
      return;
    }

    if (this.form.value.newPassword !== this.form.value.confirmPassword) {
      this.error = 'Passwords do not match';
      return;
    }

    if (!this.token || !this.email) {
      this.error = 'Invalid password reset link.';
      return;
    }

    const resetData = {
      token: this.token,
      email: this.email,
      newPassword: this.form.value.newPassword
    };

    this.authService.resetPassword(resetData).subscribe({
      next: (res) => {
        this.msg = res?.message || 'Password reset successful!';
        this.router.navigate(['/login']);
      },
      error: (err) => {
        if (err?.error?.text && err.status === 200) {
          this.msg = err.error.text; // treat as success
          setTimeout(() => this.router.navigate(['/login']), 2000);
        } else {
          this.error = err?.error?.message
            || (err?.error && JSON.stringify(err.error))
            || 'Failed to reset password.';
        }
      }
    });

  }
}
