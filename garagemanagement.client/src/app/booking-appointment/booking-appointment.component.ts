import { Component, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../services/alert.service';
import { WorkshopProfileService } from '../services/workshop-profile.service';
import { applyValidators, clearValidators } from '../shared/form-utils';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';

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


  //Edit
  @Input() selectedDate: string | null = null;
  @Input() selectedTime: string | null = null;

  bookingId: number | null = null;
  isEditMode = false;
  constructor(
    private fb: FormBuilder,
    public service: WorkshopProfileService,
    private router: Router,
    private route: ActivatedRoute,
    private alert: AlertService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.appointmentForm = this.fb.group({
      search : [''],
      bookingId: [null],
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


    // 🔥 READ ROUTE PARAM
    this.route.queryParams.subscribe(params => {
      if (params['id']) {
        this.bookingId = +params['id'];
        this.isEditMode = true;
        this.loadBooking(this.bookingId);
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
    console.log(this.auth.getRoleId());
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
      UserId: this.auth.getUserId(),          // as discussed
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

  loadBooking(bookingId: number) {
    this.service.getBookingById(bookingId).subscribe(res => {
      this.selectedCustomerId = res.customerId,
        this.selectedVehicleId = res.vehicleId,
        this.appointmentForm.patchValue({
        search: `${res.customerName} | +91 ${res.mobileNo} | ${res.regPrefix}${res.regNo}`,
        bookingId: res.Id,
        date: res.appointmentDate ? res.appointmentDate.split('T')[0] : null,    
        customerType: res.customerType,
        regPrefix: res.regPrefix,
        regNo: res.regNo,
        vehicleType: res.vehicleType,
        customerName: res.customerName,
        mobileNo: res.mobileNo,
        emailID: res.emailID,
        service: res.service,
        serviceAdvisor: res.serviceAdvisor,
        settings: res.settings,
        bay: res.bay
      });
      // Patch time separately after change detection
      setTimeout(() => {
        this.appointmentForm.patchValue({
          time: res.appointmentTime ? res.appointmentTime.substring(0, 5) : ''
        });
      }, 0);  // <-- just this line fixes NG0100
   
    });
    this.appointmentForm.get('search')?.disable();
    this.appointmentForm.get('regPrefix')?.disable(); 
    this.appointmentForm.get('vehicleType')?.disable();
    this.isEditMode = true;

  }

}
