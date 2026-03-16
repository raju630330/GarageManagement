import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { StockService } from '../services/stock.service';
import { JobCardService } from '../services/job-card.service';

export interface Supplier {
  supplierId: number;
  name: string;
  contactPerson: string;
  phone: string;
}

// ─────────────────────────────────────────────────────────────────────────────
// TWO WAYS THIS PAGE OPENS  (document pages 65-66)
//
// WAY 1 — From Job Card → Parts icon → Inward
//   URL: /inward?jobCardId=42&regNo=TS07UK7651&jobCardNo=SRT-J003038
//   → Header shows Reg No + JC No, Stock checkbox OFF (greyed)
//   → jobCardId saved to DB ✅
//
// WAY 2 — Directly from Parts module → Inward option  (no URL params)
//   → Header shows empty search field for "Reg No / Job Card No"
//   → User either:
//       a) Types job card no → search results → selects → jobCardId populated ✅
//       b) Ticks "Stock" checkbox → jobCardId = null, regNo = STOCK  ✅ expected
// ─────────────────────────────────────────────────────────────────────────────

@Component({
  selector: 'app-inward',
  templateUrl: './inward.component.html',
  styleUrl: './inward.component.css',
  standalone: false
})
export class InwardComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();
  private baseUrl = environment.apiUrl;

  todayDate = new Date();
  inwardForm!: FormGroup;
  suppliers: Supplier[] = [];
  submitting = false;
  successMessage = '';

  // ── Context ───────────────────────────────────────────────────────────────
  jobCardId: number | null = null;
  regNo = '';
  jobCardNo = '';
  source = '';

  // true  → Stock checkbox ticked, jobCardId intentionally null
  // false → Either came from Job Card (jobCardId set) OR user searching manually
  isStockMode = false;

  // ── Manual Job Card search (uses app-autocomplete + JobCardService) ─────────
  jobCardLinked = false;   // true once user picks from autocomplete OR came via URL

  // ── Part autocomplete ──────────────────────────────────────────────────────
  private _selectedPart: any = null;
  loadingPart = false;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    public stockService: StockService,
    public jobCardService: JobCardService
  ) { }

  ngOnInit(): void {
    this.buildForm();
    this.loadSuppliers();
    this.readQueryParams();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // ─── Form ─────────────────────────────────────────────────────────────────
  private buildForm(): void {
    this.inwardForm = this.fb.group({
      deliveryReceipt: [''],
      billNo: ['', Validators.required],
      billDate: ['', Validators.required],
      taxType: ['GST', Validators.required],
      supplierId: [null, Validators.required],

      addItemForm: this.fb.group({
        search: [null],
        inwardQty: [null, [Validators.required, Validators.min(0.01)]],
        unitPrice: [null, [Validators.required, Validators.min(0.01)]],
        discount: [null, [Validators.min(0)]],
        sellingPrice: [null, [Validators.required, Validators.min(0.01)]],
        rackNo: [''],
        barcode: [''],
        remarks: ['']
      }),

      freight: [0, Validators.min(0)],
      tcs: [0, Validators.min(0)],
      paid: [0, Validators.min(0)],
      items: this.fb.array([], Validators.minLength(1))
    });
  }

  // ─── Query params ─────────────────────────────────────────────────────────
  private readQueryParams(): void {
    this.route.queryParams.pipe(takeUntil(this.destroy$)).subscribe(p => {
      this.source = p['source'] || '';
      this.jobCardId = p['jobCardId'] ? +p['jobCardId'] : null;
      this.regNo = p['regNo'] || '';
      this.jobCardNo = p['jobCardNo'] || '';

      // Came from Job Card context → jobCardId already set, no manual search needed
      if (this.jobCardId) {
        this.jobCardLinked = true;
        this.isStockMode = false;
      } else {
        // Direct/standalone open → show manual search + Stock checkbox
        this.jobCardLinked = false;
        this.isStockMode = false;   // user must choose: link JC or tick Stock
      }
    });
  }

  // ─── Stock checkbox (document page 66) ───────────────────────────────────
  onStockCheckboxChange(checked: boolean): void {
    this.isStockMode = checked;
    if (checked) {
      // Clear job card — this is a direct stock inward, jobCardId = null is correct
      this.jobCardId = null;
      this.regNo = 'STOCK';
      this.jobCardNo = '';
      this.jobCardLinked = false;
    } else {
      this.regNo = '';
    }
  }

  // ─── Job Card autocomplete (app-autocomplete + JobCardService) ───────────
  // Called by (selected) output of app-autocomplete
  // The autocomplete item carries the full job card detail as structured data
  // Angular template:
  //   [searchFn]="jobCardService.searchJobCardDetailsForEstimation.bind(jobCardService)"
  //   (selected)="onSelectedJobCard($event)"
  //   (clearForm)="resetJobCardForm()"
  onSelectedJobCard(item: any): void {
    if (!item) return;

    // app-autocomplete returns the item selected from the dropdown.
    // JobCardService.searchJobCardDetailsForEstimation returns IdNameDto objects
    // where .id = jobCardId and .name = "RegNo | JobCardNo | CustomerName"
    // Parse the name to extract regNo and jobCardNo
    const parts = (item.name || '').split('|').map((s: string) => s.trim());

    this.jobCardId = item.id;
    this.regNo = parts[0] || '';
    this.jobCardNo = parts[1] || '';
    this.jobCardLinked = true;
  }

  // Called by (clearForm) output of app-autocomplete
  resetJobCardForm(): void {
    this.jobCardId = null;
    this.regNo = '';
    this.jobCardNo = '';
    this.jobCardLinked = false;
  }

  // ─── Suppliers ────────────────────────────────────────────────────────────
  private loadSuppliers(): void {
    this.http.get<Supplier[]>(`${this.baseUrl}/supplier`)
      .pipe(takeUntil(this.destroy$))
      .subscribe({ next: res => this.suppliers = res || [], error: () => { } });
  }

  // ─── Part autocomplete selected ───────────────────────────────────────────
  onSelectedPart(autoItem: any): void {
    if (!autoItem) return;
    const partId = autoItem.id;
    if (this.items.controls.some(c => c.value.partId === partId)) {
      alert('This part is already added.'); return;
    }
    this._selectedPart = null;
    this.loadingPart = true;
    this.http.get<any>(`${this.baseUrl}/stock/get/${partId}`)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: p => {
          this.loadingPart = false;
          if (!p) { alert('Part details could not be loaded.'); return; }
          p.partId = p.partId ?? p.id ?? partId;
          if (!p.partName || !p.partNo) {
            const seg = (autoItem.name || '').split('|').map((s: string) => s.trim());
            p.partName = p.partName || seg[0] || '';
            p.partNo = p.partNo || seg[1] || '';
          }
          this._selectedPart = p;
          this.inwardForm.get('addItemForm')!.patchValue({
            unitPrice: p.purchasePrice ?? null,
            sellingPrice: p.sellingPrice ?? null
          });
        },
        error: () => { this.loadingPart = false; alert('Could not load part details.'); }
      });
  }

  resetAddItemForm(): void {
    this.inwardForm.get('addItemForm')!.reset({
      search: null, inwardQty: null, unitPrice: null,
      discount: null, sellingPrice: null, rackNo: '', barcode: '', remarks: ''
    });
    this._selectedPart = null;
    this.loadingPart = false;
  }

  // ─── Add row ──────────────────────────────────────────────────────────────
  addItem(): void {
    const f = this.inwardForm.get('addItemForm') as FormGroup;
    f.markAllAsTouched();
    if (!this._selectedPart) {
      alert(this.loadingPart ? 'Part still loading…' : 'Search and select a part first.');
      return;
    }
    if (f.invalid) return;
    const qty = +f.value.inwardQty;
    const price = +f.value.unitPrice;
    const disc = f.value.discount != null ? +f.value.discount : 0;
    const selling = +f.value.sellingPrice;
    if (disc >= qty * price) { alert('Discount must be less than Qty × Unit Price.'); return; }
    if (selling <= 0) { alert('Selling price must be > 0.'); return; }
    this.pushItemRow(this._selectedPart, qty, price, disc, selling,
      f.value.rackNo || '', f.value.barcode || '', f.value.remarks || '');
    this.resetAddItemForm();
  }

  private pushItemRow(part: any, qty: number, price: number, disc: number,
    selling: number, rackNo = '', barcode = '', remarks = ''): void {
    const taxPct = part.taxPercent || 0;
    const safeD = isNaN(disc) ? 0 : disc;
    const base = Math.max(0, qty * price - safeD);
    const taxAmt = +(base * taxPct / 100).toFixed(2);
    const total = +(base + taxAmt).toFixed(2);
    const margin = +(selling - price).toFixed(2);

    this.items.push(this.fb.group({
      partId: [part.partId, Validators.required],
      partName: [part.partName ?? ''],
      partNo: [part.partNo ?? ''],
      brand: [part.brand ?? ''],
      hsnCode: [part.hsnCode ?? ''],
      taxPercent: [taxPct],
      inwardQty: [qty, [Validators.required, Validators.min(0.01)]],
      unitPrice: [price, [Validators.required, Validators.min(0.01)]],
      discount: [safeD],
      taxAmount: [taxAmt],
      totalPurchasePrice: [total],
      sellingPrice: [selling, [Validators.required, Validators.min(0.01)]],
      margin: [margin],
      rackNo: [rackNo],
      barcode: [barcode],
      remarks: [remarks]
    }));
  }

  recalcRow(i: number): void {
    const r = this.items.at(i);
    const qty = +r.value.inwardQty || 0;
    const price = +r.value.unitPrice || 0;
    const disc = +r.value.discount || 0;
    const tax = +r.value.taxPercent || 0;
    const sell = +r.value.sellingPrice || 0;
    const base = Math.max(0, qty * price - disc);
    const tAmt = +(base * tax / 100).toFixed(2);
    r.patchValue({
      taxAmount: tAmt, totalPurchasePrice: +(base + tAmt).toFixed(2),
      margin: +(sell - price).toFixed(2)
    }, { emitEvent: false });
  }

  removeRow(i: number): void { this.items.removeAt(i); }
  get items(): FormArray { return this.inwardForm.get('items') as FormArray; }

  // ─── Totals ───────────────────────────────────────────────────────────────
  get subTotal(): number {
    return +this.items.controls.reduce((s, c) => s + (+c.value.totalPurchasePrice || 0), 0).toFixed(2);
  }
  get grandTotal(): number {
    return +(this.subTotal + (+this.inwardForm.value.freight || 0) + (+this.inwardForm.value.tcs || 0)).toFixed(2);
  }
  get balance(): number {
    return +(this.grandTotal - (+this.inwardForm.value.paid || 0)).toFixed(2);
  }

  // ─── Submit ───────────────────────────────────────────────────────────────
  submitInward(): void {
    ['billNo', 'billDate', 'taxType', 'supplierId'].forEach(f => this.inwardForm.get(f)?.markAsTouched());

    if (this.inwardForm.get('billNo')?.invalid || this.inwardForm.get('billDate')?.invalid ||
      this.inwardForm.get('taxType')?.invalid || this.inwardForm.get('supplierId')?.invalid) {
      alert('Please fill in Bill No, Bill Date, Tax Type and Vendor.'); return;
    }
    if (this.items.length === 0) { alert('Add at least one part.'); return; }

    // Validation: must either have a job card linked OR stock mode ticked
    if (!this.isStockMode && !this.jobCardId) {
      alert('Please search and select a Job Card, or tick the "Stock" checkbox for a direct stock inward.');
      return;
    }

    this.submitting = true;
    const v = this.inwardForm.value;

    const payload = {
      jobCardId: this.jobCardId,           // null for stock inward ✅ expected
      regNo: this.regNo || 'STOCK',
      jobCardNo: this.jobCardNo || '',
      deliveryReceipt: v.deliveryReceipt || '',
      billNo: v.billNo,
      billDate: v.billDate,
      taxType: v.taxType,
      supplierId: +v.supplierId,
      freightAmount: +v.freight || 0,
      tcsAmount: +v.tcs || 0,
      paidAmount: +v.paid || 0,
      items: this.items.controls.map(c => ({
        partId: c.value.partId,
        inwardQty: c.value.inwardQty,
        unitPrice: c.value.unitPrice,
        discount: c.value.discount,
        taxPercent: c.value.taxPercent,
        taxAmount: c.value.taxAmount,
        totalPurchasePrice: c.value.totalPurchasePrice,
        sellingPrice: c.value.sellingPrice,
        rackNo: c.value.rackNo || '',
        barcode: c.value.barcode || '',
        remarks: c.value.remarks || ''
      }))
    };

    this.http.post<any>(`${this.baseUrl}/inward`, payload)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => {
          this.submitting = false;
          if (!res?.isSuccess) { alert(res?.message || 'Inward failed.'); return; }
          this.successMessage = `Inward ${res.inwardNo} created successfully!`;
          this.items.clear();
          this.inwardForm.patchValue({
            deliveryReceipt: '', billNo: '', billDate: '',
            taxType: 'GST', supplierId: null, freight: 0, tcs: 0, paid: 0
          });
          setTimeout(() => this.goBack(), 1500);
        },
        error: err => { this.submitting = false; alert(err?.error?.message || 'Server error.'); }
      });
  }

  goBack(): void { this.router.navigate(['/stock']); }

  goToOrderParts(): void {
    this.router.navigate(['/order'], {
      queryParams: { source: 'inward', jobCardId: this.jobCardId, regNo: this.regNo, jobCardNo: this.jobCardNo }
    });
  }
}
