import { Component, OnInit, AfterViewInit, ViewChildren, QueryList, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormArray, AbstractControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AlertService } from '../services/alert.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-workshop',
  templateUrl: './workshop.component.html',
  standalone: false,
  styleUrls: ['./workshop.component.css']
})
export class WorkshopComponent implements OnInit, AfterViewInit {
  workshopId: number | null = null;
  workshopForm!: FormGroup;
  isSubmitting = false;
  progressPercentage = 0;
  currentStep = 1;
  availableServices = [
    { id: '1', name: 'Oil Change' },
    { id: '2', name: 'Wheel Alignment' },
    { id: '3', name: 'Brake Service' },
    { id: '4', name: 'AC Repair' }
  ];
  availablePaymentModes = [
    { id: '1', name: 'Cash' },
    { id: '2', name: 'UPI' },
    { id: '3', name: 'Card' }
  ];

  @ViewChildren('form-step', { read: ElementRef }) stepElements!: QueryList<ElementRef>;

  private apiUrl = 'https://localhost:7086/api/WorkshopProfile/createworkshop';
  private getByIdUrl = 'https://localhost:7086/api/WorkshopProfile/getWorkshopById';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private alert: AlertService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.initializeForm();

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.workshopId = +id;
        this.loadWorkshop(this.workshopId); // 🔥 AUTO FILL
      }
    });
  }

  ngAfterViewInit() {
    setTimeout(() => this.showCurrentStep(), 100);
  }

  private initializeForm(): void {
    this.workshopForm = this.fb.group({
      // ✅ STEP 1: Basic Information
      id:[0],
      workshopName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      ownerName: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      ownerMobileNo: ['', [Validators.required, this.indianMobileValidator]],
      emailID: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
      contactPerson: ['', [Validators.required, Validators.maxLength(100)]],
      landline: ['', [Validators.required, Validators.pattern(/^[0-9+\-\s()]{10,15}$/)]],

      // ✅ STEP 2: Address
      address: this.fb.group({
        flatNo: ['', [Validators.required, Validators.maxLength(20)]],
        street: ['', [Validators.required, Validators.maxLength(100)]],
        location: ['', [Validators.required, Validators.maxLength(100)]],
        city: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
        state: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]],
        stateCode: ['', [Validators.required, Validators.pattern(/^[A-Z]{2}$/)]],
        country: ['India', [Validators.required]],
        pincode: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
        landmark: ['', [Validators.required, Validators.maxLength(100)]],
        branchAddress: ['', [Validators.required, Validators.maxLength(500)]]
      }),

      // ✅ STEP 3: Business Details
      inBusinessSince: ['', [Validators.required, Validators.pattern(/^\d{4}-\d{2}$/)]],
      avgVehicleInflowPerMonth: [0, [Validators.required, Validators.min(0), Validators.max(10000)]],
      noOfEmployees: [0, [Validators.required, Validators.min(0), Validators.max(1000)]],
      dealerCode: ['', [Validators.required, Validators.maxLength(20)]],

      // ✅ STEP 4: Timing
      timing: this.fb.group({
        startTime: ['', [Validators.required]],
        endTime: ['', [Validators.required]]
      }),

      // ✅ STEP 5: Business Config
      businessConfig: this.fb.group({
        websiteLink: ['', [Validators.required, Validators.pattern(/(https?:\/\/[^\s$.?#].[^\s]*)/i)]],
        googleReviewLink: ['', [Validators.required, Validators.pattern(/(https?:\/\/[^\s$.?#].[^\s]*)/i)]],
        externalIntegrationUrl: ['', [Validators.required, Validators.pattern(/(https?:\/\/[^\s$.?#].[^\s]*)/i)]],
        gstin: ['', [this.gstinValidator]],
        msme: ['', [Validators.required, Validators.maxLength(50)]],
        sac: ['', [Validators.required, Validators.pattern(/^\d{6}$/)]],
        sacPercentage: ['', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]]
      }),

      // ✅ STEP 6: ALL 4 BACKEND FIELDS
      serviceIds: this.fb.array([]),           // FormArray for Services
      workingDays: this.fb.array([]),          // FormArray for Days (0=Sun,1=Mon...)
      paymentModeIds: this.fb.array([]),       // FormArray for Payments
      media: this.fb.array([]),                // FormArray for Media paths

      // ✅ STEP 7: GDPR
      isGdprAccepted: [false, Validators.requiredTrue]
    });

   // this.workshopForm.valueChanges.subscribe(() => this.updateProgress());
  }

  // ✅ FormArray Getters
  get serviceIdsArray(): FormArray { return this.workshopForm.get('serviceIds') as FormArray; }
  get workingDaysArray(): FormArray { return this.workshopForm.get('workingDays') as FormArray; }
  get paymentModeIdsArray(): FormArray { return this.workshopForm.get('paymentModeIds') as FormArray; }
  get mediaArray(): FormArray { return this.workshopForm.get('media') as FormArray; }

  // ✅ Add Service to FormArray
  addService(serviceId: string) {
    if (!this.serviceIdsArray.value.includes(serviceId)) {
      this.serviceIdsArray.push(this.fb.control(serviceId));
    }
  }

  // ✅ Toggle Working Day
  toggleWorkingDay(dayNum: number) {
    const index = this.workingDaysArray.value.indexOf(dayNum.toString());
    if (index > -1) {
      this.workingDaysArray.removeAt(index);
    } else {
      this.workingDaysArray.push(this.fb.control(dayNum.toString()));
    }
  }

  // ✅ Add Payment Mode
  addPaymentMode(paymentId: string) {
    if (!this.paymentModeIdsArray.value.includes(paymentId)) {
      this.paymentModeIdsArray.push(this.fb.control(paymentId));
    }
  }

  // ✅ Add Media Path (for demo - replace with file upload)
  addMediaPath(path: string) {
    this.mediaArray.push(this.fb.control(path));
  }

  getDayName(dayNum: number): string {
    const days = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    return days[dayNum] || '';
  }

  // ✅ COMPLETE Step Validation (ALL fields)
  canGoNext(): boolean {
    switch (this.currentStep) {
      case 1: // Basic Info - ALL 6 fields
        return !!(this.workshopForm.get('workshopName')?.valid &&
          this.workshopForm.get('ownerName')?.valid &&
          this.workshopForm.get('ownerMobileNo')?.valid &&
          this.workshopForm.get('emailID')?.valid &&
          this.workshopForm.get('contactPerson')?.valid &&
          this.workshopForm.get('landline')?.valid);

      case 2: // Address - ALL 10 fields
        const address = this.workshopForm.get('address') as FormGroup;
        return !!(address?.valid);

      case 3: // Business Details - ALL 4 fields
        return !!(this.workshopForm.get('inBusinessSince')?.valid &&
          this.workshopForm.get('avgVehicleInflowPerMonth')?.valid &&
          this.workshopForm.get('noOfEmployees')?.valid &&
          this.workshopForm.get('dealerCode')?.valid);

      case 4: // Timing - ALL 2 fields
        const timing = this.workshopForm.get('timing') as FormGroup;
        return !!timing?.valid;

      case 5: // Business Config - ALL 7 fields
        const businessConfig = this.workshopForm.get('businessConfig') as FormGroup;
        return !!businessConfig?.valid;

      case 6: // ALL 4 Backend Fields - Minimum selections required
        return !!(this.serviceIdsArray.length > 0 &&
          this.workingDaysArray.length >= 5 &&
          this.paymentModeIdsArray.length > 0 &&
          this.mediaArray.length > 0);

      case 7: // GDPR
        return !!this.workshopForm.get('isGdprAccepted')?.valid;

      default:
        return true;
    }
  }

  nextStep(): void {
    if (this.canGoNext()) {
      this.hideAllSteps();
      this.currentStep = Math.min(this.currentStep + 1, 7);
      this.showCurrentStep();
      this.updateProgress();
    }
  }

  prevStep(): void {
    if (this.currentStep > 1) {
      this.hideAllSteps();
      this.currentStep--;
      this.showCurrentStep();
      this.updateProgress();
    }
  }

  private showCurrentStep(): void {
    this.stepElements?.forEach((step, index) => {
      step.nativeElement.classList.toggle('show', index + 1 === this.currentStep);
    });
  }

  private hideAllSteps(): void {
    this.stepElements?.forEach(step => step.nativeElement.classList.remove('show'));
  }
  totalSteps = 7;
  updateProgress(): void {
    this.progressPercentage = Math.min(
      Math.round((this.currentStep / this.totalSteps) * 100),
      100
    );
  }


  private getAllFormControls(formGroup: FormGroup): AbstractControl[] {
    let controls: AbstractControl[] = [];
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      if (control instanceof FormGroup || control instanceof FormArray) {
        controls = controls.concat(this.getAllFormControls(control as FormGroup));
      } else if (control) {
        controls.push(control);
      }
    });
    return controls;
  }

  // Custom Validators
  indianMobileValidator(control: AbstractControl): { [key: string]: any } | null {
    if (!control.value) return null;
    return /^[6-9]\d{9}$/.test(control.value) ? null : { invalidMobile: true };
  }

  gstinValidator(control: AbstractControl): { [key: string]: any } | null {
    if (!control.value) return null;
    return /^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$/.test(control.value) ? null : { invalidGSTIN: true };
  }

  submit(): void {
    if (this.workshopForm.invalid) {
      this.markFormGroupTouched(this.workshopForm);
      this.alert.showError('Please complete all required fields!');
      return;
    }

    this.isSubmitting = true;
    const formData = this.workshopForm.value; // ✅ EXACTLY matches your C# DTO

    this.http.post(this.apiUrl, formData).subscribe({
      next: (response) => {
        this.alert.showInfo('Workshop created successfully with all configurations!');
        this.resetForm();
      },
      error: (error) => {
        this.alert.showError(error.error?.message || 'Creation failed!');
        this.isSubmitting = false;
      }
    });
  }

  private markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();
      if (control instanceof FormGroup || control instanceof FormArray) {
        this.markFormGroupTouched(control as FormGroup);
      }
    });
  }

  private resetForm(): void {
    this.workshopForm.reset({
      address: { country: 'India' },
      avgVehicleInflowPerMonth: 0,
      noOfEmployees: 0,
      isGdprAccepted: false
    });
    this.progressPercentage = 0;
    this.currentStep = 1;
    this.showCurrentStep();
    this.isSubmitting = false;
  }

  // Nested form getters
  get addressForm(): FormGroup { return this.workshopForm.get('address') as FormGroup; }
  get timingForm(): FormGroup { return this.workshopForm.get('timing') as FormGroup; }
  get businessConfigForm(): FormGroup { return this.workshopForm.get('businessConfig') as FormGroup; }
  // Add INSIDE WorkshopComponent class (before ngOnInit)

  loadWorkshop(id: number): void {
    this.http.get<any>(`${this.getByIdUrl}/${id}`).subscribe({
      next: (res) => {

        // ✅ PATCH MAIN FORM (STEP 1–5)
        this.workshopForm.patchValue({
          id: res.id,

          workshopName: res.workshopName,
          ownerName: res.ownerName,
          ownerMobileNo: res.ownerMobileNo,
          emailID: res.emailID,
          contactPerson: res.contactPerson,
          landline: res.landline,

          inBusinessSince: res.inBusinessSince?.substring(0, 7), // YYYY-MM
          avgVehicleInflowPerMonth: res.avgVehicleInflowPerMonth,
          noOfEmployees: res.noOfEmployees,
          dealerCode: res.dealerCode,

          isGdprAccepted: res.isGdprAccepted,

          address: res.address,
          businessConfig: res.businessConfig,

          timing: {
            startTime: this.formatTime(res.timing?.startTime),
            endTime: this.formatTime(res.timing?.endTime)
          }
        });

        alert(JSON.stringify(res.serviceIds))

        // ✅ PATCH FORM ARRAYS (STEP 6)
        // ✅ SERVICES (M:M)
        this.setFormArray(
          this.serviceIdsArray,
          res.serviceIds?.map((x: any) => x.toString())
        );

        // ✅ WORKING DAYS (1:M)
        this.setFormArray(
          this.workingDaysArray,
          res.workingDays?.map((x: any) => x.toString())
        );

        // ✅ PAYMENT MODES (M:M)
        this.setFormArray(
          this.paymentModeIdsArray,
          res.paymentModeIds?.map((x: any) => x.toString())
        );

        // ✅ MEDIA (1:M)
        this.setFormArray(
          this.mediaArray,
          res.media?.map((x: any) => x.toString())
        );


        // ✅ Move user to last completed step
        this.currentStep = 7;
        this.updateProgress();
        this.showCurrentStep();
      },
      error: () => {
        this.alert.showError('Failed to load workshop data');
      }
    });
  }


  private setFormArray(formArray: FormArray, values: any[]): void {
    formArray.clear();
    if (values && values.length) {
      values.forEach(v => formArray.push(this.fb.control(v)));
    }
  }
  private formatTime(time: string | null): string {
    if (!time) return '';
    return time.substring(0, 5); // "10:30:00" → "10:30"
  }

}
