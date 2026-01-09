import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';

interface IssueItem {
  inStock: boolean;
  partName: string;
  partNo: string;
  brand: string;
  qtyOnHand: number;
  requestedQty?: number;
  issueQty?: number;
  pendingQty?: number;
  sellingPrice: number;
  issuedTo?: string;
}

@Component({
  selector: 'app-issue',
  templateUrl: './issue.component.html',
  styleUrls: ['./issue.component.css'],
  standalone: false
})
export class IssueComponent implements OnInit {

  issueTabs = ['pending', 'issued', 'returned'];
  activeIssueTab = 'pending';

  searchControl = new FormControl('');
  qtyControl = new FormControl('');

  issueItems: IssueItem[] = [];
  filteredIssueItems: IssueItem[] = [];

  ngOnInit(): void {
    // Dummy Data for Frontend
    this.issueItems = [
      { inStock: true, partName: 'Yellow Fog Lamp', partNo: 'FOG/LAMP/Y', brand: 'ABC', qtyOnHand: 3, requestedQty: 2, sellingPrice: 400 },
      { inStock: false, partName: '3mm Copper Wire', partNo: '3MM WIRE', brand: 'XYZ', qtyOnHand: 0, requestedQty: 5, sellingPrice: 80 }
    ];

    this.filteredIssueItems = [...this.issueItems];

    // Filter on search or quantity input
    this.searchControl.valueChanges.subscribe(() => this.applyFilters());
    this.qtyControl.valueChanges.subscribe(() => this.applyFilters());
  }

  setActiveIssueTab(tab: string) {
    this.activeIssueTab = tab;
    this.applyFilters();
  }

  applyFilters() {
    const search = this.searchControl.value?.toLowerCase() || '';
    const qty = Number(this.qtyControl.value);  // <-- convert to number

    this.filteredIssueItems = this.issueItems.filter(item => {
      const matchesSearch =
        item.partName.toLowerCase().includes(search) ||
        item.partNo.toLowerCase().includes(search);

      const matchesQty = !isNaN(qty) ? item.qtyOnHand >= qty : true; // check if valid number

      return matchesSearch && matchesQty;
    });
  }

}
