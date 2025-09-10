import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-garage-management',
  standalone: false,
  templateUrl: './garage-management.component.html',
  styleUrl: './garage-management.component.css'
})
export class GarageManagementComponent {
  garageForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.garageForm = this.fb.group({
      rows: this.fb.array([]),
    });

    // Create 4 rows by default
    for (let i = 0; i < 4; i++) {
      this.addRow();
    }
  }

  get rows() {
    return this.garageForm.get('rows') as FormArray;
  }

  addRow() {
    const row = this.fb.group({
      slNo: ['', [Validators.required, Validators.pattern(/^[0-9]+$/)]],
      driverVoice: ['', [Validators.pattern(/^[a-zA-Z ]*$/), Validators.maxLength(50)]],
      supInstructions: ['', [Validators.pattern(/^[a-zA-Z ]*$/), Validators.maxLength(50)]],
      actionTaken: ['', [Validators.pattern(/^[a-zA-Z ]*$/), Validators.maxLength(50)]],
      startTime: ['', [Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9] (AM|PM)$/)]],
      endTime: ['', [Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9] (AM|PM)$/)]],
    });
    this.rows.push(row);
  }
}
