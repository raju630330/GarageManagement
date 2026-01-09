import { Component, OnInit } from '@angular/core';

interface InwardItem {
  partName: string;
  partNo: string;
  brand: string;
  requestedQty: number;
  rackNo: string;
  inwardQty: number;
  unitPrice: number;
  discount: number;
  hsn: string;
  taxPercent: number;
  totalPurchase: number;
  sellingPrice: number;
  margin: number;
  remarks: string;
  barcode: string;
}

@Component({
  selector: 'app-inward',
  templateUrl: './inward.component.html',
  styleUrls: ['./inward.component.css'],
  standalone: false
})
export class InwardComponent implements OnInit {

  today: Date = new Date();

  inwardItems: InwardItem[] = []; // Will populate when backend ready

  summary = {
    total: 0,
    freight: 0,
    tcs: 0,
    grandTotal: 0,
    paid: 0,
    balance: 0
  };

  constructor() { }

  ngOnInit(): void {
    // Dummy data example (frontend only)
    this.inwardItems = [
      {
        partName: '3MM WIRE',
        partNo: '3MM001',
        brand: 'XYZ',
        requestedQty: 10,
        rackNo: 'R1',
        inwardQty: 10,
        unitPrice: 50,
        discount: 0,
        hsn: '8544',
        taxPercent: 18,
        totalPurchase: 500,
        sellingPrice: 80,
        margin: 30,
        remarks: '',
        barcode: '987654321012'
      }
    ];

    // Summary calculation
    this.calculateSummary();
  }

  calculateSummary() {
    this.summary.total = this.inwardItems.reduce((sum, item) => sum + item.totalPurchase, 0);
    this.summary.grandTotal = this.summary.total + this.summary.freight + this.summary.tcs;
    this.summary.balance = this.summary.grandTotal - this.summary.paid;
  }

}
