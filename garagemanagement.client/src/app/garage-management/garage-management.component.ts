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

  addRow() {
    this.rows.push({ driverVoice: '', supervisorInstructions: '', actionTaken: '', startTime: '', endTime: '' });
  }

  removeRow(index: number) {
    const confirmDelete = confirm('Are you sure you want to remove this row?');
    if (confirmDelete) {
      this.rows.splice(index, 1);
    }
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
