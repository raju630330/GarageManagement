import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../services/alert.service';
import { WorkshopProfileService } from '../services/workshop-profile.service';
import { applyValidators, clearValidators } from '../shared/form-utils';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-booking-appointment',
  templateUrl: './booking-appointment.component.html',
  styleUrl: './booking-appointment.component.css',
  standalone: false
})
export class BookingAppointmentComponent {

  appointmentForm!: FormGroup;
  minDate = new Date();

  serviceAdvisors = ['Ram', 'Krishna', 'Madhav', 'Govind'];
  bays = ['Pune', 'Nashik', 'Mumbai', 'Hyderabad', 'Banglore'];
  vehicleTypes = ['Passenger', 'Commercial'];

  // Selected IDs (IMPORTANT)
  selectedCustomerId!: number;
  selectedVehicleId: number | null = null;

  constructor(
    private fb: FormBuilder,
    public service: WorkshopProfileService,
    private router: Router,
    private route: ActivatedRoute,
    private alert: AlertService
  ) { }

  ngOnInit() {
    this.appointmentForm = this.fb.group({
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

    // Dynamic validators
    this.appointmentForm.get('customerType')?.valueChanges.subscribe(type => {
      if (type === 'Individual') {
        applyValidators(this.appointmentForm, ['regNo'], [
          Validators.required,
          Validators.pattern(/^[0-9]{2}[A-Z]{2}[0-9]{4}$/)
        ]);
        applyValidators(this.appointmentForm, ['vehicleType','service','serviceAdvisor', 'bay'], [Validators.required]);
        applyValidators(this.appointmentForm, ['customerName'], [Validators.required]);
        applyValidators(this.appointmentForm, ['mobileNo'], [Validators.required]);
        applyValidators(this.appointmentForm, ['emailID'], [Validators.required, Validators.email]);
      } else {
        clearValidators(this.appointmentForm, [
          'regNo', 'vehicleType', 'customerName',
          'mobileNo', 'emailID', 'service','serviceAdvisor', 'bay'
        ]);
      }
    });
  }

  // =============================
  // AUTOCOMPLETE FUNCTIONS
  // =============================

  onCustomerSelected(item: any) {
    const customerId = item.id;

    this.service.getCustomerDetails(customerId).subscribe((res : any) => {
      const vehicle = res.vehicles?.[0];

      this.selectedCustomerId = res.id;
      this.selectedVehicleId = vehicle?.id ?? null;

      this.appointmentForm.patchValue({
        customerType: res.customerType,
        customerName: res.customerName,
        mobileNo: res.mobileNo,
        emailID: res.email,
        regPrefix: vehicle?.regPrefix ?? 'TS',
        regNo: vehicle?.regNo ?? '',
        vehicleType: vehicle?.vehicleType ?? ''
      });
    });
  }

  resetAppointmentForm() {
    this.selectedCustomerId = 0;
    this.selectedVehicleId = null;
    this.appointmentForm.reset({
      regPrefix: 'TS'
    });
  }

  // =============================
  // SAVE
  // =============================
  onbookSubmit() {
    if (this.appointmentForm.invalid || !this.selectedCustomerId) {
      this.appointmentForm.markAllAsTouched();
      this.alert.showError('Please complete all required fields');
      return;
    }

    const payload = {
      CustomerId: this.selectedCustomerId,
      VehicleId: this.selectedVehicleId,
      AppointmentDate: this.appointmentForm.value.date,
      AppointmentTime: this.appointmentForm.value.time,
      CustomerType: this.appointmentForm.value.customerType,
      Service: this.appointmentForm.value.service,
      ServiceAdvisor: this.appointmentForm.value.serviceAdvisor,
      Bay: this.appointmentForm.value.bay,
      UserId: 2,          // as discussed
      WorkshopId: 1       // fixed / from login later
    };

    this.service.saveBookAppointment(payload).subscribe({
      next: (res) => {
        this.alert.showInfo(res.message || 'Booking Appointment saved successfully', () => {
          this.router.navigate(['/Calendar']);
        });
      },
      error: err => {
        this.alert.showError(err?.error?.message || 'Error saving booking appointment');
      }
    });
  }

  closeModal() {
    this.router.navigate(['../'], { relativeTo: this.route });
  }

  get f() {
    return this.appointmentForm.controls;
  }
}
