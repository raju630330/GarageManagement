import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgetpassword',
  standalone: false,
  templateUrl: './forgetpassword.component.html',
  styleUrl: './forgetpassword.component.css'
})
export class ForgetpasswordComponent implements OnInit {
  //form!: FormGroup;

  //msg: string = '';
  //error: string = '';
  //devToken: string = '';
  //constructor(private fb: FormBuilder, private authService: AuthService) { }

  //ngOnInit(): void {
  //  this.form = this.fb.group({
  //    emailOrUsername: ['', [Validators.required, Validators.email]]
  //  });
  //}

  //submit() {
  //  if (this.form.invalid) return;

  //  const value = this.form.value.emailOrUsername;

  //  this.authService.forgotPassword(value).subscribe(res=>
  //    {
  //    if (res.exists) {
  //      this.msg = value;

  //  } else {
  //    this.error = "Email not found";
  //  }
  //});
  //}

  form!: FormGroup;
  msg: string = '';
  error: string = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  checkEmail() {
    this.error = '';
    this.msg = '';

    if (this.form.invalid) {
      this.error = 'Please enter a valid email.';
      return;
    }

    const email = this.form.value.email;

    this.authService.forgotPassword(email).subscribe({
      next: (res: any) => {
        if (res.exists) {
          this.msg = 'Email verified. Redirecting to reset password...';
          // Pass email to reset-password route
          setTimeout(() => {
            this.router.navigate(['/reset-password'], { queryParams: { email } });
          }, 1000);
        } else {
          this.error = 'Email not found.';
        }
      },
      error: () => {
        this.error = 'Something went wrong. Try again.';
      }
    });
  }
}

