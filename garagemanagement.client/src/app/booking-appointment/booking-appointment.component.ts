import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { WorkshopProfileService } from '../services/workshop-profile.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../services/alert.service';
import { applyValidators, clearValidators } from '../shared/form-utils';

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

  serviceAdvisors: string[] = ['Ram', 'Krishna', 'Madhav','Govind'];
  bays: string[] = ['Pune', 'Nashik', 'Mumbai','Hyderabad','Banglore'];
  vehicleTypes = ['Passenger', 'Commercial'];


  constructor(private fb: FormBuilder, private http: HttpClient, private WorkshopProfileService: WorkshopProfileService, private router: Router, private route: ActivatedRoute, private alert: AlertService) { }

  ngOnInit() {
    this.appointmentForm = this.fb.group({
      search: ['', Validators.required],
      date: ['', Validators.required],
      time: ['', Validators.required],
      customerType: ['', Validators.required],
      regPrefix: ['TS'],
      regNo: [''],
      vehicleType: [''],
      customerName: [''],
      mobileNo: [''],
      emailID: [''],
      service: [''],
      serviceAdvisor: [''],
      settings: [''],
      bay: ['']
    });

    this.appointmentForm.get('customerType')?.valueChanges.subscribe(type => {
      if (type === 'Individual') {
        applyValidators(this.appointmentForm, ['regNo'], [
          Validators.required,
          Validators.pattern(/^[0-9]{2}[A-Z]{2}[0-9]{4}$/)
        ]);

        applyValidators(this.appointmentForm, ['vehicleType', 'serviceAdvisor', 'bay'], [
          Validators.required
        ]);

        applyValidators(this.appointmentForm, ['customerName'], [
          Validators.required,
          Validators.pattern(/^[A-Za-z ]+$/),
          Validators.minLength(2),
          Validators.maxLength(25)
        ]);

        applyValidators(this.appointmentForm, ['mobileNo'], [
          Validators.required,
          Validators.pattern(/^\+\d{2}\s\d{10}$/)
        ]);

        applyValidators(this.appointmentForm, ['emailID'], [
          Validators.required,
          Validators.email
        ]);

      } else {
        clearValidators(this.appointmentForm, [
          'regNo', 'vehicleType', 'customerName', 'mobileNo',
          'emailID', 'service', 'serviceAdvisor', 'settings', 'bay'
        ]);
      }
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
      this.alert.showError('Please fix validation errors before submitting!');
      return;
    }

    const bookingData = this.appointmentForm.value;

    this.WorkshopProfileService.saveBookAppointment(bookingData).subscribe({
      next: (res) => {
        this.alert.showInfo(res.message || 'Booking Appointment Data saved successfully!', () => {
          this.router.navigate(['/Calendar']);
        });       
      },
      error: (err) => {
        this.alert.showError(err?.error || 'Error in saving Booking Appointment!');
      }
    });
  }
  closeModal() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

}
