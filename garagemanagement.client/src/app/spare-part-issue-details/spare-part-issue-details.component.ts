import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-spare-part-issue-details',
  standalone: false,
  templateUrl: './spare-part-issue-details.component.html',
  styleUrls: ['./spare-part-issue-details.component.css']
})
export class SparePartIssueDetailsComponent {
  partsForm: FormGroup;
  errorMessage = '';

  // Pagination
  pageSize = 2;
  currentPage = 1;

  constructor(private fb: FormBuilder) {
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
      this.errorMessage = '⚠️ Please fill all fields correctly before adding a new row.';
      return;
    }
    this.errorMessage = '';

    const partForm = this.fb.group({
      description: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      partNumber: ['', [Validators.required, Validators.pattern(/^[a-zA-Z0-9-_/#]+$/)]],
      make: ['', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      unitCost: ['', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      qty: ['', [Validators.required, Validators.pattern(/^[0-9]+$/)]],
    });

    this.parts.push(partForm);
    this.currentPage = this.totalPages; // Go to page where new row belongs
  }

  removePart(index: number) {
    this.parts.removeAt(index);
    if (this.currentPage > this.totalPages) {
      this.currentPage = this.totalPages;
    }
  }

  getAmount(index: number): number {
    const unitCost = parseFloat(this.parts.at(index).get('unitCost')?.value) || 0;
    const qty = parseInt(this.parts.at(index).get('qty')?.value, 10) || 0;
    return unitCost * qty;
  }

  getTotalAmount(): number {
    return this.parts.controls.reduce((sum, _, i) => sum + this.getAmount(i), 0);
  }

  onSubmit() {
    if (this.partsForm.valid) {
      console.log(this.partsForm.value);
      alert('✅ Form submitted successfully!');
    } else {
      alert('⚠️ Please fix validation errors before submitting!');
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
}
