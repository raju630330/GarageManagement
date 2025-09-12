import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { WorkshopProfileService } from '../workshop-profile.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-booking-appointment',
  standalone: false,
  templateUrl: './booking-appointment.component.html',
  styleUrl: './booking-appointment.component.css'
})
export class BookingAppointmentComponent {

  appointmentForm!: FormGroup;
  minDate = new Date();
  showModal = false;
  selectedCategory = '';

  serviceAdvisors: string[] = ['John Doe', 'Jane Smith', 'Robert Brown'];
  bays: string[] = ['Bay 1', 'Bay 2', 'Bay 3'];
   

  constructor(private fb: FormBuilder, private http: HttpClient, private WorkshopProfileService: WorkshopProfileService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.appointmentForm = this.fb.group({
      search: ['', [Validators.required]],
      date: ['', [Validators.required]],
      time: ['', [Validators.required]],
      customerType: ['Individual', Validators.required],
      regNo: ['', [Validators.required, Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}\s{2}$/)]],
      vehicle: ['', [Validators.required, Validators.pattern(/^[A-Za-z]+$/)]],
      customerName: ['',[Validators.required, Validators.pattern(/^[A-Za-z ]+$/)]],
      mobileNo: ['', [Validators.required, Validators.pattern(/^\+\d{2}\s\d{10}$/)]],
      emailID: ['', [Validators.required, Validators.email]],
      service: ['', [Validators.required]],
      serviceAdvisor: ['', [Validators.required]],
      settings: ['', [Validators.required, Validators.pattern(/^[A-Za-z]$/)]],
      bay: ['', [Validators.required]]
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
    this.closeModal();
  }
  closeModal() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

}
