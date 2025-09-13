import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WorkshopProfileService } from '../workshop-profile.service';


@Component({
  selector: 'app-profile',
  standalone: false,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent {
  profileForm!: FormGroup;
  submitted = false;
  responsemessage = '';
  minDate = new Date();

  constructor(private fb: FormBuilder, private http: HttpClient, private WorkshopProfileService: WorkshopProfileService) { }

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      WorkshopName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
      OwnerName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
      OwnerMobileNo: ['', [Validators.required, Validators.pattern(/^\+91\d{10}$/)]],
      EmailID: ['', [Validators.required, Validators.email]],
      ContactPerson: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25), Validators.pattern(/^[A-Za-z ]+$/)]],
      ContactNo: ['', [Validators.required, Validators.pattern(/^\+91\d{10}$/)]],
      Landline: ['', [Validators.required, Validators.pattern(/^\d{3,5}-\d{6,8}$/)]],
      CalendarDate: ['', [Validators.required]]
    });

  }
  get f() {
    return this.profileForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.profileForm.invalid) {
      this.profileForm.markAllAsTouched();
      console.log('Form is invalid');
      return;
    }

    console.log('Form submitted successfully:', this.profileForm.value);

    const profileData = this.profileForm.value;

    this.WorkshopProfileService.saveProfile(profileData).subscribe({
      next: (res) => {
        console.log('Saved successfully', res);
        alert('Data Save successfully');
      },
      error: (err) => {
        console.error('Error:', err);
        if (err.error?.errors) {
          for (let field in err.error.errors) {
            console.error(`${field}: ${err.error.errors[field].join(', ')}`);
          }
        }
      }
    });
  }
  onReset() {
    this.submitted = false;
    this.profileForm.reset();
  }
}


 
