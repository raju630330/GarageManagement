import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LabourDetailsService } from '../services/labour-details.service';
import { ROLES } from '../constants/roles.constants';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-labour-details',
  standalone: false,
  templateUrl: './labour-details.component.html',
  styleUrls: ['./labour-details.component.css']
})
export class LabourDetailsComponent implements OnInit {

  ROLES = ROLES;
  labourForm!: FormGroup;

  constructor(private fb: FormBuilder, private labourDetailsService: LabourDetailsService, private alert: AlertService) { }

  ngOnInit(): void {
    this.labourForm = this.fb.group({
      labourDetails: this.fb.array([this.createRow()])
    });
    this.labourDetails.valueChanges.subscribe(() => {
      this.updateTotals();
    });
  }

  get labourDetails(): FormArray {
    return this.labourForm.get('labourDetails') as FormArray;
  }

  createRow(): FormGroup {
    return this.fb.group({
      description: ['', [Validators.required, Validators.maxLength(50)]],
      labourCharges: ['', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      outsideLabour: ['', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
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

  submit(): void {
    if (this.labourForm.valid) {

      const labourArray = this.labourForm.value.labourDetails.map((row: any) => ({
        description: row.description,
        labourCharges: parseFloat(row.labourCharges),
        outsideLabour: parseFloat(row.outsideLabour),
        amount: parseFloat(row.labourCharges) + parseFloat(row.outsideLabour)
      }));

      this.labourDetailsService.addLabourDetails(labourArray).subscribe({
        next: (res: any) => {
          this.alert.showInfo(res.message || 'Labour details submitted successfully!', () => {
            this.labourDetails.clear();
            this.addRow();
          });
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

    // Round to 2 decimals
    labourTotal = parseFloat(labourTotal.toFixed(2));
    outsideTotal = parseFloat(outsideTotal.toFixed(2));

    this.labourDetailsService.setLabourChargesTotal(labourTotal);
    this.labourDetailsService.setOutsideLabourTotal(outsideTotal);
  }

  forceNumberswithtwodecimalsOnly(index: number, field: string) {
    const control = this.labourDetails.at(index).get(field);
    let value = control?.value ?? "";

    // Remove invalid characters – allow only digits and one dot
    value = value.replace(/[^0-9.]/g, "");

    // Ensure only one decimal point
    const parts = value.split('.');
    if (parts.length > 2) {
      parts.splice(2);
    }

    // Limit decimals to 2 digits
    if (parts[1]) {
      parts[1] = parts[1].substring(0, 2);
    }

    value = parts.join('.');

    control?.setValue(value, { emitEvent: false });
  }

  forceNumbersOnly(index: number, field: string) {
    const control = this.labourDetails.at(index).get(field);
    let value = control?.value ?? "";

    // Remove everything except digits
    value = value.replace(/[^0-9]/g, "");

    control?.setValue(value, { emitEvent: false });
  }

}
