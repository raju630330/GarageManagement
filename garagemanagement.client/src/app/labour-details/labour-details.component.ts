import { Component } from '@angular/core';
import { LabourDetailsService } from '../labour-details.service';


@Component({
  selector: 'app-labour-details',
  standalone: false,
  templateUrl: './labour-details.component.html',
  styleUrls: ['./labour-details.component.css']
})
export class LabourDetailsComponent {

  constructor(private labourDetailsService: LabourDetailsService) { }

  labourDetails = [
    { serialNo: 1, description: '', labourCharges: 0, outsideLabour: 0, amount: 0 }
  ];

  updateAmount(detail: any) {
    detail.amount = (detail.labourCharges || 0) + (detail.outsideLabour || 0);
    this.calculateTotal();
  }

  calculateTotal(): number {
    const total = this.labourDetails.reduce((sum, d) => sum + (d.amount || 0), 0);

    // Send totals to service
    this.labourDetailsService.setLabourChargesTotal(this.getLabourChargesTotal());
    this.labourDetailsService.setOutsideLabourTotal(this.getOutsideLabourTotal());

    return total;
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
    this.calculateTotal();
  }
  allowOnlyPositiveNumbers(detail: any, field: string, event: any) {
  const input = event.target.value;
  // Remove anything that isn't a digit
  const numericValue = input.replace(/[^0-9]/g, '');
  event.target.value = numericValue;
  detail[field] = numericValue ? parseInt(numericValue, 10) : 0;

  this.updateAmount(detail); // Keep your existing logic
  }
  getLabourChargesTotal() {
    return this.labourDetails.reduce((sum, d) => sum + (d.labourCharges || 0), 0);
  }

  getOutsideLabourTotal() {
    return this.labourDetails.reduce((sum, d) => sum + (d.outsideLabour || 0), 0);
  }
}
