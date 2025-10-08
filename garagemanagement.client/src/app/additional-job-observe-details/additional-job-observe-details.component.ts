import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-additional-job-observe-details',
  standalone: false,
  templateUrl: './additional-job-observe-details.component.html',
  styleUrl: './additional-job-observe-details.component.css'
})
export class AdditionalJobObserveDetailsComponent {
  jobForm!: FormGroup;

  // Custom alert state
  showAlert = false;
  alertMessage = '';
  confirmAction: (() => void) | null = null; // callback for Yes button

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.jobForm = this.fb.group({
      rows: this.fb.array([this.createRow(1)])
    });
  }

  get rows(): FormArray {
    return this.jobForm.get('rows') as FormArray;
  }

  createRow(slNo: number): FormGroup {
    return this.fb.group({
      slNo: [slNo],
      technicianVoice: ['', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      supervisorInstructions: ['', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      actionTaken: ['', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      startTime: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]],
      endTime: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]]
    });
  }

  addRow(): void {
    const nextSlNo = this.rows.length + 1;
    this.rows.push(this.createRow(nextSlNo));
  }

  // Use custom alert instead of browser confirm
  removeRow(index: number): void {
    this.alertMessage = 'Are you sure you want to remove this row?';
    this.showAlert = true;
    this.confirmAction = () => {
      this.rows.removeAt(index);
      this.closeAlert();
    };
  }

  // Triggered when user clicks Yes in alert
  confirmYes(): void {
    if (this.confirmAction) this.confirmAction();
  }

  // Close alert (No button)
  closeAlert(): void {
    this.showAlert = false;
    this.confirmAction = null;
  }

  submit(): void {
    if (this.jobForm.valid) {
      this.alertMessage = 'Form submitted successfully!';
      this.showAlert = true;
      this.confirmAction = null; // no action needed for submit alert
      console.log('✅ Submitted data:', this.jobForm.value);
    } else {
      this.jobForm.markAllAsTouched();
      this.alertMessage = 'Please fix all validation errors before submitting.';
      this.showAlert = true;
      this.confirmAction = null;
    }
  }
}
