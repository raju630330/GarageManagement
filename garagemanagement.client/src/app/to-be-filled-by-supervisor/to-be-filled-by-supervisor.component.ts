import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ROLES } from '../constants/roles.constants';
import { AlertService } from '../services/alert.service';
import { ToBeFilledBySupervisorService } from '../services/tobefilledbysupervisor.service';

@Component({
  selector: 'app-to-be-filled-by-supervisor',
  standalone: false,
  templateUrl: './to-be-filled-by-supervisor.component.html',
  styleUrl: './to-be-filled-by-supervisor.component.css'
})
export class ToBeFilledBySupervisorComponent implements OnInit {
  ROLES = ROLES;
  jobForm!: FormGroup;
  constructor(private fb: FormBuilder, private alert: AlertService, private tobefilledbysupervisor: ToBeFilledBySupervisorService) { }

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
      this.alert.showError('Please fill all fields correctly before adding a new row.');
      return;
    }
    this.rows.push(this.createRow());
  }

  removeRow(index: number): void {
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      this.rows.removeAt(index);
    });
  }

  submit(): void {
    if (this.jobForm.valid) {

      const tobefilledbysupervisorDetails = this.jobForm.value.rows.map((row: any) => ({
          driverVoice: row.driverVoice,
          supervisorInstructions: row.supervisorInstructions,
          actionTaken: row.actionTaken,
          startTime: row.startTime,
          endTime: row.endTime
        }));

      this.tobefilledbysupervisor.createSupervisor(tobefilledbysupervisorDetails).subscribe({
          next: (res: any) => {
            this.alert.showInfo(res.message || 'All details submitted successfully!', () => {
              this.rows.clear();
              this.addRow();
            });
          },
          error: (err: any) => {
            this.alert.showError(err?.error.title || 'Error saving details!');
          }
        });
      } else {
        this.jobForm.markAllAsTouched();
        this.alert.showError('Please fix validation errors before submitting!');
        return;
      }
    }
 }
  
