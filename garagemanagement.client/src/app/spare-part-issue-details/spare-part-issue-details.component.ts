import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SparePartsIssueDetailsService } from '../services/spare-parts-issue-details.service';
import { ROLES } from '../constants/roles.constants';
import { AlertService } from '../services/alert.service';
import { RepairOrderService } from '../services/repair-order.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-spare-part-issue-details',
  standalone: false,
  templateUrl: './spare-part-issue-details.component.html',
  styleUrls: ['./spare-part-issue-details.component.css']
})
export class SparePartIssueDetailsComponent implements OnInit, OnDestroy {

  ROLES = ROLES;
  partsForm!: FormGroup;

  repairOrderId: number | null = null;
  private sub!: Subscription;

  // Pagination
  pageSize = 2;
  currentPage = 1;

  constructor(
    private fb: FormBuilder,
    private sparePartsIssueDetailsService: SparePartsIssueDetailsService,
    private repairOrderService: RepairOrderService,
    private alert: AlertService
  ) { }

  ngOnInit(): void {
    this.createForm();

    // 🔥 Listen to Repair Order Id (Supervisor-style flow)
    this.sub = this.repairOrderService.repairOrderId$.subscribe(id => {
      if (!id || id === this.repairOrderId) return;

      this.repairOrderId = id;
      this.loadSpareParts(id);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  /* ================= FORM ================= */

  createForm() {
    this.partsForm = this.fb.group({
      parts: this.fb.array([])
    });

    this.addPart(); // Start with one row
  }

  get parts(): FormArray {
    return this.partsForm.get('parts') as FormArray;
  }

  /* ================= PAGINATION ================= */

  get pagedParts(): FormGroup[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.parts.controls.slice(start, start + this.pageSize) as FormGroup[];
  }

  get totalPages(): number {
    return Math.ceil(this.parts.length / this.pageSize) || 1;
  }

  /* ================= ROW ================= */

  createPart(data?: any): FormGroup {
    return this.fb.group({
      description: [data?.description || '', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      partNumber: [data?.partNumber || '', [Validators.required, Validators.pattern(/^[a-zA-Z0-9-_/#]+$/)]],
      make: [data?.make || '', [Validators.required, Validators.pattern(/^[a-zA-Z\s]+$/)]],
      unitCost: [data?.unitCost || '', [Validators.required, Validators.pattern(/^\d+(\.\d{1,2})?$/)]],
      qty: [data?.quantity || '', [Validators.required, Validators.pattern(/^[0-9]+$/)]],
    });
  }

  addPart(data?: any) {
    if (this.parts.length > 0 && this.parts.at(this.parts.length - 1).invalid) {
      this.partsForm.markAllAsTouched();
      this.alert.showError('Please fill all fields correctly before adding a new row.');
      return;
    }

    this.parts.push(this.createPart(data));
    this.currentPage = this.totalPages;
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

  /* ================= LOAD (GET) ================= */

  loadSpareParts(repairOrderId: number) {
    this.sparePartsIssueDetailsService
      .getSpareParts(repairOrderId)
      .subscribe({
        next: (res: any[]) => {
          this.parts.clear();

          if (res && res.length > 0) {
            res.forEach(p => this.addPart(p));
          } else {
            this.addPart(); // blank row
          }

          this.updateTotals();
        },
        error: () => {
          this.parts.clear();
          this.addPart();
        }
      });
  }

  /* ================= CALCULATIONS ================= */

  getAmount(index: number): number {
    const unitCost = parseFloat(this.parts.at(index).get('unitCost')?.value) || 0;
    const qty = parseInt(this.parts.at(index).get('qty')?.value, 10) || 0;
    return unitCost * qty;
  }

  getTotalAmount(): number {
    return this.parts.controls.reduce((sum, _, i) => sum + this.getAmount(i), 0);
  }

  updateTotals() {
    this.sparePartsIssueDetailsService.setPartsTotal(this.getTotalAmount());
  }

  /* ================= SUBMIT (POST) ================= */

  onSubmit() {
    if (!this.repairOrderId) {
      this.alert.showError('⚠️ Please select Repair Order first!');
      return;
    }

    if (this.partsForm.invalid) {
      this.partsForm.markAllAsTouched();
      this.alert.showError('Please fix validation errors before submitting!');
      return;
    }

    const payload = this.parts.value.map((part: any) => ({
      repairOrderId: this.repairOrderId,
      description: part.description,
      partNumber: part.partNumber,
      make: part.make,
      unitCost: parseFloat(part.unitCost),
      quantity: parseInt(part.qty, 10)
    }));

    this.sparePartsIssueDetailsService.createSpareParts(payload).subscribe({
      next: (res: any) => {
        this.alert.showSuccess(res.message || 'Spare parts saved successfully!');
      },
      error: (err: any) => {
        this.alert.showError(err?.error || 'Error saving spare parts!');
      }
    });
  }

  /* ================= INPUT HELPERS ================= */

  forceNumberswithtwodecimalsOnly(index: number, field: string) {
    const control = this.parts.at(index).get(field);
    let value = control?.value ?? "";

    value = value.replace(/[^0-9.]/g, "");
    const parts = value.split('.');
    if (parts.length > 2) parts.splice(2);
    if (parts[1]) parts[1] = parts[1].substring(0, 2);

    control?.setValue(parts.join('.'), { emitEvent: false });
  }

  forceNumbersOnly(index: number, field: string) {
    const control = this.parts.at(index).get(field);
    let value = control?.value ?? "";
    value = value.replace(/[^0-9]/g, "");
    control?.setValue(value, { emitEvent: false });
  }

  nextPage() {
    if (this.currentPage < this.totalPages) this.currentPage++;
  }

  prevPage() {
    if (this.currentPage > 1) this.currentPage--;
  }
}
