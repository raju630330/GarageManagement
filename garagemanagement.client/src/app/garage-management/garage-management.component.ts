import { Component } from '@angular/core';

@Component({
  selector: 'app-garage-management',
  standalone: false,
  templateUrl: './garage-management.component.html',
  styleUrl: './garage-management.component.css'
})
export class GarageManagementComponent {
  rows = [
    { driverVoice: '', supervisorInstructions: '', actionTaken: '', startTime: '', endTime: '' }
  ];

  // for custom alert
  showAlert = false;
  rowIndexToRemove: number | null = null;
  alertMessage = '';

  addRow() {
    this.rows.push({ driverVoice: '', supervisorInstructions: '', actionTaken: '', startTime: '', endTime: '' });
  }

  // show confirm box
  removeRow(index: number) {
    this.rowIndexToRemove = index;
    this.alertMessage = 'Are you sure you want to remove this row?';
    this.showAlert = true;
  }

  // confirm deletion
  confirmRemove() {
    if (this.rowIndexToRemove !== null) {
      this.rows.splice(this.rowIndexToRemove, 1);
    }
    this.closeAlert();
  }

  // cancel deletion
  closeAlert() {
    this.showAlert = false;
    this.rowIndexToRemove = null;
  }

  onSubmit(form: any) {
    if (form.valid) {
      alert('Form submitted successfully!');
      console.log(this.rows);
    } else {
      alert('Please correct the errors before submitting.');
    }
  }
}
