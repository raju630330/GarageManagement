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

  private _selectedPart: any = null;

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
        search: ['', Validators.required],
        qty: [1, [Validators.required, Validators.min(1), Validators.max(99999)]],
        unitPrice: [0, [Validators.required, Validators.min(0.01)]],  // FIX: min(0.01) not min(0)
        discount: [0, [Validators.min(0)]],
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

  private preloadPartById(partId: number): void {
    // FIX: correct URL /api/stock/get/{id}
    this.http.get<any>(`${this.baseUrl}/stock/get/${partId}`)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: part => {
          if (!part) return;
          // FIX: use part.partId (not part.id), part.purchasePrice (not part.unitPrice)
          this.pushItemRow(part, 1, part.purchasePrice || 0, 0, 'Part');
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

  onSelectedPart(part: any): void {
    if (!part) return;

    const exists = this.items.controls
      .some(c => c.value.partId === part.partId);

    if (exists) {
      alert('This part is already added.');
      return;
    }

    this._selectedPart = part;

    // FIX: pre-fill with purchasePrice, not sellingPrice
    this.orderForm.get('addItemForm')!.patchValue({
      unitPrice: part.purchasePrice || 0
    });
  }

  resetAddItemForm(): void {
    this.orderForm.get('addItemForm')!.reset({
      search: '',
      qty: 1,
      unitPrice: 0,
      discount: 0,
      serviceType: 'Part'
    });
    this._selectedPart = null;
  }

  addItem(): void {
    const f = this.orderForm.get('addItemForm') as FormGroup;
    f.markAllAsTouched();

    if (f.invalid) {
      alert('Fill required fields');
      return;
    }

    if (!this._selectedPart) {
      alert('Select part first');
      return;
    }

    const { qty, unitPrice, discount, serviceType } = f.value;

    // FIX: added all missing validations
    if (qty <= 0) {
      alert('Quantity must be greater than 0');
      return;
    }

    if (unitPrice <= 0) {
      alert('Unit price must be greater than 0');
      return;
    }

    if (discount < 0) {
      alert('Discount cannot be negative');
      return;
    }

    if (discount >= qty * unitPrice) {
      alert('Discount must be less than item total amount');
      return;
    }

    this.pushItemRow(this._selectedPart, qty, unitPrice, discount, serviceType);
    this.resetAddItemForm();
  }

  private pushItemRow(
    part: any,
    qty: number,
    unitPrice: number,
    discount: number,
    serviceType: string
  ): void {
    const taxPercent = part.taxPercent || 0;
    const baseAmount = qty * unitPrice - discount;
    const taxAmount = +(baseAmount * taxPercent / 100).toFixed(2);
    const total = +(baseAmount + taxAmount).toFixed(2);

    const row = this.fb.group({
      partId: [part.partId, Validators.required],  // FIX: part.partId not part.id
      partName: [part.partName],
      partNo: [part.partNo],
      brand: [part.brand],
      hsnCode: [part.hsnCode || ''],                // FIX: hsnCode not part.hsn
      taxPercent: [taxPercent],
      qty: [qty, [Validators.required, Validators.min(1)]],
      unitPrice: [unitPrice, [Validators.required, Validators.min(0)]],
      discount: [discount],
      taxAmount: [taxAmount],
      totalPurchasePrice: [total],
      serviceType: [serviceType],
      remarks: [''],
      sellerInfo: ['']
    });

    this.items.push(row);
  }

  recalcRow(i: number): void {
    const row = this.items.at(i);
    const qty = +row.value.qty || 0;
    const price = +row.value.unitPrice || 0;
    const disc = +row.value.discount || 0;
    const taxPct = +row.value.taxPercent || 0;

    const base = qty * price - disc;
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
      .reduce((s, c) => s + (+c.value.totalPurchasePrice || 0), 0)
      .toFixed(2);
  }

  get totalTaxAmount(): number {
    return +this.items.controls
      .reduce((s, c) => s + (+c.value.taxAmount || 0), 0)
      .toFixed(2);
  }

  get totalDiscountAmount(): number {
    return +this.items.controls
      .reduce((s, c) => s + (+c.value.discount || 0), 0)
      .toFixed(2);
  }

  submitOrder(): void {
    this.orderForm.markAllAsTouched();

    if (this.orderForm.invalid) {
      alert('Fill required fields');
      return;
    }

    if (this.items.length === 0) {
      alert('Add parts first');
      return;
    }

    this.submitting = true;

    const payload = {
      supplierId: +this.orderForm.value.supplierId,  // FIX: cast to number with +
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
            alert(res?.message || 'Order failed');
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
      case 'estimation':
        this.router.navigate(['/estimate', this.jobCardId]);
        break;
      case 'issue':
        this.router.navigate(['/issue'], { queryParams: { jobCardId: this.jobCardId } });
        break;
      case 'inward':
        this.router.navigate(['/inward']);
        break;
      case 'psf':
        this.router.navigate(['/psf']);
        break;
      case 'upload':
        this.router.navigate(['/upload-stock']);
        break;
      default:
        this.router.navigate(['/stock']);
        break;
    }
  }
}
