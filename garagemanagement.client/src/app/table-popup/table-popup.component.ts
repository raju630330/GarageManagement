import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { AbstractControl, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PopupColumnConfig, PopupTabConfig } from '../models/job-card';
import { AlertService } from '../services/alert.service';
import { ROLES } from '../constants/roles.constants';

@Component({
  selector: 'app-table-popup',
  templateUrl: './table-popup.component.html',
  styleUrls: ['./table-popup.component.css'],
  standalone: false
})
export class TablePopupComponent implements OnInit {

  form!: FormGroup;
  activeTab!: PopupTabConfig;
  ROLES = ROLES;
  // Optional: dynamic brand lists
  tyreBrands = [
    { label: 'MRF', value: 'MRF' },
    { label: 'Apollo', value: 'Apollo' },
    { label: 'JK Tyre', value: 'JK Tyre' }
  ];

  batteryBrands = [
    { label: 'Exide', value: 'Exide' },
    { label: 'Amaron', value: 'Amaron' },
    { label: 'Luminous', value: 'Luminous' }
  ];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<TablePopupComponent>, private alert: AlertService,
    @Inject(MAT_DIALOG_DATA) public data: {
      tabs: PopupTabConfig[];
      popupData: any;
      activeTabKey?: string;
    }
  ) { }

  ngOnInit(): void {
    this.createEmptyForm();   // STEP 1: Build form structure
    this.populateForm();      // STEP 2: Patch existing data

    this.activeTab =
      this.data.tabs.find(t => t.tabKey === this.data.activeTabKey)
      || this.data.tabs[0];
  }

  /* ================= FORM BUILD ================= */
  private createEmptyForm() {
    const group: any = {};
    this.data.tabs.forEach(tab => {
      if (tab.isTextarea) {
        group[tab.tabKey] = [''];
      } else {
        group[tab.tabKey] = this.fb.array([]);
      }
    });

    this.form = this.fb.group(group);
  }

  private populateForm() {
    this.data.tabs.forEach(tab => {
      if (tab.isTextarea) {
        this.form.get(tab.tabKey)?.setValue(
          this.data.popupData?.[tab.tabKey] || ''
        );
      } else {
        const arr = this.getArray(tab);
        const rows = this.data.popupData?.[tab.tabKey] || [];

        if (rows.length) {
          rows.forEach((r: any) => arr.push(this.createRow(tab, r)));
        } else {
          arr.push(this.createRow(tab));
        }
      }
    });
  }

  /* ================= HELPERS ================= */
  getArray(tab: PopupTabConfig): FormArray {
    return this.form.get(tab.tabKey) as FormArray;
  }

  createRow(tab: PopupTabConfig, data: any = {}): FormGroup {
    const row: any = {};

    tab.columns?.forEach(col => {
      let value = data[col.field];

      // ✅ EXISTING DATA → USE IT
      if (value !== undefined && value !== null && value !== '') {
        if (col.type === 'date') {
          value = this.formatDateForInput(value);
        }
      }
      // ✅ NEW ROW → APPLY DEFAULT
      else {
        value = this.getDefaultValue(col);
      }

      row[col.field] = [
        value,
        col.validators || []
      ];
    });

    return this.fb.group(row);
  }


  setTab(tab: PopupTabConfig) {
    this.activeTab = tab;
  }

  getErrorMessage(ctrl: AbstractControl | null, col: PopupColumnConfig): string {
    if (!ctrl || !ctrl.errors) return '';

    if (ctrl.hasError('required')) return `${col.header} is required`;
    if (ctrl.hasError('min')) return `${col.header} must be greater than zero`;

    return 'Invalid value';
  }

  /* ================= ACTIONS ================= */
  addRow(tab: PopupTabConfig) {
    const arr = this.getArray(tab);

    // Stop if any existing row is invalid
    if (arr.invalid) {
      arr.markAllAsTouched();
      return;
    }

    arr.push(this.createRow(tab));
  }

  removeRow(tab: PopupTabConfig, index: number) {
    const arr = this.getArray(tab); 
    if (arr.length > 1) {
    this.alert.confirm('Are you sure you want to remove this row?', () => {      
        arr.removeAt(index);
    });
      return;
    }
    this.alert.showWarning('Atleast one row is required');
  }

  save() {
    this.dialogRef.close(this.form.value);
  }

  close() {
    // Preserve current form data
    const updatedData: any = {};

    this.data.tabs.forEach(tab => {
      if (tab.isTextarea) {
        updatedData[tab.tabKey] = this.form.get(tab.tabKey)?.value;
      } else {
        updatedData[tab.tabKey] = this.getArray(tab).value;
      }
    });

    this.dialogRef.close(updatedData); // Pass updated data to parent
  }

  /* ================= DYNAMIC OPTIONS ================= */
  getBrandOptions(row: any) {
    if (row.type === 'Tyre') return this.tyreBrands;
    if (row.type === 'Battery') return this.batteryBrands;
    return [];
  }
  private formatDateForInput(value: any): string {
    if (!value) return '';

    // Already yyyy-MM-dd
    if (typeof value === 'string' && value.length === 10 && value.includes('-')) {
      return value;
    }

    const date = new Date(value);
    if (isNaN(date.getTime())) return '';

    const yyyy = date.getFullYear();
    const mm = ('0' + (date.getMonth() + 1)).slice(-2);
    const dd = ('0' + date.getDate()).slice(-2);

    return `${yyyy}-${mm}-${dd}`;
  }
  private getToday(): string {
    const d = new Date();
    const yyyy = d.getFullYear();
    const mm = ('0' + (d.getMonth() + 1)).slice(-2);
    const dd = ('0' + d.getDate()).slice(-2);
    return `${yyyy}-${mm}-${dd}`;
  }

  private getDefaultValue(col: PopupColumnConfig): any {
    switch (col.type) {
      case 'date':
        return this.getToday();               // ✅ TODAY
      case 'select':
        return col.options?.length ? col.options[0].value : '';
      default:
        return '';
    }
  }
}
