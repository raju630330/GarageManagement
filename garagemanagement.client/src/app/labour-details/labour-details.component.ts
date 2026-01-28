import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LabourDetailsService } from '../services/labour-details.service';
import { ROLES } from '../constants/roles.constants';
import { AlertService } from '../services/alert.service';
import { RepairOrderService } from '../services/repair-order.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-labour-details',
  standalone: false,
  templateUrl: './labour-details.component.html',
  styleUrls: ['./labour-details.component.css']
})
export class LabourDetailsComponent implements OnInit, OnDestroy {

  ROLES = ROLES;
  labourForm!: FormGroup;

  // 🔥 ADDED
  repairOrderId: number | null = null;
  private sub!: Subscription;

  constructor(
    private fb: FormBuilder,
    private labourDetailsService: LabourDetailsService,
    private repairOrderService: RepairOrderService, // 🔥 ADDED
    private alert: AlertService
  ) { }

  ngOnInit(): void {
    this.labourForm = this.fb.group({
      labourDetails: this.fb.array([this.createRow()])
    });

    this.labourDetails.valueChanges.subscribe(() => {
      this.updateTotals();
    });

    // 🔥 ADDED – SAME FLOW AS SPARE PARTS
    this.sub = this.repairOrderService.repairOrderId$.subscribe(id => {
      if (!id || id === this.repairOrderId) return;

      this.repairOrderId = id;
      this.loadLabourDetails(id);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  get labourDetails(): FormArray {
    return this.labourForm.get('labourDetails') as FormArray;
  }

  createRow(data?: any): FormGroup {
    return this.fb.group({
      description: [data?.description || '', [Validators.required, Validators.maxLength(50)]],
      labourCharges: [data?.labourCharges || '', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      outsideLabour: [data?.outsideLabour || '', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
    });
  }

  addRow(): void {
    if (this.labourDetails.length > 0 && this.labourDetails.at(this.labourDetails.length - 1).invalid) {
      this.labourForm.markAllAsTouched();
      this.alert.showError('Please fill all fields correctly before adding a new row.');
      return;
    }
    this.labourDetails.push(this.createRow());
  }

  removeRow(index: number): void {
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      this.labourDetails.removeAt(index);
    });
  }

  loadLabourDetails(repairOrderId: number) {
    this.labourDetailsService.getLabourDetails(repairOrderId).subscribe({
      next: (res: any[]) => {
        this.labourDetails.clear();

        if (res && res.length > 0) {
          res.forEach(row => this.labourDetails.push(this.createRow(row)));
        } else {
          this.addRow();
        }

        this.updateTotals();
      },
      error: () => {
        this.labourDetails.clear();
        this.addRow();
      }
    });
  }

  submit(): void {

    // 🔥 ADDED
    if (!this.repairOrderId) {
      this.alert.showError('⚠️ Please select Repair Order first!');
      return;
    }

    if (this.labourForm.valid) {

      const labourArray = this.labourForm.value.labourDetails.map((row: any) => ({
        repairOrderId: this.repairOrderId, // 🔥 ADDED
        description: row.description,
        labourCharges: parseFloat(row.labourCharges),
        outsideLabour: parseFloat(row.outsideLabour)
      }));

      this.labourDetailsService.addLabourDetails(labourArray).subscribe({
        next: (res: any) => {
          this.alert.showInfo(res.message || 'Labour details submitted successfully!');
        },
        error: (err: any) => {
          this.alert.showError(err?.error || 'Something went wrong');
        }
      });

    } else {
      this.labourForm.markAllAsTouched();
      this.alert.showError('Please fix validation errors before submitting!');
      return;
    }
  }

  calculateTotal(): number {
    return this.labourDetails.controls.reduce((sum, row: any) => {
      const labour = Number(row.value.labourCharges || 0);
      const outside = Number(row.value.outsideLabour || 0);
      return sum + labour + outside;
    }, 0);
  }

  updateTotals() {
    let labourTotal = 0;
    let outsideTotal = 0;

    this.labourDetails.controls.forEach(ctrl => {
      labourTotal += parseFloat(ctrl.get('labourCharges')?.value || '0');
      outsideTotal += parseFloat(ctrl.get('outsideLabour')?.value || '0');
    });

    labourTotal = parseFloat(labourTotal.toFixed(2));
    outsideTotal = parseFloat(outsideTotal.toFixed(2));

    this.labourDetailsService.setLabourChargesTotal(labourTotal);
    this.labourDetailsService.setOutsideLabourTotal(outsideTotal);
  }

  forceNumberswithtwodecimalsOnly(index: number, field: string) {
    const control = this.labourDetails.at(index).get(field);
    let value = control?.value ?? "";

    value = value.replace(/[^0-9.]/g, "");
    const parts = value.split('.');
    if (parts.length > 2) parts.splice(2);
    if (parts[1]) parts[1] = parts[1].substring(0, 2);

    control?.setValue(parts.join('.'), { emitEvent: false });
  }

  forceNumbersOnly(index: number, field: string) {
    const control = this.labourDetails.at(index).get(field);
    let value = control?.value ?? "";

    value = value.replace(/[^0-9]/g, "");
    control?.setValue(value, { emitEvent: false });
  }
}
