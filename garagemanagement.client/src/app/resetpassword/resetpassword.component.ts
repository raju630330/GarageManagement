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
  //form!: FormGroup;
  //token: string = '';
  //email: string = '';
  //msg = '';
  //error = '';

  //constructor(
  //  private fb: FormBuilder,
  //  private route: ActivatedRoute,
  //  private authService: AuthService,
  //  private router: Router
  //) { }

  //showPassword = false;
  //showConfirmPassword = false;

  //togglePassword() {
  //  this.showPassword = !this.showPassword;
  //}

  //toggleConfirmPassword() {
  //  this.showConfirmPassword = !this.showConfirmPassword;
  //}

  //ngOnInit(): void {
  //  this.form = this.fb.group({
  //    newPassword: ['', [Validators.required, Validators.minLength(8)]],
  //    confirmPassword: ['', Validators.required]
  //  });

  //  this.route.queryParams.subscribe(params => {
  //    this.token = params['token'] || '';
  //    this.email = params['email'] || '';
  //  });
  //}

  //submit(): void {
  //  this.error = '';
  //  this.msg = '';

  //  if (this.form.invalid) {
  //    this.error = 'Please enter a valid password (min 8 characters).';
  //    return;
  //  }

  //  if (this.form.value.newPassword !== this.form.value.confirmPassword) {
  //    this.error = 'Passwords do not match';
  //    return;
  //  }

  //  if (!this.token || !this.email) {
  //    this.error = 'Invalid password reset link.';
  //    return;
  //  }

  //  const resetData = {
  //    token: this.token,
  //    email: this.email,
  //    newPassword: this.form.value.newPassword
  //  };

  //  this.authService.resetPassword(resetData).subscribe({
  //    next: (res) => {
  //      this.msg = res?.message || 'Password reset successful!';
  //      this.router.navigate(['/login']);
  //    },
  //    error: (err) => {
  //      if (err?.error?.text && err.status === 200) {
  //        this.msg = err.error.text; // treat as success
  //        setTimeout(() => this.router.navigate(['/login']), 2000);
  //      } else {
  //        this.error = err?.error?.message
  //          || (err?.error && JSON.stringify(err.error))
  //          || 'Failed to reset password.';
  //      }
  //    }
  //  });

  //}
  form!: FormGroup;
  msg: string = '';
  error: string = '';
  email: string = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Get email from query params
    this.route.queryParams.subscribe(params => {
      this.email = params['email'] || '';
    });

    this.form = this.fb.group({
      email: [{ value: this.email, disabled: true }], // readonly field
      newPassword: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', Validators.required]
    });
  }

  resetPasswordM() {
    this.error = '';
    this.msg = '';

    if (this.form.invalid) {
      this.error = 'Please fill all fields correctly.';
      return;
    }

    if (this.form.value.newPassword !== this.form.value.confirmPassword) {
      this.error = 'Passwords do not match.';
      return;
    }

    const data = {
      emailOrUsername: this.email,  // match backend property
      newPassword: this.form.value.newPassword,
      confirmPassword: this.form.value.confirmPassword
    };

    this.authService.resetPassword(data).subscribe({
      next: (res: any) => {
        this.msg = res?.message || 'Password changed successfully!';
        this.error = '';
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1500);
      },
      error: (err) => {
        this.error = err?.error?.message || 'Failed to change password.';
      }
    });
  }
}

