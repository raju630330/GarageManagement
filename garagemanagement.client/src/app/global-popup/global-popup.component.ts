import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-global-popup',
  templateUrl: './global-popup.component.html',
  styleUrls: ['./global-popup.component.css'],
  standalone: false
})
export class GlobalPopupComponent implements OnInit {
  tableData: any[] = [];
  displayedColumns: string[] = ['jobCardNo', 'date', 'status'];

  constructor(
    public dialogRef: MatDialogRef<GlobalPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
    if (this.data.fieldsArray) {
      this.tableData = this.data.fieldsArray;
    } else if (this.data.fields) {
      // fallback if keyvalue format
      this.tableData = Object.keys(this.data.fields).map(key => ({
        key,
        value: this.data.fields[key]
      }));
      this.displayedColumns = ['key', 'value'];
    }
  }

  close() {
    this.dialogRef.close();
  }
}
