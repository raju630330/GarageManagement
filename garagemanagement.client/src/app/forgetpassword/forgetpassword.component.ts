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
  form!: FormGroup;

  msg: string = '';
  error: string = '';
  devToken: string = '';
  constructor(private fb: FormBuilder, private authService: AuthService) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      emailOrUsername: ['', [Validators.required, Validators.email]]
    });
  }

  submit() {
    if (this.form.invalid) return;

    const value = this.form.value.emailOrUsername;

    this.authService.forgotPassword(value).subscribe(res=>
      {
      if (res.exists) {
        this.msg = value;

    } else {
      this.error = "Email not found";
    }
  });
  }
}

