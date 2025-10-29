import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-additional-job-observe-details',
  standalone: false,
  templateUrl: './additional-job-observe-details.component.html',
  styleUrls: ['./additional-job-observe-details.component.css']
})
export class AdditionalJobObserveDetailsComponent implements OnInit {
  jobForm!: FormGroup;

  showAlert = false;
  alertMessage = '';
  confirmAction: (() => void) | null = null;

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

  removeRow(index: number): void {
    this.alertMessage = 'Are you sure you want to remove this row?';
    this.showAlert = true;

    this.confirmAction = () => {
      this.rows.removeAt(index);
      this.recalculateSlNo();
      this.recalculateSerialNumbers();
      this.closeAlert();
    };
  }

  confirmYes(): void {
    if (this.confirmAction) this.confirmAction();
  }

  closeAlert(): void {
    this.showAlert = false;
    this.confirmAction = null;
  }

  private recalculateSlNo(): void {
  }

  recalculateSerialNumbers(): void {
    this.rows.controls.forEach((control, i) => {
      control.get('slNo')?.setValue(i + 1);
    });
  }

  submit(): void {
    if (this.jobForm.valid) {
      this.alertMessage = 'Form submitted successfully!';
      this.showAlert = true;
      this.confirmAction = null;
      console.log('✅ Submitted data:', this.jobForm.value);
    } else {
      this.jobForm.markAllAsTouched();
      this.alertMessage = 'Please correct the errors before submitting.';
      this.showAlert = true;
      this.confirmAction = null;
    }
  }
}
