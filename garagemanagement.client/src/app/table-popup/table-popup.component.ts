import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormArray, FormGroup, Validators } from '@angular/forms';
import { AlertService } from '../services/alert.service';
import { ROLES } from '../constants/roles.constants';

@Component({
  selector: 'app-table-popup',
  templateUrl: './table-popup.component.html',
  styleUrls: ['./table-popup.component.css'],  // corrected: styleUrls not styleUrl
  standalone: false
})
export class TablePopupComponent implements OnInit {
  ROLES = ROLES;
  activeTab: string;
  form!: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private dialogRef: MatDialogRef<TablePopupComponent>,
    private fb: FormBuilder, private alert: AlertService,
  ) {
    this.activeTab = data.activeTab || data.tabs[0];
  }

  ngOnInit(): void {
    this.form = this.fb.group({
      tyreBattery: this.fb.array([]),
      cancelledInvoices: this.fb.array([]),
      serviceSuggestions: ['', Validators.required],
      collections: this.fb.array([]),
      remarks: ['', Validators.required],
    });

    for (const tab of this.data.tabs) {

      // Skip single-value tabs
      if (tab === 'SERVICE SUGGESTIONS') {
        this.form.patchValue({ serviceSuggestions: this.data.serviceSuggestions || '' });
        continue;
      }
      if (tab === 'REMARKS') {
        this.form.patchValue({ remarks: this.data.remarks || '' });
        continue;
      }

      const arr = this.getArray(tab);
      const key = tabKey(tab);             // ✅ use tabKey here
      const existingData = this.data[key] || [];

      if (existingData.length) {
        existingData.forEach((item: any) => arr.push(this.createRow(tab, item)));
      } else {
        arr.push(this.createRow(tab));
      }
    }
  }

  setTab(tab: string) {
    this.activeTab = tab;
  }

  // Create row with default values and validators
  createRow(tab: string, data: any = {}): FormGroup {
    const parseDate = (d: string | undefined) => d ? new Date(d) : null;

    switch (tab) {
      case 'TYRE/BATTERY':
        return this.fb.group({
          type: [data.type || 'Tyre', Validators.required],
          brand: [data.brand || '', Validators.required],
          model: [data.model || '', Validators.required],
          manufactureDate: [parseDate(data.manufactureDate), Validators.required],
          expiryDate: [parseDate(data.expiryDate), Validators.required],
          condition: [data.condition || '', Validators.required]
        });

      case 'CANCELLED INVOICES':
        return this.fb.group({
          invoiceNo: [data.invoiceNo || '', Validators.required],
          date: [parseDate(data.date), Validators.required],
          amount: [data.amount || 0, [Validators.required, Validators.min(0)]]
        });

      case 'COLLECTIONS':
        return this.fb.group({
          type: [data.type || 'Cash', Validators.required],
          bank: [data.bank || '', Validators.required],
          chequeNo: [data.chequeNo || '', Validators.required],
          amount: [data.amount || 0, [Validators.required, Validators.min(0)]],
          date: [parseDate(data.date), Validators.required],
          invoiceNo: [data.invoiceNo || '', Validators.required],
          remarks: [data.remarks || '', Validators.required]
        });

      default:
        return this.fb.group({});
    }
  }

  // Get FormArray for a given tab
  getArray(tab: string): FormArray {
    const key = tabKey(tab);
    return this.form.get(key) as FormArray;
  }

  // Add a new row to a tab
  addRow(tab: string): void {
    const arr = this.getArray(tab);
    if (!arr) return;

    // Check if last row is invalid
    const lastRow = arr.at(arr.length - 1);
    if (lastRow.invalid) {
      // Mark all controls in the form as touched to show validation messages
      this.form.markAllAsTouched();
      this.alert.showError('Please fill all fields correctly before adding a new row.');
      return;
    }

    // If valid, add a new row
    arr.push(this.createRow(tab));
  }

  // Remove a row from a tab
  removeRow(tab: string, index: number): void {
    const arr = this.getArray(tab);
    if (!arr) return;

    const row = arr.at(index);

    if (arr.length <= 1) {
      this.alert.showError('At least one row is required.');
      return;
    }

    if (row.invalid) {
      this.alert.confirm('This row has invalid/unsaved data. Are you sure you want to remove it?', () => {
        arr.removeAt(index);
      });
      return;
    }

    // Normal remove
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      arr.removeAt(index);
    });
  }

  // Save data
  save() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      this.alert.showError('Please fill all required fields.');
      return;
    }

    const result = this.form.value;
    console.log('Saved data:', result);
    this.dialogRef.close(result);
  }

  close() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      this.alert.showError('Please fill all required fields.');
      return;
    }
    this.dialogRef.close(this.form.value);
  }

}

// Helper functions to map tab names to FormArray keys
function tabKey(tab: string): string {
  switch (tab) {
    case 'TYRE/BATTERY': return 'tyreBattery';
    case 'CANCELLED INVOICES': return 'cancelledInvoices';
    case 'SERVICE SUGGESTIONS': return 'serviceSuggestions';
    case 'COLLECTIONS': return 'collections';
    case 'REMARKS': return 'remarks';
    default: return '';
  }
}

function toCamelCase(tab: string): string {
  const key = tab.replace(/\s/g, '');
  return key.charAt(0).toLowerCase() + key.slice(1);
}
