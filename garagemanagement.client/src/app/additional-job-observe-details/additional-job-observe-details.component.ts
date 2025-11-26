import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ROLES } from '../constants/roles.constants';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-additional-job-observe-details',
  standalone: false,
  templateUrl: './additional-job-observe-details.component.html',
  styleUrls: ['./additional-job-observe-details.component.css']
})
export class AdditionalJobObserveDetailsComponent implements OnInit {
  ROLES = ROLES;
  jobForm!: FormGroup;

  constructor(private fb: FormBuilder, private alert: AlertService) { }

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
    if (this.rows.length > 0 && this.rows.at(this.rows.length - 1).invalid) {
      this.jobForm.markAllAsTouched();
      this.alert.showError('Please fill all fields correctly before adding a new row.');
      return;
    }
    const nextSlNo = this.rows.length + 1;
    this.rows.push(this.createRow(nextSlNo));
  }

  removeRow(index: number): void {
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      this.rows.removeAt(index);
      this.recalculateSlNo();
      this.recalculateSerialNumbers();
    });
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
      this.alert.showSuccess('Form submitted successfully!');
      console.log('✅ Submitted data:', this.jobForm.value);
    } else {
      this.jobForm.markAllAsTouched();
      this.alert.showError('Please correct the errors before submitting.');
    }
  }
}
