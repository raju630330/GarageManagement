import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { StockService } from '../services/stock.service';

export interface Supplier {
  supplierId: number;
  name: string;
  contactPerson: string;
  phone: string;
}

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrl: './order.component.css',
  standalone: false
})
export class OrderComponent implements OnInit, OnDestroy {

  private destroy$ = new Subject<void>();
  private baseUrl = environment.apiUrl;

  todayDate: Date = new Date();
  orderForm!: FormGroup;
  suppliers: Supplier[] = [];
  submitting = false;
  successMessage = '';

  jobCardId: number | null = null;
  regNo = '';
  jobCardNo = '';
  source = '';

  // Holds the full part detail fetched from the API after autocomplete selection.
  // null while a fetch is in progress or nothing is selected.
  private _selectedPart: any = null;
  loadingPart = false;   // used in template to disable Add Item while fetching

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    public stockService: StockService
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

  private buildForm(): void {
    this.orderForm = this.fb.group({
      supplierId: [null, Validators.required],
      paymentType: ['', Validators.required],
      stockType: [''],
      remarks: ['', Validators.maxLength(200)],

      addItemForm: this.fb.group({
        search: [null, Validators.required],
        qty: [null, [Validators.required, Validators.min(1), Validators.max(99999)]],
        unitPrice: [null, [Validators.required, Validators.min(0.01)]],
        discount: [null, [Validators.min(0)]],
        serviceType: ['Part', Validators.required]
      }),

      items: this.fb.array([], Validators.minLength(1))
    });
  }

  private readQueryParams(): void {
    this.route.queryParams
      .pipe(takeUntil(this.destroy$))
      .subscribe(p => {
        this.source = p['source'] || '';
        this.jobCardId = p['jobCardId'] ? +p['jobCardId'] : null;
        this.regNo = p['regNo'] || '';
        this.jobCardNo = p['jobCardNo'] || '';

        if (p['partIds']) {
          const ids: number[] = p['partIds']
            .split(',')
            .map(Number)
            .filter(Boolean);
          ids.forEach(id => this.preloadPartById(id));
        }
      });
  }

  // ─── Autocomplete selected ────────────────────────────────────────────────
  // The autocomplete emits a flat object:  { id: 11, name: "Engine Mount | ENG-011 | Qty: 6.00" }
  // We fetch the full stock record by id to get purchasePrice, taxPercent, brand, hsnCode, etc.
  onSelectedPart(autoItem: any): void {
    if (!autoItem) return;

    const partId: number = autoItem.id;

    // Duplicate check using the id that is already available
    if (this.items.controls.some(c => c.value.partId === partId)) {
      alert('This part is already added.');
      return;
    }

    this._selectedPart = null;
    this.loadingPart = true;

    this.http.get<any>(`${this.baseUrl}/stock/get/${partId}`)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: fullPart => {
          this.loadingPart = false;

          if (!fullPart) {
            alert('Part details could not be loaded.');
            return;
          }

          // Normalise: the API might return partId or id
          fullPart.partId = fullPart.partId ?? fullPart.id ?? partId;

          // If the API doesn't return partName / partNo, fall back to parsing the
          // autocomplete label:  "Engine Mount | ENG-011 | Qty: 6.00"
          if (!fullPart.partName || !fullPart.partNo) {
            const parsed = this.parseAutoLabel(autoItem.name);
            fullPart.partName = fullPart.partName || parsed.partName;
            fullPart.partNo = fullPart.partNo || parsed.partNo;
          }

          this._selectedPart = fullPart;

          // Pre-fill purchase price so the user can see / override it
          this.orderForm.get('addItemForm')!.patchValue({
            unitPrice: fullPart.purchasePrice ?? null
          });
        },
        error: () => {
          this.loadingPart = false;
          alert('Could not load part details. Please try again.');
        }
      });
  }

  // Parse "Engine Mount | ENG-011 | Qty: 6.00" → { partName, partNo }
  private parseAutoLabel(label: string): { partName: string; partNo: string } {
    const segments = (label || '').split('|').map(s => s.trim());
    const partName = segments[0] || '';
    // Second segment is the part number; strip any leading/trailing whitespace
    const partNo = segments[1] || '';
    return { partName, partNo };
  }

  // ─── Preload from query params ────────────────────────────────────────────
  private preloadPartById(partId: number): void {
    this.http.get<any>(`${this.baseUrl}/stock/get/${partId}`)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: part => {
          if (!part) return;
          part.partId = part.partId ?? part.id ?? partId;
          this.pushItemRow(part, 1, part.purchasePrice || 0, 0, 'Part');
        },
        error: () => {
          alert(`Could not load part #${partId}. It may have been removed.`);
        }
      });
  }

  private loadSuppliers(): void {
    this.http.get<Supplier[]>(`${this.baseUrl}/supplier`)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => this.suppliers = res || [],
        error: () => this.suppliers = []
      });
  }

  resetAddItemForm(): void {
    this.orderForm.get('addItemForm')!.reset({
      search: null,
      qty: null,
      unitPrice: null,
      discount: null,
      serviceType: 'Part'
    });
    this._selectedPart = null;
    this.loadingPart = false;
  }

  // ─── Add item to table ────────────────────────────────────────────────────
  addItem(): void {
    const f = this.orderForm.get('addItemForm') as FormGroup;
    f.markAllAsTouched();

    if (!this._selectedPart) {
      alert(this.loadingPart
        ? 'Part details are still loading. Please wait a moment.'
        : 'Please search and select a part first.');
      return;
    }

    if (f.invalid) {
      return;   // inline validation messages in the template handle feedback
    }

    const qty = +f.value.qty;
    const unitPrice = +f.value.unitPrice;
    const discount = f.value.discount != null ? +f.value.discount : 0;

    if (discount >= qty * unitPrice) {
      alert('Discount must be less than the item total (Qty × Unit Price).');
      return;
    }

    this.pushItemRow(this._selectedPart, qty, unitPrice, discount, f.value.serviceType);
    this.resetAddItemForm();
  }

  // ─── Build a FormGroup row from a full part record ────────────────────────
  private pushItemRow(
    part: any,
    qty: number,
    unitPrice: number,
    discount: number,
    serviceType: string
  ): void {
    const taxPercent = part.taxPercent || 0;
    const safeDiscount = isNaN(discount) ? 0 : discount;
    const baseAmount = Math.max(0, qty * unitPrice - safeDiscount);
    const taxAmount = +(baseAmount * taxPercent / 100).toFixed(2);
    const total = +(baseAmount + taxAmount).toFixed(2);

    this.items.push(this.fb.group({
      partId: [part.partId, Validators.required],
      partName: [part.partName ?? ''],
      partNo: [part.partNo ?? ''],
      brand: [part.brand ?? ''],
      hsnCode: [part.hsnCode ?? ''],
      taxPercent: [taxPercent],
      qty: [qty, [Validators.required, Validators.min(1)]],
      unitPrice: [unitPrice, [Validators.required, Validators.min(0.01)]],
      discount: [safeDiscount],
      taxAmount: [taxAmount],
      totalPurchasePrice: [total],
      serviceType: [serviceType],
      remarks: [''],
      sellerInfo: ['']
    }));
  }

  recalcRow(i: number): void {
    const row = this.items.at(i);
    const qty = +row.value.qty || 0;
    const price = +row.value.unitPrice || 0;
    const disc = +row.value.discount || 0;
    const taxPct = +row.value.taxPercent || 0;

    const base = Math.max(0, qty * price - disc);
    const taxAmt = +(base * taxPct / 100).toFixed(2);
    const total = +(base + taxAmt).toFixed(2);

    row.patchValue({ taxAmount: taxAmt, totalPurchasePrice: total }, { emitEvent: false });
  }

  removeRow(i: number): void {
    this.items.removeAt(i);
  }

  get items(): FormArray {
    return this.orderForm.get('items') as FormArray;
  }

  get grandTotal(): number {
    return +this.items.controls
      .reduce((s, c) => s + (isNaN(+c.value.totalPurchasePrice) ? 0 : +c.value.totalPurchasePrice), 0)
      .toFixed(2);
  }

  get totalTaxAmount(): number {
    return +this.items.controls
      .reduce((s, c) => s + (isNaN(+c.value.taxAmount) ? 0 : +c.value.taxAmount), 0)
      .toFixed(2);
  }

  get totalDiscountAmount(): number {
    return +this.items.controls
      .reduce((s, c) => s + (isNaN(+c.value.discount) ? 0 : +c.value.discount), 0)
      .toFixed(2);
  }

  submitOrder(): void {
    // Touch only the order-level controls — NOT addItemForm.
    // addItemForm is a transient input widget; after the user clicks "Add Item"
    // the row appears in the table and addItemForm is reset to null/empty.
    // markAllAsTouched() on the whole form would touch addItemForm.search
    // (null after reset) and make orderForm.invalid = true, falsely
    // blocking submit even when items are present.
    this.orderForm.get('supplierId')?.markAsTouched();
    this.orderForm.get('paymentType')?.markAsTouched();

    const orderInvalid =
      this.orderForm.get('supplierId')?.invalid ||
      this.orderForm.get('paymentType')?.invalid;

    if (orderInvalid) {
      alert('Please select a Vendor and Payment Type before submitting.');
      return;
    }

    if (this.items.length === 0) {
      alert('Add at least one part before submitting.');
      return;
    }

    this.submitting = true;

    const payload = {
      supplierId: +this.orderForm.value.supplierId,
      paymentType: this.orderForm.value.paymentType,
      stockType: this.orderForm.value.stockType || '',
      remarks: this.orderForm.value.remarks || '',
      jobCardId: this.jobCardId,
      regNo: this.regNo || 'STOCK',
      jobCardNo: this.jobCardNo || '',
      source: this.source,
      items: this.items.controls.map(c => ({
        partId: c.value.partId,
        qty: c.value.qty,
        unitPrice: c.value.unitPrice,
        discount: c.value.discount,
        serviceType: c.value.serviceType,
        remarks: c.value.remarks || '',
        sellerInfo: c.value.sellerInfo || ''
      }))
    };

    this.http.post<any>(`${this.baseUrl}/purchaseorder`, payload)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: res => {
          this.submitting = false;
          if (!res?.isSuccess) {
            alert(res?.message || 'Order failed.');
            return;
          }
          this.successMessage = `Order ${res.orderNo} created successfully!`;
          this.items.clear();
          this.orderForm.patchValue({
            supplierId: null,
            paymentType: '',
            stockType: '',
            remarks: ''
          });
          setTimeout(() => this.goBack(), 1500);
        },
        error: err => {
          this.submitting = false;
          alert(err?.error?.message || 'Server error while creating order.');
        }
      });
  }

  goBack(): void {
    switch (this.source) {
      case 'estimation': this.router.navigate(['/estimate', this.jobCardId]); break;
      case 'issue': this.router.navigate(['/issue'], { queryParams: { jobCardId: this.jobCardId } }); break;
      case 'inward': this.router.navigate(['/inward']); break;
      case 'psf': this.router.navigate(['/psf']); break;
      case 'upload': this.router.navigate(['/upload-stock']); break;
      default: this.router.navigate(['/stock']); break;
    }
  }
}
