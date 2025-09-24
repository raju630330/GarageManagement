import { Component, OnInit } from '@angular/core';
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
  repairItems: RepairItem[] = [
    { sno: '1', description: 'Parts Charges', amount: 300 },
    { sno: '2', description: 'Labour Charges', amount: 200 },
    { sno: '3', description: 'Outside Labour', amount: 100 }
  ];

  grandTotal: number = 0;

  ngOnInit(): void {
    this.calculateTotal();
  }

  calculateTotal(): void {
    this.grandTotal = this.repairItems.reduce((sum, item) => sum + (item.amount || 0), 0);
  }
}
