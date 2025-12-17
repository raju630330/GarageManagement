import { Component, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
  AbstractControl,
  ValidatorFn
} from '@angular/forms';
import { Router } from '@angular/router';
import { JobCardService } from '../services/job-card.service';
import { MatDialog } from '@angular/material/dialog';
import { GlobalPopupComponent } from '../global-popup/global-popup.component';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-new-jobcard',
  templateUrl: './new-jobcard.component.html',
  styleUrls: ['./new-jobcard.component.css'],
  standalone: false
})
export class NewJobCardComponent implements OnInit {

  jobCardForm!: FormGroup;
  newConcern!: FormControl;
  today: string = '';
  constructor(private fb: FormBuilder, private router: Router, public jobcardService: JobCardService, private dialog: MatDialog, private alert: AlertService) { }

  ngOnInit(): void {
    const now = new Date();
    this.today = now.toISOString().split('T')[0];

    this.newConcern = this.fb.control('', Validators.required);

    this.jobCardForm = this.fb.group({
      id:[0],
      vehicleData: this.fb.group({
        registrationNo: ['', Validators.required],
        odometerIn: ['', [Validators.required, Validators.min(0.1)]],
        avgKmsPerDay: ['', Validators.required],
        vin: ['', Validators.required],
        engineNo: ['', Validators.required],
        vehicleColor: ['', Validators.required],
        fuelType: ['', Validators.required],
        serviceType: ['', Validators.required],
        serviceAdvisor: ['', Validators.required],
        technician: ['', Validators.required],
        vendor: ['', Validators.required]
      }),
      concerns: this.fb.array([], this.minLengthArray(1)),
      customerInfo: this.fb.group({
        corporate: ['', Validators.required],
        customerName: ['', Validators.required],
        mobile: ['', [Validators.required, Validators.pattern(/^[6-9]\d{9}$/)]],
        alternateMobile: ['', Validators.pattern(/^[6-9]\d{9}$/)],
        email: ['', [Validators.required, Validators.pattern(
          '^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$'
        )]],
        deliveryDate: ['', Validators.required],
        insuranceCompany: ['', Validators.required]
      }),
      advancePayment: this.fb.group({
        cash: ['', [Validators.required, Validators.min(0.01)]],
        bankName: ['', Validators.required],
        chequeNo: ['', Validators.required],
        amount: ['', [Validators.required, Validators.min(0.01)]],
        date: ['', Validators.required]
      })
    });
  }

  // Custom validator to ensure FormArray has min length
  private minLengthArray(min: number): ValidatorFn {
    return (control: AbstractControl | null) => {
      if (!control || !(control instanceof FormArray)) return null;
      return control.length >= min ? null : { minLengthArray: { requiredLength: min, actualLength: control.length } };
    };
  }

  // ------- convenience getters ----------

  // in html we need this otherwise we need to get formgroup then code will increase
  // <div * ngIf="vehicleData.get('registrationNo')?.touched && vehicleData.get('registrationNo')?.invalid"
  get vehicleData(): FormGroup {
    return this.jobCardForm.get('vehicleData') as FormGroup;
  }
  get concernsArray(): FormArray {
    return this.jobCardForm.get('concerns') as FormArray;
  }
  get customerInfo(): FormGroup {
    return this.jobCardForm.get('customerInfo') as FormGroup;
  }
  get advancePayment(): FormGroup {
    return this.jobCardForm.get('advancePayment') as FormGroup;
  }

  // ----------------- CONCERNS ARRAY -------------------

  addConcern() {
    const text = (this.newConcern.value || '').toString().trim();
    if (!text) {
      this.newConcern.markAsTouched();
      return;
    }

    this.concernsArray.push(
      this.fb.group({
        text: [text, Validators.required],
        active: [true]
      })
    );

    // After Concern is added then validation need to reset means
    //form should not validate cocern text box so below we rest the control
    this.newConcern.reset();

    // mark concerns as touched so validation hides after push
    this.concernsArray.markAsTouched();
  }

  toggleConcern(index: number) {
    const c = this.concernsArray.at(index);
    c.patchValue({ active: !c.value['active'] });
  }

  removeConcern(index: number) {
    this.alert.confirm('Are you want to remove this concern ?', () => {
      this.concernsArray.removeAt(index);
    })   
  }

  // ----------------- SUBMIT -------------------
  expandAll = false;
  onPrepareEstimate() {

    // if invalid, stop — submit button normally disabled but guard anyway
    if (this.jobCardForm.invalid) {
      // mark all controls as touched so validation messages appear
      this.markAllTouched(this.jobCardForm);
      this.expandAll = true;
      this.alert.showError('Please fix validation errors before submitting!');
      return;
    }

    const dto = this.jobCardForm.value;

    this.jobcardService.saveJobCard(dto).subscribe({
      next: (res) => {
        this.router.navigate(['/estimate'], {
          queryParams: { id: res.id }
        });
      },
      error: (err) => {
        console.error(err);

        // Check if backend returned validation errors
        if (err.status === 400 && err.error?.errors) {
          // err.error.errors should be a dictionary { fieldName: errorMessage }
          Object.keys(err.error.errors).forEach((field) => {
            const control = this.jobCardForm.get(field);
            if (control) {
              control.setErrors({ backend: err.error.errors[field] });
            }
          });
        } else {
          // Generic error alert
          alert(err.error?.message || 'Something went wrong!');
        }
      }
    });

  }

  // utility to mark controls touched
  private markAllTouched(control: AbstractControl) {
    if (control.hasOwnProperty('controls')) {
      // it's a FormGroup or FormArray
      const ctrl = control as any;
      for (const inner in ctrl.controls) {
        if (ctrl.controls.hasOwnProperty(inner)) {
          this.markAllTouched(ctrl.controls[inner]);
        }
      }
    }
    control.markAsTouched();
  }

// This block is for opening popup for buttons below the search 
  openPopup(type: string) {
    const data = {
      title: type,
      fields: {
        'Registration No': 'ABCD123456',
        'VIN': '1HGCM82633A123456',
        'Engine No': 'ENG123456',
        'Odometer': 12000,
        'Registratidon No': 'ABCD123456',
        'VIdN': '1HGCM82633A123456',
        'Engdine No': 'ENG123456',
        'Odomedter': 12000,
        'RegWistration No': 'ABCD123456',
        'VFIN': '1HGCM82633A123456',
        'EnRgine No': 'ENG123456',
        'OdBometer': 12000
      }
    };

    this.dialog.open(GlobalPopupComponent, {
      data,
      width: '800px', 
      maxWidth: '95vw',
      autoFocus: false,
      disableClose: true,
    });
  }

  // To fetch data after select option in autocomplete
  onSelectedRegistration(item: any) {
    if (!item || !item.id) { // validate
      return;
    }
    this.jobcardService.getJobCardDetails(item.id).subscribe(res => {
      this.jobCardForm.get('id')!.patchValue(res.id);
      this.expandAll = true;
      // Patch vehicleData directly
      this.jobCardForm.get('vehicleData')!.patchValue(res.vehicleData);

      // Patch customerInfo with formatted deliveryDate
      this.jobCardForm.get('customerInfo')!.patchValue({
        ...res.customerInfo,
        deliveryDate: res.customerInfo?.deliveryDate?.split('T')[0] // yyyy-MM-dd
      });

      // Patch concerns
      const concernsArray = this.jobCardForm.get('concerns') as FormArray;
      concernsArray.clear();
      res.concerns?.forEach((c: any) => concernsArray.push(this.fb.group({ text: c.text, active: c.active })));

      // Patch advancePayment
      this.jobCardForm.get('advancePayment')!.patchValue({
        ...res.advancePayment,
        date: res.advancePayment?.date?.split('T')[0]
      });
    });
  }
}
