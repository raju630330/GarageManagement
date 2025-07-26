import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  profileForm!: FormGroup;
  submitted = false;


  constructor(private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      workshopName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      ownerName: ['',[Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      ownerMobile: ['', [Validators.required, Validators.pattern(/^\+\d{2}\s\d{10}$/)]],
      email: ['', [Validators.required, Validators.email]],
      contactPersonName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      contactNo: ['', [Validators.required, Validators.pattern(/^\+\d{2}\s\d{10}$/)]],
      landline: ['',[Validators.required, Validators.pattern(/^\d{3,5}-\d{6,8}$/)]],
      calendar: ['', [Validators.required]]
    });
  }
  get f() {
    return this.profileForm.controls;
  }

  onSubmit(): void {
    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      return;
    }
  }
}

