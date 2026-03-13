import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import {
  StockService,
  StockItem,
  StockStats,
  PurchaseOrderListItem,
  PurchaseOrderDetail,
  PurchaseOrderFilters
} from '../services/stock.service';

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css'],
  standalone: false
})
export class StockComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();

  // ── Stats & Stock ──────────────────────────────────────────────────────────
  stockStats!: StockStats;
  filteredItems: StockItem[] = [];

  // ── UI State ───────────────────────────────────────────────────────────────
  isLoading = false;
  hasError = false;

  // ── Tabs ───────────────────────────────────────────────────────────────────
  tabs = ['stock', 'order', 'inward', 'issued', 'purchaseReturn', 'stockAlert'];
  activeTab = 'stock';

  // ── Stock filter form ──────────────────────────────────────────────────────
  searchForm!: FormGroup;

  // ── Stock pagination ───────────────────────────────────────────────────────
  currentPage = 1;
  pageSize = 10;
  totalRecords = 0;
  pageSizes = [10, 25, 50];

  // ── Order tab state ────────────────────────────────────────────────────────
  orders: PurchaseOrderListItem[] = [];
  orderLoading = false;
  orderHasError = false;

  orderSearchControl = new FormControl('');
  orderStatusControl = new FormControl('All');
  orderFromDateControl = new FormControl('');
  orderToDateControl = new FormControl('');

  orderCurrentPage = 1;
  orderPageSize = 10;
  orderTotalRecords = 0;
  orderPageSizes = [10, 25, 50];

  // FIX: must match backend status values exactly
  readonly orderStatuses = ['All', 'PENDING', 'SHIPMENT', 'CLOSED', 'CANCELLED'];

  // Detail flyout
  selectedOrder: PurchaseOrderDetail | null = null;
  detailLoading = false;
  showDetailPanel = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private stockService: StockService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadStockStats();
    this.listenToFilters();
    this.listenToOrderFilters();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // ── Stock tab ──────────────────────────────────────────────────────────────

  private initForm(): void {
    this.searchForm = this.fb.group({ stockType: ['IN'], search: [''] });
    this.loadStockList();
  }

  get stockTypeControl(): FormControl { return this.searchForm.get('stockType') as FormControl; }
  get searchControl(): FormControl { return this.searchForm.get('search') as FormControl; }

  private listenToFilters(): void {
    this.searchForm.valueChanges
      .pipe(debounceTime(300), takeUntil(this.destroy$))
      .subscribe(() => { this.currentPage = 1; this.loadStockList(); });
  }

  private loadStockList(): void {
    const { stockType, search } = this.searchForm.value;
    this.isLoading = true;
    this.stockService
      .getStockList(stockType, search, this.currentPage, this.pageSize)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => { this.filteredItems = res.items ?? []; this.totalRecords = res.totalRecords; this.isLoading = false; },
        error: () => { this.isLoading = false; this.hasError = true; }
      });
  }

  private loadStockStats(): void {
    this.stockService.getStockStats()
      .pipe(takeUntil(this.destroy$))
      .subscribe({ next: stats => (this.stockStats = stats) });
  }

  exportStock(): void { alert('Export will be implemented (Excel / PDF)'); }

  get totalPages(): number { return Math.ceil(this.totalRecords / this.pageSize); }
  get pages(): number[] { return Array.from({ length: this.totalPages }, (_, i) => i + 1); }

  changePage(page: number): void {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.loadStockList();
  }

  changePageSize(size: number): void {
    this.pageSize = Number(size); this.currentPage = 1; this.loadStockList();
  }

  // ── Tab navigation ─────────────────────────────────────────────────────────

  setActiveTab(tab: string): void {
    this.activeTab = tab;
    // FIX: added !this.orderLoading guard to prevent double-firing if the user
    // clicks the tab again while a request is already in flight.
    if (tab === 'order' && this.orders.length === 0 && !this.orderLoading) {
      this.loadOrders();
    }
  }

  // ── Order tab ──────────────────────────────────────────────────────────────

  private listenToOrderFilters(): void {
    this.orderSearchControl.valueChanges
      .pipe(debounceTime(300), takeUntil(this.destroy$))
      .subscribe(() => this.resetAndLoadOrders());

    this.orderStatusControl.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.resetAndLoadOrders());

    this.orderFromDateControl.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.resetAndLoadOrders());

    this.orderToDateControl.valueChanges
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => this.resetAndLoadOrders());
  }

  private resetAndLoadOrders(): void {
    this.orderCurrentPage = 1;
    this.loadOrders();
  }

  loadOrders(): void {
    this.orderLoading = true;
    this.orderHasError = false;

    const filters: PurchaseOrderFilters = {
      search: this.orderSearchControl.value ?? '',
      status: this.orderStatusControl.value ?? 'All',
      fromDate: this.orderFromDateControl.value ?? '',
      toDate: this.orderToDateControl.value ?? '',
      page: this.orderCurrentPage,
      pageSize: this.orderPageSize
    };

    this.stockService.getPurchaseOrders(filters)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => {
          this.orders = res.items ?? [];
          this.orderTotalRecords = res.totalRecords;
          this.orderLoading = false;
        },
        error: () => { this.orderLoading = false; this.orderHasError = true; }
      });
  }

  get orderTotalPages(): number { return Math.ceil(this.orderTotalRecords / this.orderPageSize); }
  get orderPages(): number[] { return Array.from({ length: this.orderTotalPages }, (_, i) => i + 1); }

  changeOrderPage(page: number): void {
    if (page < 1 || page > this.orderTotalPages) return;
    this.orderCurrentPage = page;
    this.loadOrders();
  }

  changeOrderPageSize(size: number): void {
    this.orderPageSize = Number(size); this.orderCurrentPage = 1; this.loadOrders();
  }

  createNewOrder(): void {
    this.router.navigate(['/order'], { queryParams: { source: 'stock' } });
  }

  openOrderDetail(orderId: number): void {
    this.showDetailPanel = true;
    this.detailLoading = true;
    this.selectedOrder = null;

    this.stockService.getPurchaseOrderById(orderId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: detail => {
          // Guard: API may return items as null/undefined if the order has no lines
          detail.items = detail.items ?? [];
          this.selectedOrder = detail;
          this.detailLoading = false;
        },
        error: () => { this.detailLoading = false; }
      });
  }

  closeDetailPanel(): void { this.showDetailPanel = false; this.selectedOrder = null; }

  updateOrderStatus(orderId: number, status: string): void {
    this.stockService.updatePurchaseOrderStatus(orderId, status)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          const row = this.orders.find(o => o.orderId === orderId);
          if (row) row.status = status;
          if (this.selectedOrder) this.selectedOrder.status = status;
        },
        error: () => alert('Failed to update status. Please try again.')
      });
  }

  /**
   * Bootstrap badge class per status.
   * FIX: was checking 'Received'/'Cancelled' — corrected to backend values.
   *
   * PENDING   → yellow  (warning)
   * SHIPMENT  → blue    (info)
   * CLOSED    → green   (success)
   * CANCELLED → red     (danger)
   */
  statusBadgeClass(status: string): string {
    switch (status) {
      case 'CLOSED': return 'bg-success';
      case 'SHIPMENT': return 'bg-info text-dark';
      case 'CANCELLED': return 'bg-danger';
      default: return 'bg-warning text-dark';  // PENDING
    }
  }

  /**
   * Derive a btn- class from the badge class for the status action buttons.
   * Keeps the template clean — no inline .replace() calls in the HTML.
   */
  statusBtnClass(status: string): string {
    switch (status) {
      case 'CLOSED': return 'btn-success';
      case 'SHIPMENT': return 'btn-info';
      case 'CANCELLED': return 'btn-danger';
      default: return 'btn-warning';  // PENDING
    }
  }

  /** Grand total for the detail panel — replaces the unregistered sumBy pipe. */
  getDetailTotal(): number {
    if (!this.selectedOrder?.items?.length) return 0;
    return +this.selectedOrder.items
      .reduce((sum, i) => sum + (isNaN(+i.totalPurchasePrice) ? 0 : +i.totalPurchasePrice), 0)
      .toFixed(2);
  }
}
