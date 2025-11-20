import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-garage-management',
  standalone: false,
  templateUrl: './garage-management.component.html',
  styleUrl: './garage-management.component.css'
})
export class GarageManagementComponent implements OnInit {

  jobForm!: FormGroup;

  // Custom alert
  showAlert = false;
  alertMessage = '';
  confirmAction: (() => void) | null = null;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.jobForm = this.fb.group({
      rows: this.fb.array([this.createRow()])
    });
  }

  get rows(): FormArray {
    return this.jobForm.get('rows') as FormArray;
  }

  createRow(): FormGroup {
    return this.fb.group({
      driverVoice: ['', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      supervisorInstructions: ['', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      actionTaken: ['', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      startTime: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]],
      endTime: ['', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]]
    });
  }

  addRow(): void {
    if (this.rows.length > 0 && this.rows.at(this.rows.length - 1).invalid) {
      this.jobForm.markAllAsTouched();
      this.alertMessage = 'Please fill all fields correctly before adding a new row.';
      this.showAlert = true;
      this.confirmAction = null;
      return;
    }
    this.alertMessage = '';
    this.rows.push(this.createRow());
  }

  removeRow(index: number): void {
    this.alertMessage = 'Are you sure you want to remove this row?';
    this.showAlert = true;
    this.confirmAction = () => {
      this.rows.removeAt(index);
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

  submit(): void {
    if (this.jobForm.valid) {
      this.alertMessage = 'Form submitted successfully!';
      this.showAlert = true;
      this.confirmAction = null;
      console.log('✅ Submitted:', this.jobForm.value);
    } else {
      this.jobForm.markAllAsTouched();
      this.alertMessage = 'Please correct the errors before submitting.';
      this.showAlert = true;
      this.confirmAction = null;
    }
  }
}
