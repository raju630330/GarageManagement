import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../services/alert.service';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JobCardService } from '../services/job-card.service';
import { MatDialog } from '@angular/material/dialog';
import { GlobalPopupComponent } from '../global-popup/global-popup.component';
import { TablePopupComponent } from '../table-popup/table-popup.component';
import { JobCardDto, PopupOption, PopupTabConfig, VehicleDetailsUI } from '../models/job-card';
import { getToday } from '../shared/form-utils';
import { HttpClient } from '@angular/common/http';
import { LoaderService } from '../services/loader.service';
import { StockService } from '../services/stock.service';
import { IssueService } from '../services/issue.service';

@Component({
  selector: 'app-estimation',
  standalone: false,
  templateUrl: './estimation.component.html',
  styleUrl: './estimation.component.css',
})
export class EstimationComponent implements OnInit {

  id!: number;

  vehicleDetails: VehicleDetailsUI = {
    jobCardNumberForEstimation: '',
    regNo: '',
    jobCardNo: '',
    customerName: '',
    mobile: '',
    email: '',
    odometer: 0,
    model: '',
    fuelType: '',
    vin: '',
    engineNo: ''
  };

  totalDue = 10000;
  showCanvas = false;
  showEstimation = true;
  showMenu = false;
  popupData: any = {};

  estimationForm!: FormGroup;
  previousJobCards: any[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private alert: AlertService,
    private jobcardService: JobCardService,
    private dialog: MatDialog,
    private router: Router,
    private http: HttpClient,
    private loader: LoaderService,
    public stockservice: StockService,
    public issueservice: IssueService
  ) { }

  ngOnInit(): void {
    this.estimationForm = this.fb.group({
      addItemForm: this.fb.group({
        search: ['', Validators.required],
        workshopState: ['', Validators.required],
        quantity: [null, [Validators.required, Validators.min(1)]],  // FIX: added min(1)
        unitPrice: [null, [Validators.required, Validators.min(0.01)]], // FIX: min(0.01)
        serviceType: ['', Validators.required]
      }),
      discountInput: [0],
      paidAmount: [0],
      items: this.fb.array([])
    });

    this.route.paramMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.id = Number(idParam);
        this.loadEstimationData(this.id);
        this.loadPreviousJobCards(this.id);
      }
    });
  }

  // ── Autocomplete ──────────────────────────────────────────────────────────

  selectedPart: any = null;

  onSelectedPart(part: any): void {
    // part.id comes from autocomplete result
    this.stockservice.getPartById(part.id).subscribe({
      next: (data: any) => {
        this.selectedPart = data;
        // FIX: use data.sellingPrice for estimation (customer charge)
        this.addItemForm.patchValue({
          search: part.name,
          unitPrice: data.sellingPrice || 0,
          quantity: 1
        });
      },
      error: (err) => {
        console.error('Failed to load part details', err);
      }
    });
  }

  resetEntireForm(): void {
    this.selectedPart = null;
    this.addItemForm.reset({
      search: '',
      workshopState: '',
      unitPrice: 0,
      quantity: 0,
      serviceType: '',
    });
  }

  get addItemForm(): FormGroup {
    return this.estimationForm.get('addItemForm') as FormGroup;
  }

  get items(): FormArray {
    return this.estimationForm.get('items') as FormArray;
  }

  // ── Load Data ─────────────────────────────────────────────────────────────

  loadEstimationData(id: number): void {
    this.jobcardService.getEstimationDetails(id).subscribe({
      next: (res) => {
        this.vehicleDetails = {
          jobCardNumberForEstimation: res.vehicleData.jobCardNo,
          regNo: res.vehicleData.registrationNo,
          jobCardNo: id.toString(),
          customerName: res.customerInfo.customerName,
          mobile: res.customerInfo.mobile,
          email: res.customerInfo.email,
          odometer: +res.vehicleData.odometerIn,
          model: res.vehicleData.serviceType,
          fuelType: res.vehicleData.fuelType,
          vin: res.vehicleData.vin,
          engineNo: res.vehicleData.engineNo
        };

        const itemsArray = this.items;
        itemsArray.clear();
        if (res.estimation?.items?.length) {
          res.estimation.items.forEach((i: any) => {
            itemsArray.push(this.fb.group({
              id: [i.id],
              partId: [i.partId],
              name: [i.name, Validators.required],
              type: [i.type, Validators.required],
              partNo: [i.partNo, Validators.required],
              rate: [i.rate, [Validators.required, Validators.min(0)]],
              discount: [i.discount, Validators.min(0)],
              hSN: [i.hsn],
              taxPercent: [i.taxPercent],
              taxAmount: [i.taxAmount],
              total: [i.total],
              approval: [i.approval && i.approval.trim() !== '' ? i.approval : 'Pending'],
              reason: ['']
            }));
          });
        }

        this.estimationForm.patchValue({
          discountInput: res.estimation?.discountInput ?? 0,
          paidAmount: res.estimation?.paidAmount ?? 0
        });

        this.popupData = {
          tyreBattery: [...(res.popup?.tyreBattery || [])],
          cancelledInvoices: [...(res.popup?.cancelledInvoices || [])],
          collections: [...(res.popup?.collections || [])],
          serviceSuggestions: res.popup?.serviceSuggestions || '',
          remarks: res.popup?.remarks || ''
        };

        this.calculateTotals();
      },
      error: (err) => console.error(err)
    });
  }

  // ── Add Item ──────────────────────────────────────────────────────────────

  addItem(): void {
    if (this.addItemForm.invalid) {
      this.addItemForm.markAllAsTouched();
      this.alert.showError('Please fill all required fields before adding the item.');
      return;
    }

    if (!this.selectedPart) {
      this.alert.showError('Please select a Part before adding.');
      return;
    }

    const { quantity, serviceType } = this.addItemForm.value;
    const part = this.selectedPart;

    // FIX: all validations added
    if (quantity <= 0) {
      this.alert.showError('Quantity must be greater than 0');
      return;
    }

    if ((part.sellingPrice || 0) <= 0) {
      this.alert.showError('Part has no selling price. Please update the part first.');
      return;
    }

    // FIX: taxPercent from part, not hardcoded 10
    const taxPercent = part.taxPercent || 0;
    const baseAmount = quantity * part.sellingPrice;
    const taxAmount = +(baseAmount * taxPercent / 100).toFixed(2);
    const total = +(baseAmount + taxAmount).toFixed(2);

    const item = this.fb.group({
      jobCardId: [this.id],
      id: [0],
      partId: [part.partId],           // FIX: part.partId not part.id
      name: [part.partName],
      type: [serviceType],
      partNo: [part.partNo],
      brand: [part.brand],
      quantity: [quantity],
      rate: [part.sellingPrice],
      discount: [0],
      hSN: [part.hsnCode ?? ''],    // FIX: part.hsnCode not part.hsn
      taxPercent: [taxPercent],            // FIX: from part not hardcoded
      taxAmount: [taxAmount],
      total: [total],
      approval: ['Pending'],
      reason: ['']
    });

    this.jobcardService.saveJobCardEstimation(item.value).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.items.push(item);
          this.calculateTotals();
          this.resetEntireForm();
        } else {
          this.alert.showError(res.message);
        }
      },
      error: () => {
        this.alert.showError('Internal server error');
      }
    });
  }

  // ── Remove Row ────────────────────────────────────────────────────────────

  removeRow(index: number): void {
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      this.items.removeAt(index);
      this.calculateTotals();
    });
  }

  // ── Totals ────────────────────────────────────────────────────────────────

  totalDiscountAmount = 0;
  totalTaxAmount = 0;
  grossAmount = 0;
  netPayableAmount = 0;
  roundOffAmount = 0;
  balanceAmount = 0;

  calculateTotals(): void {
    const items = this.items.controls as FormGroup[];

    this.totalDiscountAmount = items.reduce(
      (sum, item) => sum + (+item.get('discount')?.value || 0), 0
    );

    this.totalTaxAmount = items.reduce(
      (sum, item) => sum + (+item.get('taxAmount')?.value || 0), 0
    );

    this.grossAmount = items.reduce(
      (sum, item) => sum + (+item.get('total')?.value || 0), 0
    );

    this.estimationForm.get('discountInput')?.setValue(
      this.totalDiscountAmount, { emitEvent: false }
    );

    const discountInput = this.totalDiscountAmount || 0;
    const paidAmount = this.estimationForm.value.paidAmount || 0;
    const afterDiscount = this.grossAmount - discountInput;

    this.roundOffAmount = +(Math.round(afterDiscount) - afterDiscount).toFixed(2);
    this.netPayableAmount = afterDiscount + this.roundOffAmount;
    this.balanceAmount = this.netPayableAmount - paidAmount;
  }

  // ── UI Toggles ────────────────────────────────────────────────────────────

  toggleMenu(): void { this.showMenu = !this.showMenu; }
  toggleContent(): void { this.showCanvas = !this.showCanvas; }

  selectCategory(cat: any): void {
    this.addItemForm.get('search')?.setValue(cat.label);
  }

  loadPreviousJobCards(id: number): void {
    this.previousJobCards = this.jobcardService.getPreviousJobCards(id);
  }

  openPreviousJobCardsPopup(): void {
    if (!this.previousJobCards || this.previousJobCards.length === 0) {
      this.openPopup('Previous Job Cards (R)', { Message: 'No previous job cards found.' });
      return;
    }
    const formatted = this.previousJobCards.map(jc => ({
      jobCardNo: jc.jobCardNo,
      date: jc.date?.split('T')[0],
      status: jc.status
    }));
    this.openPopup('Previous Job Cards (R)', { fieldsArray: formatted });
  }

  openPopup(title: string, data: any): void {
    this.dialog.open(GlobalPopupComponent, {
      width: '600px',
      data: { title, ...data }
    });
  }

  // ── Service Categories ────────────────────────────────────────────────────

  serviceCategories = [
    { key: 'packages', label: 'Packages', icon: 'bi-box-seam' },
    { key: 'parts', label: 'Relevant Parts', icon: 'bi-gear' },
    { key: 'services', label: 'All Services', icon: 'bi-car-front' },
    { key: 'alignment', label: 'Wheel Alignment', icon: 'bi-arrow-left-right' },
    { key: 'balancing', label: 'Wheel Balancing', icon: 'bi-circle-half' },
    { key: 'wash', label: 'Wash & Detailing', icon: 'bi-droplet' },
    { key: 'pms', label: 'PMS & Checkups', icon: 'bi-shield-check' },
    { key: 'tyres', label: 'Tyres & Services', icon: 'bi-speedometer2' },
    { key: 'engine', label: 'Engine Work', icon: 'bi-cpu' },
    { key: 'battery', label: 'Battery', icon: 'bi-battery-full' },
    { key: 'electrical', label: 'Electrical Work', icon: 'bi-lightning-charge' },
    { key: 'ac', label: 'AC Service', icon: 'bi-snow' },
    { key: 'brakes', label: 'Brake Service', icon: 'bi-slash-circle' },
    { key: 'suspension', label: 'Suspension', icon: 'bi-tropical-storm' },
    { key: 'diagnostics', label: 'Diagnostics', icon: 'bi-search' },
    { key: 'body', label: 'Body Work', icon: 'bi-car-front' },
    { key: 'paint', label: 'Painting', icon: 'bi-palette' },
    { key: 'lights', label: 'Lights & Indicators', icon: 'bi-lightbulb' },
    { key: 'insurance', label: 'Insurance Claim', icon: 'bi-file-earmark-text' },
    { key: 'pickup', label: 'Pick-up & Drop', icon: 'bi-truck' },
    { key: 'custom', label: 'Custom Requests', icon: 'bi-stars' },
    { key: 'accessories', label: 'Accessories', icon: 'bi-bag-plus' }
  ];

  tyreBrands: PopupOption[] = [
    { label: 'MRF', value: 'MRF' },
    { label: 'Apollo', value: 'Apollo' },
    { label: 'JK Tyre', value: 'JK Tyre' }
  ];

  batteryBrands: PopupOption[] = [
    { label: 'Exide', value: 'Exide' },
    { label: 'Amaron', value: 'Amaron' },
    { label: 'Luminous', value: 'Luminous' }
  ];

  popupTabs: PopupTabConfig[] = [
    {
      tabKey: 'tyreBattery',
      label: 'TYRE / BATTERY',
      allowAdd: true,
      allowDelete: true,
      columns: [
        {
          field: 'type', header: 'Type', type: 'select',
          options: [
            { label: 'Select Type', value: null },
            { label: 'Tyre', value: 'Tyre' },
            { label: 'Battery', value: 'Battery' }
          ],
          validators: [Validators.required]
        },
        {
          field: 'brand', header: 'Brand', type: 'select',
          options: [{ label: 'Select Brand', value: null }],
          validators: [Validators.required],
          getOptions: (row: any): PopupOption[] => {
            const base: PopupOption[] = [{ label: 'Select Brand', value: null }];
            if (row.type === 'Tyre') return base.concat(this.tyreBrands);
            if (row.type === 'Battery') return base.concat(this.batteryBrands);
            return base;
          }
        },
        { field: 'model', header: 'Model', type: 'text', validators: [Validators.required] },
        { field: 'manufactureDate', header: 'Mfg Date', type: 'date', validators: [Validators.required], maxDate: getToday() },
        { field: 'expiryDate', header: 'Expiry Date', type: 'date', validators: [Validators.required], minDate: getToday() },
        {
          field: 'condition', header: 'Condition', type: 'select',
          options: [
            { label: 'Select', value: null },
            { label: 'Good', value: 'Good' },
            { label: 'Average', value: 'Average' },
            { label: 'Poor', value: 'Poor' }
          ],
          validators: [Validators.required]
        }
      ]
    },
    {
      tabKey: 'cancelledInvoices',
      label: 'CANCELLED INVOICES',
      allowAdd: true,
      allowDelete: true,
      columns: [
        { field: 'invoiceNo', header: 'Invoice No', type: 'text', validators: [Validators.required] },
        { field: 'date', header: 'Date', type: 'date', validators: [Validators.required] },
        { field: 'amount', header: 'Amount', type: 'number', validators: [Validators.required, Validators.min(0)] }
      ]
    },
    { tabKey: 'serviceSuggestions', label: 'SERVICE SUGGESTIONS', isTextarea: true },
    {
      tabKey: 'collections',
      label: 'COLLECTIONS',
      allowAdd: true,
      allowDelete: true,
      columns: [
        {
          field: 'type', header: 'Payment Type', type: 'select',
          options: [
            { label: 'Select', value: null },
            { label: 'Cash', value: 'Cash' },
            { label: 'Cheque', value: 'Cheque' },
            { label: 'Online', value: 'Online' }
          ],
          validators: [Validators.required]
        },
        {
          field: 'bank', header: 'Bank', type: 'select',
          options: [
            { label: 'Select', value: null },
            { label: 'HDFC', value: 'HDFC' },
            { label: 'ICICI', value: 'ICICI' },
            { label: 'SBI', value: 'SBI' }
          ],
          validators: [Validators.required]
        },
        { field: 'chequeNo', header: 'Cheque No', type: 'text', validators: [Validators.required] },
        { field: 'amount', header: 'Amount', type: 'number', validators: [Validators.required, Validators.min(0)] },
        { field: 'date', header: 'Date', type: 'date', validators: [Validators.required] },
        { field: 'invoiceNo', header: 'Invoice No', type: 'text', validators: [Validators.required] },
        { field: 'remarks', header: 'Remarks', type: 'text', validators: [Validators.required] }
      ]
    },
    { tabKey: 'remarks', label: 'REMARKS', isTextarea: true }
  ];

  openPopupForBottomMenu(label: string): void {
    const tab = this.popupTabs.find(t => t.label === label);
    if (!tab) return;

    const ref = this.dialog.open(TablePopupComponent, {
      width: '95vw',
      maxWidth: '1200px',
      disableClose: true,
      data: {
        tabs: this.popupTabs,
        popupData: this.popupData || {},
        activeTabKey: tab.tabKey,
        jobCardId: this.id
      }
    });

    ref.afterClosed().subscribe(updatedData => {
      if (updatedData) {
        this.popupData = { ...this.popupData, ...updatedData };
      }
    });
  }

  // ── Mark Job as Done ──────────────────────────────────────────────────────

  markJobAsDone(): void {
    if (!this.id) {
      this.alert.showError('Job Card ID is missing. Please reload and try again.');
      return;
    }

    this.issueservice.getPendingIssues(this.id).subscribe({
      next: (res) => {
        if (!res.isSuccess) {
          this.alert.showError('Unable to verify pending items.');
          return;
        }

        if (res.data && res.data.length > 0) {
          this.alert.showWarning(
            `Cannot complete job. ${res.data.length} item(s) are still pending to issue.`
          );
          return;
        }

        const dto = {
          jobCardId: this.id,
          discountInput: this.totalDiscountAmount,
          paidAmount: this.estimationForm.value.paidAmount || 0,
          grossAmount: this.grossAmount,
          netAmount: this.netPayableAmount,
          balanceAmount: this.balanceAmount,
          roundOffAmount: this.roundOffAmount,
          items: this.items.controls.map((item: any) => ({
            itemId: item.value.id,
            discount: item.value.discount
          }))
        };

        this.jobcardService.completeJobCard(dto).subscribe({
          next: (response: any) => {
            if (response.isSuccess) {
              this.alert.showSuccess('Job marked as completed successfully.');
              this.router.navigate(['/jobcardlist']);
            } else {
              this.alert.showError(response.message);
            }
          },
          error: () => {
            this.alert.showError('Internal server error.');
          }
        });
      }
    });
  }

  cancelJobCard(): void {
    this.alert.showInfo('Job Card Cancelled');
    this.router.navigate(['/jobcardlist']);
  }

  getTabLabel(tabKey: string): string {
    return this.popupTabs.find(t => t.tabKey === tabKey)?.label || tabKey;
  }

  getFieldLabel(tabKey: string, field: string): string {
    const tab = this.popupTabs.find(t => t.tabKey === tabKey);
    const col = tab?.columns?.find(c => c.field === field);
    return col?.header || field;
  }

  // ── PDF Printing ──────────────────────────────────────────────────────────

  openWorkOrderPdf(): void {
    setTimeout(() => this.loader.show(), 0);
    this.http.get(`https://localhost:7086/api/reports/work-order/${this.id}`, { responseType: 'blob' })
      .subscribe(
        pdfBlob => this.printPdfInIframe(URL.createObjectURL(pdfBlob)),
        () => { this.loader.hide(); this.alert.showError('Error loading PDF'); }
      );
  }

  openEstimationPdf(): void {
    setTimeout(() => this.loader.show(), 0);
    this.http.get(`https://localhost:7086/api/reports/estimate/${this.id}`, { responseType: 'blob' })
      .subscribe(
        pdfBlob => this.printPdfInIframe(URL.createObjectURL(pdfBlob)),
        () => { this.loader.hide(); this.alert.showError('Error loading PDF'); }
      );
  }

  openGatePassPdf(): void {
    setTimeout(() => this.loader.show(), 0);
    this.http.get(`https://localhost:7086/api/reports/gatepass/${this.id}`, { responseType: 'blob' })
      .subscribe(
        pdfBlob => this.printPdfInIframe(URL.createObjectURL(pdfBlob)),
        () => { this.loader.hide(); this.alert.showError('Error loading PDF'); }
      );
  }

  private printPdfInIframe(fileURL: string): void {
    const oldIframe = document.getElementById('pdf-print-iframe') as HTMLIFrameElement;
    if (oldIframe) oldIframe.remove();

    const iframe = document.createElement('iframe');
    iframe.id = 'pdf-print-iframe';
    iframe.style.cssText = 'position:fixed;width:0;height:0;border:0;';
    iframe.src = fileURL;
    document.body.appendChild(iframe);

    const startTime = Date.now();
    iframe.onload = () => {
      const remaining = 800 - (Date.now() - startTime);
      setTimeout(() => {
        this.loader.hide();
        iframe.contentWindow?.focus();
        iframe.contentWindow?.print();
        iframe.contentWindow!.onafterprint = () => {
          iframe.remove();
          URL.revokeObjectURL(fileURL);
        };
      }, remaining > 0 ? remaining : 0);
    };
  }

  // ── Order Parts ───────────────────────────────────────────────────────────

  orderParts(): void {
    if (this.items.length === 0) {
      alert('No parts available to order.');
      return;
    }

    const partIds = this.items.controls
      .map(c => c.value.partId)
      .filter(Boolean);

    if (partIds.length === 0) {
      alert('No parts found to order.');
      return;
    }

    this.router.navigate(['/order'], {
      queryParams: {
        source: 'estimation',
        jobCardId: this.id,
        regNo: this.vehicleDetails.regNo,
        jobCardNo: this.vehicleDetails.jobCardNumberForEstimation,
        partIds: partIds.join(',')
      }
    });
  }
}
