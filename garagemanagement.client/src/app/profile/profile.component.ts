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
      workshopName: ['', Validators.required],
      ownerName: ['', Validators.required],
      ownerMobile: ['', [Validators.required, Validators.pattern(/^[6-9]\d{9}$/)]],
      email: ['', [Validators.email]],
      contactPerson: [''],
      contactNo: [''],
      landline: [''],
      calendar: ['']
    });
  }

  // Submit handler
  onSubmit(): void {
    this.submitted = true;

    if (this.profileForm.invalid) {
      return;
    }

    
  }

  // To access form controls easily in HTML
  get f() {
    return this.profileForm.controls;
  }
}

