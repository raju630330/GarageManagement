import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { WorkshopProfileService } from '../workshop-profile.service';
import { Modal } from 'bootstrap';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-booking-appointment',
  standalone: false,
  templateUrl: './booking-appointment.component.html',
  styleUrls: ['./booking-appointment.component.css']
})
export class BookingAppointmentComponent implements OnInit {

  
  appointmentForm!: FormGroup;
  selectedCategory = '';

  serviceAdvisors: string[] = ['Ad1', 'Ad2', 'Ad3'];
  bays: string[] = ['Bay 1', 'Bay 2', 'Bay 3'];

  constructor(private fb: FormBuilder, private http: HttpClient, private workshopProfileService: WorkshopProfileService, private router: Router, private route: ActivatedRoute) { }

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
      mobileNo: ['', [Validators.required, Validators.pattern(/^\+\d{2}\s\d{10}$/)]],
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

    this.workshopProfileService.saveBookAppointment(this.appointmentForm.value).subscribe({
      next: (res) => {
        console.log('Saved successfully', res);
        alert('Data saved successfully');
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
    this.closeModal();
  }

  closeModal() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  
}
