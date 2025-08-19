import { Component } from '@angular/core';
import { Validators, FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-forgetpassword',
  standalone: false,
  templateUrl: './forgetpassword.component.html',
  styleUrl: './forgetpassword.component.css'
})
export class ForgetpasswordComponent {
  form!: FormGroup;
  msg = ''; devToken: string | null = null; error = '';

constructor(private fb: FormBuilder, private authService: AuthService) { }

ngOnInit (): void {
  this.form = this.fb.group({
    emailOrUsername: ['', Validators.required]
  });
}

  submit() {
    this.msg = ''; this.error = ''; this.devToken = null;
    this.authService.forgot(this.form.value.emailOrUsername!).subscribe({
      next: res => { this.msg = res.message; this.devToken = res.devToken ?? null; },
      error: () => this.error = 'Something went wrong'
    });
  }

}
