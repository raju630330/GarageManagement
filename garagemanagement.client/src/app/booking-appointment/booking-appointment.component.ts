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
  vehicleTypes = ['Passenger', 'Commercial'];


  constructor(private fb: FormBuilder, private http: HttpClient, private WorkshopProfileService: WorkshopProfileService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.appointmentForm = this.fb.group({
      search: ['', Validators.required],
      date: ['', Validators.required],
      time: ['', Validators.required],
      customerType: ['', Validators.required],
      regPrefix: ['TS'],
      regNo: ['', [
        Validators.required,
        Validators.pattern(/^[A-Z ]{2}[0-9]{6}$/) 
      ]],
      vehicleType: ['', Validators.required],
      customerName: ['', [
        Validators.required,
        Validators.pattern(/^[A-Za-z ]+$/),
        Validators.minLength(2),
        Validators.maxLength(25)
      ]],
      mobileNo: ['', [
        Validators.required,
        Validators.pattern(/^\+\d{2}\s\d{10}$/)
      ]],
      emailID: ['', [
        Validators.required,
        Validators.email
      ]],
      service: ['',[ Validators.required, Validators.pattern(/^[A-Za-z ]+$/)]],
      serviceAdvisor: ['', Validators.required],
      settings: ['',[Validators.required, Validators.pattern(/^[A-Za-z ]+$/)]],
      bay: ['', Validators.required]
    });
  }

  selectCustomerType(type: string) {
    this.appointmentForm.get('customerType')?.setValue(type);
  }
  get f() {
    return this.appointmentForm.controls;
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
