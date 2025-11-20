import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LabourDetailsService } from '../labour-details.service';

@Component({
  selector: 'app-labour-details',
  standalone: false,
  templateUrl: './labour-details.component.html',
  styleUrls: ['./labour-details.component.css']
})
export class LabourDetailsComponent implements OnInit {

  labourForm!: FormGroup;

  // Custom alert
  showAlert = false;
  alertMessage = '';
  confirmAction: (() => void) | null = null;

  constructor(private fb: FormBuilder, private labourDetailsService: LabourDetailsService) { }

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
      this.alertMessage = 'Please fill all required fields before adding a new row.';
      this.showAlert = true;
      return;
    }
    this.labourDetails.push(this.createRow());
  }

  removeRow(index: number): void {
    this.alertMessage = 'Are you sure you want to delete this row?';
    this.showAlert = true;

    this.confirmAction = () => {
      this.labourDetails.removeAt(index);
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
    if (this.labourForm.valid) {

      const labourArray = this.labourForm.value.labourDetails.map((row: any) => ({
        description: row.description,
        labourCharges: parseFloat(row.labourCharges),
        outsideLabour: parseFloat(row.outsideLabour),
        amount: parseFloat(row.labourCharges) + parseFloat(row.outsideLabour)
      }));

      this.labourDetailsService.addLabourDetails(labourArray).subscribe({
        next: (res: any) => {
          alert(res.message || 'Labour details submitted successfully!');
          this.labourDetails.clear();
          this.addRow();
          this.alertMessage = '';
        },
        error: (err: any) => {
          alert(err?.error?.message || 'Error saving labour details!');
        }
      });

    } else {
      this.labourForm.markAllAsTouched();
      this.alertMessage = 'Please fix validation errors before submitting!';
      this.showAlert = true;
      this.confirmAction = null;
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
