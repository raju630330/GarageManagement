import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { IssueService } from '../services/issue.service';
import { AlertService } from '../services/alert.service';
import { Router } from '@angular/router';
interface IssueItemDto {
  estimationItemId: number;
  issueQty: number;
}
interface ReturnItemDto {
  estimationItemId: number;
  returnQty: number;
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
  filteredIssueItems: any[] = [];
  issueItems: any[] = [];

  jobCardId!: number;

  constructor(
    private fb: FormBuilder,
    private issueService: IssueService,
    private route: ActivatedRoute,
    private alert: AlertService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.issueForm = this.fb.group({
      selectAll: [false],
      items: this.fb.array([])
    });

    this.route.queryParamMap.subscribe(p => {
      const id = p.get('jobCardId');
      if (id) {
        this.jobCardId = +id;
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
    if (this.activeIssueTab === 'pending') this.loadPendingIssues();
    else if (this.activeIssueTab === 'issued') this.loadIssuedIssues();
    else this.loadReturnedIssues();
  }

  loadPendingIssues() {
    this.issueService.getPendingIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.issueItems = res.data;
        this.filteredIssueItems = [...this.issueItems];
        console.log(this.filteredIssueItems);
        this.buildForm();
      }
    });
  }

  loadIssuedIssues() {
    this.issueService.getIssuedIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.issueItems = res.data;
        this.filteredIssueItems = [...this.issueItems];
        this.buildForm();
      }
    });
  }

  loadReturnedIssues() {
    this.issueService.getReturnedIssues(this.jobCardId).subscribe(res => {
      if (res.isSuccess) {
        this.activeIssueTab === 'returned'
        this.issueItems = res.data;
        this.filteredIssueItems = [...this.issueItems];
        this.buildForm();
      }
    });
  }

  buildForm() {
    this.itemsFormArray.clear();
    this.issueForm.get('selectAll')?.setValue(false);

    this.filteredIssueItems.forEach(item => {
      this.itemsFormArray.push(
        this.fb.group({
          checked: [false],
          issueQty: [item.issueQty || 0],
          returnQty: [0],
          estimationItemId: [item.estimationItemId]
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
    return this.itemsFormArray.controls.some(c => c.get('checked')?.value);
  }

  applyFilters() {
    const search = (this.searchControl.value || '').toLowerCase();
    const qty = Number(this.qtyControl.value);

    this.filteredIssueItems = this.issueItems.filter(i =>
      (i.partName.toLowerCase().includes(search) ||
        i.partNo.toLowerCase().includes(search)) &&
      (!qty || (i.pendingQty ?? 0) >= qty)
    );

    this.buildForm();
  }

  issueParts() {
    const selectedRows: IssueItemDto[] = this.itemsFormArray.controls
      .filter(ctrl => ctrl.get('checked')?.value)
      .map(ctrl => {
        const estimationItemId = ctrl.get('estimationItemId')?.value;
        const issueQty = Number(ctrl.get('issueQty')?.value);

        if (!estimationItemId || issueQty <= 0) return null;

        return {
          estimationItemId,
          issueQty
        };
      })
      .filter((x): x is IssueItemDto => x !== null);

    if (selectedRows.length === 0) {
      this.alert.showError('Please select items and enter Issue Qty (> 0)');
      return;
    }

    this.issueService.issueParts({
      jobCardId: this.jobCardId,
      items: selectedRows
    }).subscribe({
      next: res => {
        this.activeIssueTab = 'issued'
        this.alert.showSuccess(res.message || 'Items issued successfully');
        this.loadIssuedIssues();
      },
      error: err => {
        this.alert.showError(err.error?.message || 'Failed to issue parts');
      }
    });
  }

  returnParts() {

    const selectedRows: ReturnItemDto[] = this.itemsFormArray.controls
      .filter(ctrl => ctrl.get('checked')?.value)
      .map(ctrl => {
        const estimationItemId = ctrl.get('estimationItemId')?.value;
        const returnQty = Number(ctrl.get('returnQty')?.value);

        if (!estimationItemId || returnQty <= 0) return null;

        return {
          estimationItemId,
          returnQty
        };
      })
      .filter((x): x is ReturnItemDto => x !== null);

    if (selectedRows.length === 0) {
      this.alert.showError('Please select items and enter Return Qty (> 0)');
      return;
    }
    this.issueService.returnParts({
      jobCardId: this.jobCardId,
      items : selectedRows
    }).subscribe({
      next: res => {
        this.activeIssueTab = 'returned'
        this.alert.showSuccess(res.message || 'Items returned successfully');
        this.loadReturnedIssues();
      },
      error: err => {
        this.alert.showError(err.error?.message || 'Failed to return parts');
      }
    });
  }

  goBack() {
    if (!this.jobCardId) {
      this.alert.showError('Job Card Id missing');
      return;
    }

    this.router.navigate(['/estimate', this.jobCardId]);
  }

}
