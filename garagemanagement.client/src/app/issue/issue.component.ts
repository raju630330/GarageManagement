import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { IssuedItem, IssueService, PendingIssueItem } from '../services/issue.service';
import { AlertService } from '../services/alert.service';

interface IssueItem {
  estimationId: number; 
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
  unitPrice: number;
  issuedId: string;
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

  issueForm!: FormGroup;

  issueItems: IssueItem[] = [];
  issuedItems: IssuedItem[] = [];
  filteredIssueItems: any[] = [];

  jobCardId!: number;

  constructor(
    private issueService: IssueService,
    private route: ActivatedRoute,
    private alert: AlertService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.issueForm = this.fb.group({
      selectAll: [false],
      items: this.fb.array([])
    });

    this.route.queryParamMap.subscribe(params => {
      const jobCardId = params.get('jobCardId');
      if (jobCardId) {
        this.jobCardId = Number(jobCardId);
        this.loadDataByTab();
      }
    });

    this.searchControl.valueChanges.subscribe(() => this.applyFilters());
    this.qtyControl.valueChanges.subscribe(() => this.applyFilters());
  }

  get itemsFormArray(): FormArray {
    return this.issueForm.get('items') as FormArray;
  }

  setActiveIssueTab(tab: string) {
    this.activeIssueTab = tab;
    this.loadDataByTab();
  }

  loadDataByTab() {
    if (this.activeIssueTab === 'pending') {
      this.loadPendingIssues();
    } else if (this.activeIssueTab === 'issued') {
      this.loadIssuedIssues();
    } else {
      this.loadReturnedIssues();
    }
  }

  loadPendingIssues() {
    this.issueService.getPendingIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.issueItems = res.data.map((x: PendingIssueItem) => ({
          estimationId: x.estimationItemId, 
          partName: x.partName,
          partNo: x.partNo,
          brand: x.brand,
          qtyOnHand: x.qtyOnHand,
          inStock: x.inStock,
          requestedQty: x.requestedQty,
          issuedQty: x.issuedQty,
          pendingQty: x.pendingQty,
          sellingPrice: x.sellingPrice,
          issueQty: x.issueQty ?? 0,
          issuedTo: x.issuedTo ?? ''
        }));

        this.filteredIssueItems = [...this.issueItems];
        this.buildCheckboxes();
      }
    });
  }

  loadIssuedIssues() {
    this.issueService.getIssuedIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.activeIssueTab = 'issued';
        this.issuedItems = res.data;
        this.filteredIssueItems = this.issuedItems;
        this.buildCheckboxes();
      }
    });
  }

  loadReturnedIssues() {
    this.issueService.getReturnedIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.issueItems = res.data;
        this.filteredIssueItems = [...this.issueItems];
        this.buildCheckboxes();
      }
    });
  }

  buildCheckboxes() {
    this.itemsFormArray.clear();
    this.issueForm.get('selectAll')?.setValue(false);

    this.filteredIssueItems.forEach(item => {
      this.itemsFormArray.push(
        this.fb.group({
          checked: [false],
          issueQty: [item.issueQty ?? 0],
          returnQty: [item.returnQty ?? 0],
          estimationId: [item.estimationId]   
        })
      );
    });
  }


  toggleSelectAll() {
    const checked = this.issueForm.get('selectAll')?.value;
    this.itemsFormArray.controls.forEach(c =>
      c.get('checked')?.setValue(checked)
    );
  }

  isAnyChecked(): boolean {
    return this.itemsFormArray.controls.some(
      c => c.get('checked')?.value
    );
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

    this.buildCheckboxes();
  }

  issueParts() {
    const selectedRows = this.itemsFormArray.controls
      .map(ctrl => {
        if (!ctrl.get('checked')?.value) return null;

        return {
          estimationId: ctrl.get('estimationId')!.value,
          issueQty: Number(ctrl.get('issueQty')!.value)
        };
      })
      .filter(
        (x): x is { estimationId: number; issueQty: number } =>
          x !== null && x.issueQty > 0
      );

    if (selectedRows.length === 0) {
      this.alert.showError('Please select items and enter Issue Qty(>1)');
      return;
    }

    this.issueService.issueParts({
      jobCardId: this.jobCardId,
      items: selectedRows
    }).subscribe({
      next: (res) => {
        this.alert.showSuccess(res.message || 'Items issued successfully');
        this.loadIssuedIssues();   // refresh issued tab
      },
      error: (err) => {
        console.error(err);

        // Backend sent meaningful message
        if (err.error?.message) {
          this.alert.showError(err.error.message);
        }
        // Validation / string error
        else if (typeof err.error === 'string') {
          this.alert.showError(err.error);
        }
        // Fallback
        else {
          this.alert.showError('Failed to issue parts. Please try again.');
        }
      }
    });

  }

}
