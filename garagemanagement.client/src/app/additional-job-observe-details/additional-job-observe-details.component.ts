import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ROLES } from '../constants/roles.constants';
import { AlertService } from '../services/alert.service';
import { AdditionalJobObserveDetailsService } from '../services/additional-job-observe-details.service';
import { Subscription } from 'rxjs';
import { RepairOrderService } from '../services/repair-order.service';

@Component({
  selector: 'app-additional-job-observe-details',
  templateUrl: './additional-job-observe-details.component.html',
  styleUrls: ['./additional-job-observe-details.component.css'],
  standalone: false
})
export class AdditionalJobObserveDetailsComponent implements OnInit, OnDestroy {

  ROLES = ROLES;
  jobForm!: FormGroup;
  repairOrderId!: number;
  private sub!: Subscription;

  constructor(
    private fb: FormBuilder,
    private alert: AlertService,
    private repairOrderService: RepairOrderService,
    private observationdetailsservice: AdditionalJobObserveDetailsService
  ) { }

  ngOnInit(): void {
    this.jobForm = this.fb.group({
      rows: this.fb.array([])
    });

    this.sub = this.repairOrderService.repairOrderId$.subscribe(id => {
      if (!id || id === this.repairOrderId) return;
      this.repairOrderId = id;
      this.loadSupervisorDetails(id);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  get rows(): FormArray {
    return this.jobForm.get('rows') as FormArray;
  }

  createRow(slNo: number, data?: any): FormGroup {
    return this.fb.group({
      slNo: [slNo],
      technicianVoice: [
        data?.technicianVoice || '',
        [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]
      ],
      supervisorInstructions: [
        data?.supervisorInstructions || '',
        [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]
      ],
      actionTaken: [
        data?.actionTaken || '',
        [Validators.required, Validators.pattern(/^[A-Za-z ]{1,50}$/)]
      ],
      startTime: [
        data?.startTime || '',
        [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]
      ],
      endTime: [
        data?.endTime || '',
        [Validators.required, Validators.pattern(/^(0[1-9]|1[0-2]):[0-5][0-9]\s?(AM|PM)$/i)]
      ]
    });
  }

  addRow(data?: any): void {
    if (this.rows.length > 0 && this.rows.at(this.rows.length - 1).invalid) {
      this.jobForm.markAllAsTouched();
      this.alert.showError('Please fill all fields before adding a new row.');
      return;
    }
    this.rows.push(this.createRow(this.rows.length + 1, data));
  }

  removeRow(index: number): void {
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      this.rows.removeAt(index);
      this.recalculateSerialNumbers();
    });
  }

  recalculateSerialNumbers(): void {
    this.rows.controls.forEach((ctrl, i) => {
      ctrl.get('slNo')?.setValue(i + 1);
    });
  }

  submit(): void {
    if (this.jobForm.invalid) {
      this.jobForm.markAllAsTouched();
      this.alert.showError('Please fix validation errors before submitting!');
      return;
    }

    const payload = this.rows.value.map((row: any) => ({
      technicianVoice: row.technicianVoice,
      supervisorInstructions: row.supervisorInstructions,
      actionTaken: row.actionTaken,
      startTime: row.startTime,
      endTime: row.endTime,
      repairOrderId: this.repairOrderId
    }));

    this.observationdetailsservice
      .createAdditionalJobObserveDetails(payload)
      .subscribe({
        next: (res: any) => {
          this.alert.showInfo(res.message || 'Details updated successfully');
        },
        error: () => {
          this.alert.showError('Error saving details!');
        }
      });
  }

  loadSupervisorDetails(repairOrderId: number): void {
    this.observationdetailsservice
      .getAdditionalJobObserveDetails(repairOrderId)
      .subscribe({
        next: (res: any[]) => {
          this.rows.clear();
          if (res?.length) {
            res.forEach(r => this.addRow(r));
          } else {
            this.addRow();
          }
        },
        error: () => {
          this.rows.clear();
          this.addRow();
        }
      });
  }
}
