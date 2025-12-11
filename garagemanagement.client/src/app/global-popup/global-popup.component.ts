import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

interface PopupData {
  title?: string;
  fields: Record<string, any>;
}

@Component({
  selector: 'app-global-popup',
  templateUrl: './global-popup.component.html',
  styleUrls: ['./global-popup.component.css'],
  standalone: false
})
export class GlobalPopupComponent {

  constructor(
    public dialogRef: MatDialogRef<GlobalPopupComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PopupData
  ) { }

  close() {
    this.dialogRef.close();
  }

}
