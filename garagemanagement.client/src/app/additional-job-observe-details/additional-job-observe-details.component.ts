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

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.jobForm = this.fb.group({
      rows: this.fb.array([this.createRow(1)]) // first row gets slNo = 1
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
    this.rows.removeAt(index);

    // Recalculate serial numbers after removing a row
    this.rows.controls.forEach((control, i) => {
      control.get('slNo')?.setValue(i + 1);
    });
  }

  submit(): void {
    if (this.jobForm.valid) {
      console.log('✅ Submitted data:', this.jobForm.value);
      alert('Form submitted successfully!');
    } else {
      this.jobForm.markAllAsTouched();
      alert('❌ Please fix all validation errors before submitting.');
    }
  }
}
