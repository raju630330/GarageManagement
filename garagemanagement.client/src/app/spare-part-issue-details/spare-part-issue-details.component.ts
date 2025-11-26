import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SparePartsIssueDetailsService } from '../services/spare-parts-issue-details.service';
import { ROLES } from '../constants/roles.constants';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-spare-part-issue-details',
  standalone: false,
  templateUrl: './spare-part-issue-details.component.html',
  styleUrls: ['./spare-part-issue-details.component.css']
})
export class SparePartIssueDetailsComponent {
  ROLES = ROLES;
  partsForm: FormGroup;

  // Pagination
  pageSize = 2;
  currentPage = 1;

  constructor(private fb: FormBuilder, private sparePartsIssueDetailsService: SparePartsIssueDetailsService, private alert: AlertService) {
    this.partsForm = this.fb.group({
      parts: this.fb.array([])
    });

    this.addPart(); // Start with one row
  }

  get parts(): FormArray {
    return this.partsForm.get('parts') as FormArray;
  }

  /** Slice controls for current page */
  get pagedParts(): FormGroup[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.parts.controls.slice(start, start + this.pageSize) as FormGroup[];
  }

  get totalPages(): number {
    return Math.ceil(this.parts.length / this.pageSize) || 1;
  }

  addPart() {
    if (this.parts.length > 0 && this.parts.at(this.parts.length - 1).invalid) {
      this.partsForm.markAllAsTouched();
      this.alert.showError('Please fill all fields correctly before adding a new row.');
      return;
    }

    const partForm = this.fb.group({
      description: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      partNumber: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9-_/#]+$/)]],
      make: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      unitCost: ['', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      qty: ['', [Validators.required, Validators.pattern(/^[0-9]+$/)]],
    });

    this.parts.push(partForm);
    this.currentPage = this.totalPages; // Go to page where new row belongs
    this.updateTotals();
  }

  removePart(index: number) {
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      this.parts.removeAt(index);
      if (this.currentPage > this.totalPages) {
        this.currentPage = this.totalPages;
      }
      this.updateTotals();
    });
  }

  getAmount(index: number): number {
    const unitCost = parseFloat(this.parts.at(index).get('unitCost')?.value) || 0;
    const qty = parseInt(this.parts.at(index).get('qty')?.value, 10) || 0;
    return unitCost * qty;
  }

  getTotalAmount(): number {
    return this.parts.controls.reduce((sum, _, i) => sum + this.getAmount(i), 0);
  }

  // Push total into service

  updateTotals() {
    const total = this.getTotalAmount();
    this.sparePartsIssueDetailsService.setPartsTotal(total);
  }

  onSubmit() {
    if (this.partsForm.valid) {
      const partsArray = this.partsForm.value.parts.map((part: any) => ({
        description: part.description,
        partNumber: part.partNumber,
        make: part.make,
        unitCost: parseFloat(part.unitCost),
        quantity: parseInt(part.qty, 10)
      }));

      this.sparePartsIssueDetailsService.createSpareParts(partsArray).subscribe({
        next: (res: any) => {
          this.alert.showInfo(res.message || 'All parts submitted successfully!', () => {
            this.parts.clear();
            this.addPart();
          });
        },
        error: (err: any) => {
          this.alert.showError(err?.error || 'Error saving parts!');
        }
      });
    } else {
      this.partsForm.markAllAsTouched();
      this.alert.showError('Please fix validation errors before submitting!');
      return;
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }

  forceNumberswithtwodecimalsOnly(index: number, field: string) {
    const control = this.parts.at(index).get(field);
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
    const control = this.parts.at(index).get(field);
    let value = control?.value ?? "";

    // Remove everything except digits
    value = value.replace(/[^0-9]/g, "");

    control?.setValue(value, { emitEvent: false });
  }
}
