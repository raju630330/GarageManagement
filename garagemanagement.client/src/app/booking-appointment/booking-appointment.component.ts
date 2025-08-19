import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { WorkshopProfileService } from '../workshop-profile.service';

@Component({
  selector: 'app-booking-appointment',
  standalone: false,
  templateUrl: './booking-appointment.component.html',
  styleUrl: './booking-appointment.component.css'
})
export class BookingAppointmentComponent {

  appointmentForm!: FormGroup;
  showModal = false;
  selectedCategory = '';

  serviceAdvisors: string[] = ['John Doe', 'Jane Smith', 'Robert Brown'];
  bays: string[] = ['Bay 1', 'Bay 2', 'Bay 3'];

  constructor(private fb: FormBuilder, private http: HttpClient, private WorkshopProfileService: WorkshopProfileService) { }

  ngOnInit(): void {
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
