import { Component, OnInit } from '@angular/core';
import { LabourDetailsService } from '../labour-details.service';
import { SparePartsIssueDetailsService } from '../spare-parts-issue-details.service';
interface RepairItem {
  sno: string;
  description: string;
  amount: number;
}

@Component({
  selector: 'app-total-repair-cost',
  standalone: false,
  templateUrl: './total-repair-cost.component.html',
  styleUrl: './total-repair-cost.component.css'
})
export class TotalRepairCostComponent {

  constructor(
    private labourDetailsService: LabourDetailsService, private sparePartsIssueDetailsService: SparePartsIssueDetailsService

  ) { }

  repairItems: RepairItem[] = [
    { sno: '1', description: 'Parts Charges', amount: 0 },
    { sno: '2', description: 'Labour Charges', amount: 0 },
    { sno: '3', description: 'Outside Labour', amount: 0 }
  ];

  grandTotal: number = 0;

  ngOnInit(): void {

    // Subscribe to Parts total from SparePartsIssueDetailsService

    this.sparePartsIssueDetailsService.partsTotal$.subscribe(total => {
      this.repairItems[0].amount = total;
      this.calculateTotal();
    });

    // Subscribe to live totals from LabourDetailsService

    this.labourDetailsService.labourChargesTotal$.subscribe(total => {
      this.repairItems[1].amount = total;
      this.calculateTotal();
    });

    this.labourDetailsService.outsideLabourTotal$.subscribe(total => {
      this.repairItems[2].amount = total;
      this.calculateTotal();
    });
  }

  calculateTotal(): void {
    this.grandTotal = this.repairItems.reduce((sum, item) => sum + (item.amount || 0), 0);
  }
  allowOnlyPositiveNumbers(item: any, field: string, event: any) {
    let input = event.target.value;

    // 1️⃣ Remove all non-digit characters
    input = input.replace(/[^0-9]/g, '');

    // 2️⃣ Enforce max 7 digits
    if (input.length > 7) {
      input = input.slice(0, 7);
    }

    // 3️⃣ Update the value and model
    event.target.value = input;
    item[field] = input ? parseInt(input, 10) : 0;

    // 4️⃣ Recalculate total
    this.calculateTotal();
  }

}
