import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ROLES } from '../constants/roles.constants';
import { AlertService } from '../services/alert.service';
import { ToBeFilledBySupervisorService } from '../services/tobefilledbysupervisor.service';
import { RepairOrderService } from '../services/repair-order.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-to-be-filled-by-supervisor',
  standalone: false,
  templateUrl: './to-be-filled-by-supervisor.component.html',
  styleUrls: ['./to-be-filled-by-supervisor.component.css']
})
export class ToBeFilledBySupervisorComponent implements OnInit, OnDestroy {
  ROLES = ROLES;
  jobForm!: FormGroup;
  repairOrderId: number | null = null;
  private sub!: Subscription;

  constructor(
    private repairOrderService: RepairOrderService,
    private fb: FormBuilder,
    private alert: AlertService,
    private tobefilledbysupervisor: ToBeFilledBySupervisorService
  ) { }

  ngOnInit(): void {
    this.createForm();

    this.sub = this.repairOrderService.repairOrderId$.subscribe(id => {
      if (!id || id === this.repairOrderId) return;

      this.repairOrderId = id;
      this.loadSupervisorDetails(id);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  createForm() {
    this.jobForm = this.fb.group({
      rows: this.fb.array([])
    });

    // Start with one empty row
    this.addRow();
  }

  get rows(): FormArray {
    return this.jobForm.get('rows') as FormArray;
  }

  createRow(data?: any): FormGroup {
    return this.fb.group({
      driverVoice: [data?.driverVoice || '', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      supervisorInstructions: [data?.supervisorInstructions || '', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      actionTaken: [data?.actionTaken || '', [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]],
      startTime: [data?.startTime || '', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]],
      endTime: [data?.endTime || '', [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]]
    });
  }

  addRow(data?: any): void {
    if (this.rows.length > 0 && this.rows.at(this.rows.length - 1).invalid) {
      this.jobForm.markAllAsTouched();
      this.alert.showError('Please fill all fields correctly before adding a new row.');
      return;
    }
    this.rows.push(this.createRow(data));
  }

  removeRow(index: number): void {
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      this.rows.removeAt(index);
      if (this.rows.length === 0) this.addRow();
    });
  }

  loadSupervisorDetails(repairOrderId: number) {
    this.tobefilledbysupervisor.getSupervisorByRepairOrderId(repairOrderId)
      .subscribe({
        next: (res: any) => {
          this.rows.clear(); // Clear existing rows
          if (res && res.length > 0) {
            res.forEach((row: any) => this.addRow(row));
          } else {
            this.addRow(); // No data, add one blank row
          }
        },
        error: () => {
          this.rows.clear();
          this.addRow(); // Fallback to blank row
        }
      });
  }

  submit(): void {
    if (!this.repairOrderId) {
      this.alert.showError('⚠️ No Repair Order selected!');
      return;
    }

    if (this.jobForm.invalid) {
      this.jobForm.markAllAsTouched();
      this.alert.showError('Please fix validation errors before submitting!');
      return;
    }

    const tobefilledbysupervisorDetails = this.rows.value.map((row: any) => ({
      ...row,
      repairOrderId: this.repairOrderId
    }));

    this.tobefilledbysupervisor.createSupervisor(tobefilledbysupervisorDetails).subscribe({
      next: (res: any) => {
        this.alert.showSuccess(res.message || 'All details submitted successfully!');
      },
      error: (err: any) => {
        this.alert.showError(err?.error?.title || 'Error saving details!');
      }
    });
  }
}
