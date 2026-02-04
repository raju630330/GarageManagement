import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { IssueService, PendingIssueItem } from '../services/issue.service';

interface IssueItem {
  inStock?: boolean;
  partName: string;
  partNo: string;
  brand?: string;
  qtyOnHand?: number;
  requestedQty?: number;
  issuedQty?: number;
  pendingQty?: number;
  sellingPrice: number;
  issueQty?: number;
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

  jobCardId = 53; // 🔥 get from route later

  constructor(private issueService: IssueService) { }

  ngOnInit(): void {
    this.loadDataByTab();

    this.searchControl.valueChanges.subscribe(() => this.applyFilters());
    this.qtyControl.valueChanges.subscribe(() => this.applyFilters());
  }

  setActiveIssueTab(tab: string) {
    this.activeIssueTab = tab;
    this.loadDataByTab();
  }

  // 🔥 CORE LOGIC
  loadDataByTab() {
    if (this.activeIssueTab === 'pending') {
      this.loadPendingIssues();
    } else if (this.activeIssueTab === 'issued') {
      this.loadIssuedIssues();
    } else if (this.activeIssueTab === 'returned') {
      this.loadReturnedIssues();
    }
  }

  loadPendingIssues() {
    this.issueService.getPendingIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.issueItems = res.data.map((x: PendingIssueItem) => ({
          partName: x.partName,
          partNo: x.partNo,
          brand: x.brand,
          qtyOnHand: x.qtyOnHand,
          inStock: x.inStock,
          requestedQty: x.requestedQty,
          issuedQty: x.issuedQty,
          pendingQty: x.pendingQty,
          sellingPrice: x.sellingPrice,
          issueQty: x.issueQty ?? 0,     // default 0 if null
          issuedTo: x.issuedTo ?? ''     // default empty if null
        }));

        this.filteredIssueItems = [...this.issueItems];
      }
    });
  }


  loadIssuedIssues() {
    this.issueService.getIssuedIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.issueItems = res.data;
        this.filteredIssueItems = [...this.issueItems];
      }
    });
  }

  loadReturnedIssues() {
    this.issueService.getReturnedIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.issueItems = res.data;
        this.filteredIssueItems = [...this.issueItems];
      }
    });
  }

  applyFilters() {
    const search = this.searchControl.value?.toLowerCase() || '';
    const qty = Number(this.qtyControl.value);

    this.filteredIssueItems = this.issueItems.filter(item => {
      const matchesSearch =
        item.partName.toLowerCase().includes(search) ||
        item.partNo.toLowerCase().includes(search);

      const matchesQty = !isNaN(qty)
        ? (item.pendingQty ?? 0) >= qty
        : true;

      return matchesSearch && matchesQty;
    });
  }
}
