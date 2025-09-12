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
  errorMessage: string = '';

  constructor(private fb: FormBuilder) {
    this.partsForm = this.fb.group({
      parts: this.fb.array([])
    });

    // Add first row by default
    this.addPart();
  }

  get parts(): FormArray {
    return this.partsForm.get('parts') as FormArray;
  }

  addPart() {
    // Check if last row is valid before adding new row
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
  }

  removePart(index: number) {
    this.parts.removeAt(index);
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
}

