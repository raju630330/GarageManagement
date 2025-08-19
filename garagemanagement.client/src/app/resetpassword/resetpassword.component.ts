import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-resetpassword',
  standalone: false,
  templateUrl: './resetpassword.component.html',
  styleUrl: './resetpassword.component.css'
})
export class ResetpasswordComponent {

  form!: FormGroup;
  msg = ''; error = '';

  constructor(private fb: FormBuilder, private authService: AuthService, private route: ActivatedRoute, private router: Router) {
    const q = this.route.snapshot.queryParamMap;
    const token = q.get('token');
    const user = q.get('user');
    if (token) this.form.patchValue({ token });
    if (user) this.form.patchValue({ emailOrUsername: user });
  }
  ngOnInit(): void {
    this.form = this.fb.group({
      emailOrUsername: ['', Validators.required],
      token: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6)]]
    });
  }


  submit() {
    this.msg = ''; this.error = '';
    const { emailOrUsername, token, newPassword } = this.form.value as any;
    this.authService.reset(emailOrUsername, token, newPassword).subscribe({
      next: res => { this.msg = res.message; setTimeout(() => this.router.navigate(['/login']), 800); },
      error: () => this.error = 'Invalid or expired token'
    });
  }
}


