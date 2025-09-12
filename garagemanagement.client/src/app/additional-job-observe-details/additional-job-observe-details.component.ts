import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-additional-job-observe-details',
  standalone: false,
  templateUrl: './additional-job-observe-details.component.html',
  styleUrl: './additional-job-observe-details.component.css'
})
export class AdditionalJobObserveDetailsComponent {
  jobForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.jobForm = this.fb.group({
      rows: this.fb.array([])
    });
    this.addRow(); // add first row by default
  }

  get rows() {
    return this.jobForm.get('rows') as FormArray;
  }

  addRow() {
    const nextSlNo = this.rows.length + 1; // Auto increment based on row count
    const row = this.fb.group({
      slNo: [{ value: nextSlNo, disabled: true }], // Disabled input, still displayed
      technicianVoice: ['', [Validators.required, Validators.pattern(/^[a-zA-Z ]{1,50}$/)]],
      supervisorInstructions: ['', [Validators.required, Validators.pattern(/^[a-zA-Z ]{1,50}$/)]],
      actionTaken: ['', [Validators.required, Validators.pattern(/^[a-zA-Z ]{1,50}$/)]],
      startTime: ['', [Validators.required, Validators.pattern(/^(0?[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]],
      endTime: ['', [Validators.required, Validators.pattern(/^(0?[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]],
    });
    this.rows.push(row);
  }

  removeRow(index: number) {
    this.rows.removeAt(index);
    this.reindexRows();
  }

  private reindexRows() {
    // Reset Sl. No. after removing a row
    this.rows.controls.forEach((ctrl, i) => {
      ctrl.get('slNo')?.setValue(i + 1);
    });
  }

  submit() {
    if (this.jobForm.valid) {
      // Since slNo is disabled, use getRawValue() to include it in result
      console.log(this.jobForm.getRawValue());
      alert('Form submitted successfully!');
    } else {
      alert('Please correct the errors.');
    }
  }
}
