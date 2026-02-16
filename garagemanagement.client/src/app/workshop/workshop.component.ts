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
  showStep6Errors: boolean = false;


  /* ---------- SERVICES ---------- */
  availableServices = [
    { id: 1, name: 'Car Wash' },
    { id: 2, name: 'Oil Change' },
    { id: 3, name: 'Engine Repair' }
  ];

  onServiceChange(id: number, event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const checked = checkbox.checked;

    const index = this.serviceIdsArray.value.indexOf(id.toString());

    if (checked && index === -1) {
      this.serviceIdsArray.push(this.fb.control(id.toString()));
    }

    if (!checked && index > -1) {
      this.serviceIdsArray.removeAt(index);
    }
  }

  isServiceChecked(id: number): boolean {
    return this.serviceIdsArray.value.includes(id.toString());
  }


  /* ---------- WORKING DAYS ---------- */

  toggleWorkingDay(day: number): void {
    const index = this.workingDaysArray.value.indexOf(day.toString());

    if (index > -1) {
      this.workingDaysArray.removeAt(index);
    } else {
      this.workingDaysArray.push(this.fb.control(day.toString()));
    }
  }

  isWorkingDayChecked(day: number): boolean {
    return this.workingDaysArray.value.includes(day.toString());
  }


  getDayName(day: number): string {
    return ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'][day];
  }

  /* ---------- PAYMENT MODES ---------- */
  availablePaymentModes = [
    { id: 1, name: 'Cash' },
    { id: 2, name: 'UPI' },
    { id: 3, name: 'Card' }
  ];

  onPaymentModeChange(id: number, event: Event): void {
    const checkbox = event.target as HTMLInputElement;
    const checked = checkbox.checked;

    const index = this.paymentModeIdsArray.value.indexOf(id.toString());

    if (checked && index === -1) {
      this.paymentModeIdsArray.push(this.fb.control(id.toString()));
    }

    if (!checked && index > -1) {
      this.paymentModeIdsArray.removeAt(index);
    }
  }

  isPaymentModeChecked(id: number): boolean {
    return this.paymentModeIdsArray.value.includes(id.toString());
  }


  /* ---------- FILE UPLOAD ---------- */
  selectedFiles: File[] = [];
  existingMedia: any[] = []; // from API

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;

    for (let i = 0; i < input.files.length; i++) {
      this.selectedFiles.push(input.files[i]);
    }

    // reset input so same file can be reselected
    input.value = '';
  }

  removeFile(index: number) {
    this.selectedFiles.splice(index, 1);
  }

  removeExistingFile(index: number) {
    this.existingMedia.splice(index, 1);
  }

  viewImage(url: string) {
    window.open(url, '_blank');
  }


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
      contactNo: ['', [Validators.required, this.indianMobileValidator]],
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

  // ✅ COMPLETE Step Validation (ALL fields)
  canGoNext(): boolean {
    switch (this.currentStep) {

      // STEP 1
      case 1: {
        const controls = [
          'workshopName',
          'ownerName',
          'ownerMobileNo',
          'emailID',
          'contactPerson',
          'contactNo',
          'landline'
        ];

        const valid = controls.every(c => this.workshopForm.get(c)?.valid);

        if (!valid) {
          this.markControlsTouched(controls);
        }

        return valid;
      }

      // STEP 2
      case 2: {
        const address = this.workshopForm.get('address') as FormGroup;

        if (!address.valid) {
          this.markGroupTouched('address');
          return false;
        }

        return true;
      }

      // STEP 3
      case 3: {
        const controls = [
          'inBusinessSince',
          'avgVehicleInflowPerMonth',
          'noOfEmployees',
          'dealerCode'
        ];

        const valid = controls.every(c => this.workshopForm.get(c)?.valid);

        if (!valid) {
          this.markControlsTouched(controls);
        }

        return valid;
      }

      // STEP 4
      case 4: {
        const timing = this.workshopForm.get('timing') as FormGroup;

        if (!timing.valid) {
          this.markGroupTouched('timing');
          return false;
        }

        return true;
      }

      // STEP 5
      case 5: {
        const businessConfig = this.workshopForm.get('businessConfig') as FormGroup;

        if (!businessConfig.valid) {
          this.markGroupTouched('businessConfig');
          return false;
        }

        return true;
      }

      // STEP 6 (Arrays + files)
      case 6: {
        const hasFiles =
          this.selectedFiles.length > 0 ||
          this.existingMedia.length > 0;

        const valid =
          this.serviceIdsArray.length > 0 &&
          this.workingDaysArray.length >= 5 &&
          this.paymentModeIdsArray.length > 0 &&
          hasFiles;

        if (!valid) {
          this.showStep6Errors = true;
        }

        return valid;
      }


      // STEP 7
      case 7: {
        const ctrl = this.workshopForm.get('isGdprAccepted');

        if (!ctrl?.valid) {
          ctrl?.markAsTouched();
          ctrl?.markAsDirty();
          return false;
        }

        return true;
      }

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
    else {
      this.alert.showError("Please fill all required fields!");
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

    const formData = new FormData();
    const formValue = this.workshopForm.value;

    // 🔹 Basic fields
    formData.append('Id', formValue.id);
    formData.append('WorkshopName', formValue.workshopName);
    formData.append('OwnerName', formValue.ownerName);
    formData.append('OwnerMobileNo', formValue.ownerMobileNo);
    formData.append('EmailID', formValue.emailID);
    formData.append('ContactPerson', formValue.contactPerson);
    formData.append('ContactNo', formValue.contactNo);
    formData.append('Landline', formValue.landline);
    formData.append('InBusinessSince', formValue.inBusinessSince);
    formData.append('AvgVehicleInflowPerMonth', formValue.avgVehicleInflowPerMonth);
    formData.append('DealerCode', formValue.dealerCode);
    formData.append('NoOfEmployees', formValue.noOfEmployees);
    formData.append('IsGdprAccepted', formValue.isGdprAccepted.toString());

    // 🔹 Address
    Object.keys(formValue.address).forEach(key => {
      formData.append(`Address.${key}`, formValue.address[key]);
    });

    // 🔹 Timing
    formData.append('Timing.StartTime', formValue.timing.startTime);
    formData.append('Timing.EndTime', formValue.timing.endTime);

    // 🔹 Business Config
    Object.keys(formValue.businessConfig).forEach(key => {
      formData.append(`BusinessConfig.${key}`, formValue.businessConfig[key]);
    });

    // 🔹 Arrays
    formValue.serviceIds.forEach((id: string) =>
      formData.append('ServiceIds', id)
    );

    formValue.workingDays.forEach((day: string) =>
      formData.append('WorkingDays', day)
    );

    formValue.paymentModeIds.forEach((id: string) =>
      formData.append('PaymentModeIds', id)
    );

    // 🔹 Files
    if (this.selectedFiles && this.selectedFiles.length > 0) {
      for (let i = 0; i < this.selectedFiles.length; i++) {
        formData.append('MediaFiles', this.selectedFiles[i]);
      }
    }

    this.http.post(this.apiUrl, formData).subscribe({
      next: (res: any) => {
        this.alert.showInfo(res.message || 'Workshop created successfully with media!');
        this.resetForm();
        this.isSubmitting = false;
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

        /* =========================
           STEP 1–5 : MAIN FORM
        ========================= */
        this.workshopForm.patchValue({
          id: res.id,

          workshopName: res.workshopName,
          ownerName: res.ownerName,
          ownerMobileNo: res.ownerMobileNo,
          emailID: res.emailID,
          contactPerson: res.contactPerson,
          contactNo: res.contactNo,
          landline: res.landline,

          inBusinessSince: res.inBusinessSince?.substring(0, 7),
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

        /* =========================
           STEP 6 : FORM ARRAYS
        ========================= */

        // SERVICES
        this.setFormArray(
          this.serviceIdsArray,
          res.serviceIds?.map((x: any) => x.toString()) || []
        );

        // WORKING DAYS
        this.setFormArray(
          this.workingDaysArray,
          res.workingDays?.map((x: any) => x.toString()) || []
        );

        // PAYMENT MODES
        this.setFormArray(
          this.paymentModeIdsArray,
          res.paymentModeIds?.map((x: any) => x.toString()) || []
        );

        /* =========================
           STEP 7 : MEDIA (SEPARATE)
        ========================= */

        // ✅ store existing uploaded images
        this.existingMedia = res.mediaFiles || [];

        // ❌ DO NOT touch selectedFiles here
        this.selectedFiles = [];

        /* =========================
           STEP CONTROL
        ========================= */
        this.currentStep = 1; // edit always starts from step 1
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

  private markControlsTouched(controlNames: string[]) {
    controlNames.forEach(name => {
      const ctrl = this.workshopForm.get(name);
      ctrl?.markAsTouched();
      ctrl?.markAsDirty();
    });
  }

  private markGroupTouched(groupName: string) {
    const group = this.workshopForm.get(groupName) as FormGroup;
    if (!group) return;

    Object.values(group.controls).forEach(control => {
      control.markAsTouched();
      control.markAsDirty();
    });
  }

}
