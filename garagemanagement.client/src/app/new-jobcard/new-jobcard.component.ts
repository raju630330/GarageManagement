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

@Component({
  selector: 'app-new-jobcard',
  templateUrl: './new-jobcard.component.html',
  styleUrls: ['./new-jobcard.component.css'],
  standalone: false
})
export class NewJobCardComponent implements OnInit {

  jobCardForm!: FormGroup;
  newConcern!: FormControl;

  // optional flag used to show global errors after first submit attempt
  submitted = false;

  constructor(private fb: FormBuilder, private router: Router, private jobcardService: JobCardService) { }

  ngOnInit(): void {
    this.newConcern = this.fb.control('', Validators.required);
    this.jobCardForm = this.fb.group({
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
        email: ['', [Validators.required, Validators.email]],
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

    this.newConcern.reset();
    // mark concerns as touched so validation hides after push
    this.concernsArray.markAsTouched();
  }

  toggleConcern(index: number) {
    const c = this.concernsArray.at(index);
    c.patchValue({ active: !c.value['active'] });
  }

  removeConcern(index: number) {
    this.concernsArray.removeAt(index);
  }

  // ----------------- SUBMIT -------------------
  onPrepareEstimate() {
    this.submitted = true;

    // if invalid, stop — submit button normally disabled but guard anyway
    if (this.jobCardForm.invalid) {
      // mark all controls as touched so validation messages appear
      this.markAllTouched(this.jobCardForm);
      return;
    }

    const payload = this.jobCardForm.value;
    if (this.jobCardForm.invalid) return;

    const dto = this.jobCardForm.value;

    this.jobcardService.saveJobCard(dto).subscribe({
      next: (res) => {
        console.log("Saved successfully", res);

        this.router.navigate(['/estimate'], {
          queryParams: { registrationNo: res.registrationNo }
        });
      },
      error: (err) => console.error(err)
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

  openPopup(type: string) {
    alert(`Open details for: ${type}`);
  }
}
