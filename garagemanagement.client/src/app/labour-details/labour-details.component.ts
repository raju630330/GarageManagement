import { Component } from '@angular/core';


@Component({
  selector: 'app-labour-details',
  standalone: false,
  templateUrl: './labour-details.component.html',
  styleUrls: ['./labour-details.component.css']
})
export class LabourDetailsComponent {
  labourDetails = [
    { serialNo: 1, description: '', labourCharges: 0, outsideLabour: 0, amount: 0 }
  ];

  updateAmount(detail: any) {
    detail.amount = (detail.labourCharges || 0) + (detail.outsideLabour || 0);
  }

  calculateTotal(): number {
    return this.labourDetails.reduce((sum, d) => sum + (d.amount || 0), 0);
  }

  addRow() {
    const nextSerialNo = this.labourDetails.length + 1;
    this.labourDetails.push({
      serialNo: nextSerialNo,
      description: '',
      labourCharges: 0,
      outsideLabour: 0,
      amount: 0
    });
  }

  deleteRow(index: number) {
    this.labourDetails.splice(index, 1);
    this.labourDetails.forEach((row, i) => row.serialNo = i + 1);
  }
}
