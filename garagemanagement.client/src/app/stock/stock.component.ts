import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormControl } from '@angular/forms';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { StockService, StockItem, StockStats } from '../services/stock.service';

@Component({
  selector: 'app-stock',
  templateUrl: './stock.component.html',
  styleUrls: ['./stock.component.css'],
  standalone: false
})
export class StockComponent implements OnInit, OnDestroy {

  // 🔹 Stats & Stock Data
  stockStats!: StockStats;
  filteredItems: StockItem[] = [];

  // 🔹 UI State
  isLoading = false;
  hasError = false;

  // 🔹 Tabs
  tabs = ['stock', 'order', 'inward', 'issued', 'purchaseReturn', 'stockAlert'];
  activeTab = 'stock';

  // 🔹 Filter Form
  searchForm!: FormGroup;

  // 🔹 Unsubscribe
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private stockService: StockService
  ) { }

  ngOnInit(): void {
    this.initForm();
    this.loadStockStats();
    this.listenToFilters();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // 🔹 Initialize Filter Form
  private initForm(): void {
    this.searchForm = this.fb.group({
      stockType: ['IN'],   // default In Stock
      search: ['']
    });

    this.loadStockList();
  }
  // 🔹 Form Getters
  get stockTypeControl(): FormControl {
    return this.searchForm.get('stockType') as FormControl;
  }

  get searchControl(): FormControl {
    return this.searchForm.get('search') as FormControl;
  }

  // 🔹 Listen to Filter Changes
  private listenToFilters(): void {
    this.searchForm.valueChanges
      .pipe(debounceTime(300), takeUntil(this.destroy$))
      .subscribe(() => this.loadStockList());
  }

  // 🔹 Load Stock List from API
  private loadStockList(): void {
    const { stockType, search } = this.searchForm.value;

    this.isLoading = true;
    this.hasError = false;

    this.stockService.getStockList(stockType, search)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.filteredItems = data;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
          this.hasError = true;
        }
      });
  }


  // 🔹 Load Stock Statistics
  private loadStockStats(): void {
    this.stockService.getStockStats()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: stats => this.stockStats = stats,
        error: () => { /* optionally handle error */ }
      });
  }

  // 🔹 Tab Navigation
  setActiveTab(tab: string): void {
    this.activeTab = tab;
  }

  // 🔹 Export Feature (Placeholder)
  exportStock(): void {
    alert('Export will be implemented (Excel / PDF)');
  }
}
