import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WorkshopProfileService } from './workshop-profile.service';


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

  appointmentForm!: FormGroup;
  showModal = false;
  selectedCategory = '';

  serviceAdvisors: string[] = ['John Doe', 'Jane Smith', 'Robert Brown'];
  bays: string[] = ['Bay 1', 'Bay 2', 'Bay 3'];

  constructor(private fb: FormBuilder, private http: HttpClient, private WorkshopProfileService: WorkshopProfileService) { }

  ngOnInit(): void {
    this.profileForm = this.fb.group({
      WorkshopName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      OwnerName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      OwnerMobileNo: ['', [Validators.required, Validators.pattern(/^\+\d{2}\s\d{10}$/)]],
      EmailID: ['', [Validators.required, Validators.email]],
      ContactPerson: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(25)]],
      ContactNo: ['', [Validators.required, Validators.pattern(/^\+\d{2}\s\d{10}$/)]],
      Landline: ['', [Validators.required, Validators.pattern(/^\d{3,5}-\d{6,8}$/)]],
      CalendarDate: ['', [Validators.required]]
    });


    this.appointmentForm = this.fb.group({
      search: ['', Validators.required],
      date: ['', Validators.required],
      time: ['', Validators.required],
      customerType: ['Individual', Validators.required],
      state: ['', Validators.required],
      regNo: ['', Validators.required],
      vehicle: ['', Validators.required],
      customerName: ['', Validators.required],
      mobileNo: ['', [Validators.required, Validators.pattern(/^[6-9]\d{9}$/)]],
      emailID: ['', [Validators.required, Validators.email]],
      service: ['', Validators.required],
      serviceAdvisor: ['', Validators.required],
      settings: ['', Validators.required],
      bay: ['', Validators.required]
    });
  }

  onSubmit(): void {
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
  selectCustomerType(type: string) {
    this.appointmentForm.get('customerType')?.setValue(type);
  }

  onbookSubmit() {
    if (this.appointmentForm.invalid) {
      this.appointmentForm.markAllAsTouched();
      console.log('Form is invalid');
      return;
    }

    console.log('Form submitted successfully:', this.appointmentForm.value);

    const bookingData = this.appointmentForm.value;

    this.WorkshopProfileService.saveBookAppointment(bookingData).subscribe({
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
  openModal() {
    this.showModal = true;
  }

  close() {
    this.showModal = false;
  }
}
