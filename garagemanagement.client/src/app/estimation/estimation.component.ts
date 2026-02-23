import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../services/alert.service';
import { ROLES } from '../constants/roles.constants';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JobCardService } from '../services/job-card.service';
import { MatDialog } from '@angular/material/dialog';
import { GlobalPopupComponent } from '../global-popup/global-popup.component';
import { TablePopupComponent } from '../table-popup/table-popup.component';
import { JobCardDto, PopupOption, PopupTabConfig, VehicleDetailsUI } from '../models/job-card';
import { getToday } from '../shared/form-utils';
import { HttpClient } from '@angular/common/http';
import { LoaderService } from '../services/loader.service';
import { StockComponent } from '../stock/stock.component';
import { IssueComponent } from '../issue/issue.component';
import { InwardComponent } from '../inward/inward.component';
import { OrderComponent } from '../order/order.component';
import { StockService } from '../services/stock.service';



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


  /* -----------------------------------------
     Reactive Form Setup
  ----------------------------------------- */
  estimationForm!: FormGroup;
  previousJobCards: any[] = [];
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private alert: AlertService, private jobcardService: JobCardService, private dialog: MatDialog, private router: Router, private http: HttpClient, private loader: LoaderService, public stockservice: StockService
  ) { }

  ngOnInit(): void {

    this.estimationForm = this.fb.group({
      addItemForm: this.fb.group({
        search: ['', Validators.required],
        workshopState: ['', Validators.required],
        quantity: [null, Validators.required],
        unitPrice: [null, Validators.required],
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



  //autocomplete

  selectedPart: any; // Part selected from autocomplete

  onSelectedPart(part: any) {
    // part.id comes from autocomplete
    this.stockservice.getPartById(part.id).subscribe({
      next: (data : any) => {
        this.selectedPart = data;
        // Fill form fields
        this.addItemForm.patchValue({
          search: part.name,
          workshopState: '',
          unitPrice: data.sellingPrice,
          quantity: 0                  
        });
      },
      error: (err) => {
        console.error('Failed to load part details', err);
      }
    });
  }

  resetEntireForm() {
    // Clear selected part
    this.selectedPart = null;

    // Reset the add item form
    this.addItemForm.reset({
      search: '',
      workshopState: '',
      unitPrice: 0,
      quantity: 0,      
      serviceType: '', 
    });
  }

  get addItemForm() {
    return this.estimationForm.get('addItemForm') as FormGroup;
  }

  get items(): FormArray {
    return this.estimationForm.get('items') as FormArray;
  }

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
        itemsArray.clear(); // remove old items
        if (res.estimation?.items?.length) {
          res.estimation.items.forEach((i: any) => {
            itemsArray.push(this.fb.group({
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



  /* -----------------------------------------
   Service Categories
----------------------------------------- */
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



  /* -----------------------------------------
     Add Item (Reactive)
  ----------------------------------------- */
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

    if (true) { alert(quantity) }

    // Use selected part values
    const part = this.selectedPart;

    const taxPercent = 18;
    const baseAmount = quantity * part.sellingPrice;  // Use part selling price
    const taxAmount = (baseAmount * taxPercent) / 100;
    const total = baseAmount + taxAmount;

    const item = this.fb.group({
      jobCardId: this.id,
      partId: [part.id],         // save partId instead of name/free text
      name: [part.partName],
      type: [serviceType],
      partNo: [part.partNo],
      brand: [part.brand],
      quantity: [quantity],
      rate: [part.sellingPrice],
      discount: [0],
      hSN: [part.hsn ?? '9987'], // use part HSN if exists
      taxPercent: [taxPercent],
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
        }
        else {
          this.alert.showError("Not saved successfully");
          return;
        }
        
      },
      error: (error) => {
        this.alert.showError("Internal server error");
        return;
      }
    });
  }

  /* -----------------------------------------
     Remove Row
  ----------------------------------------- */
  removeRow(index: number): void {
    this.alert.confirm('Are you sure you want to remove this row?', () => {
      this.items.removeAt(index);
      this.calculateTotals();
    });
  }

  totalDiscountAmount = 0;
  totalTaxAmount = 0;
  grossAmount = 0;
  netPayableAmount = 0;
  roundOffAmount = 0;
  balanceAmount = 0;

  /* -----------------------------------------
     Totals Calculation
  ----------------------------------------- */

  calculateTotals(): void {
    const items = this.items.controls as FormGroup[];

    this.totalDiscountAmount = items.reduce(
      (sum, item) => sum + (+item.get('discount')?.value || 0),
      0
    );

    this.totalTaxAmount = items.reduce(
      (sum, item) => sum + (+item.get('taxAmount')?.value || 0),
      0
    );

    this.grossAmount = items.reduce(
      (sum, item) => sum + (+item.get('total')?.value || 0),
      0
    );

    this.estimationForm.get('discountInput')?.setValue(
      this.totalDiscountAmount,
      { emitEvent: false }
    );

    const discountInput = this.totalDiscountAmount || 0;
    const paidAmount = this.estimationForm.value.paidAmount || 0;

    const afterDiscount = this.grossAmount - discountInput;

    this.roundOffAmount = +(
      Math.round(afterDiscount) - afterDiscount
    ).toFixed(2);

    this.netPayableAmount = afterDiscount + this.roundOffAmount;

    this.balanceAmount = this.netPayableAmount - paidAmount;
  }


  /* -----------------------------------------
     UI Toggles
  ----------------------------------------- */
  toggleMenu(): void {
    this.showMenu = !this.showMenu;
  }

  toggleContent(): void {
    this.showCanvas = !this.showCanvas;
  }

  selectCategory(cat: any): void {
    this.addItemForm.get('search')?.setValue(cat.label);
  }

  loadPreviousJobCards(id: number) {
    this.previousJobCards = this.jobcardService.getPreviousJobCards(id);
  }
  openPreviousJobCardsPopup() {
    if (!this.previousJobCards || this.previousJobCards.length === 0) {
      this.openPopup("Previous Job Cards (R)", { Message: "No previous job cards found." });
      return;
    }

    const formatted = this.previousJobCards.map(jc => ({
      jobCardNo: jc.jobCardNo,
      date: jc.date?.split('T')[0],
      status: jc.status
    }));

    this.openPopup("Previous Job Cards (R)", { fieldsArray: formatted });
  }

  openPopup(title: string, data: any) {
    this.dialog.open(GlobalPopupComponent, {
      width: '600px',
      data: { title, ...data }
    });
  }

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
          field: 'type',
          header: 'Type',
          type: 'select',
          options: [
            { label: 'Select Type', value: null },
            { label: 'Tyre', value: 'Tyre' },
            { label: 'Battery', value: 'Battery' }
          ],
          validators: [Validators.required]
        },
        {
          field: 'brand',
          header: 'Brand',
          type: 'select',
          options: [{ label: 'Select Brand', value: null }],
          validators: [Validators.required],
          getOptions: (row: any): PopupOption[] => {
            const base: PopupOption[] = [
              { label: 'Select Brand', value: null }
            ];

            if (row.type === 'Tyre') {
              return base.concat(this.tyreBrands);
            }

            if (row.type === 'Battery') {
              return base.concat(this.batteryBrands);
            }

            return base;
          }

        },
        { field: 'model', header: 'Model', type: 'text', validators: [Validators.required] },
        { field: 'manufactureDate', header: 'Mfg Date', type: 'date', validators: [Validators.required], maxDate: getToday() },
        { field: 'expiryDate', header: 'Expiry Date', type: 'date', validators: [Validators.required], minDate: getToday() },
        {
          field: 'condition',
          header: 'Condition',
          type: 'select',
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
    {
      tabKey: 'serviceSuggestions',
      label: 'SERVICE SUGGESTIONS',
      isTextarea: true
    },
    {
      tabKey: 'collections',
      label: 'COLLECTIONS',
      allowAdd: true,
      allowDelete: true,
      columns: [
        {
          field: 'type',
          header: 'Payment Type',
          type: 'select',
          options: [
            { label: 'Select', value: null },
            { label: 'Cash', value: 'Cash' },
            { label: 'Cheque', value: 'Cheque' },
            { label: 'Online', value: 'Online' }
          ],
          validators: [Validators.required]
        },
        {
          field: 'bank',
          header: 'Bank',
          type: 'select',
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
    {
      tabKey: 'remarks',
      label: 'REMARKS',
      isTextarea: true
    }
  ];


  openPopupForBottomMenu(label: string) {
    const tab = this.popupTabs.find(t => t.label === label);
    if (!tab) return;

    const ref = this.dialog.open(TablePopupComponent, {
      width: '95vw',
      maxWidth: '1200px',
      disableClose: true,
      data: {
        tabs: this.popupTabs,
        popupData: this.popupData || {},
        activeTabKey: tab.tabKey
      }
    });

    ref.afterClosed().subscribe(updatedData => {
      if (updatedData) {
        this.popupData = { ...this.popupData, ...updatedData };
      }
    });
  }



  markJobAsDone() {
    this.alert.showInfo("Job Card Saved Sucessfully");
  }

  cancelJobCard() {
    this.alert.showInfo("Job Card Cancelled");
    this.router.navigate(['/jobcardlist']);
    return;
  }
  getTabLabel(tabKey: string): string {
    return this.popupTabs.find(t => t.tabKey === tabKey)?.label || tabKey;
  }

  getFieldLabel(tabKey: string, field: string): string {
    const tab = this.popupTabs.find(t => t.tabKey === tabKey);
    const col = tab?.columns?.find(c => c.field === field);
    return col?.header || field;
  }

  openWorkOrderPdf() {
    // Force loader to render immediately
    setTimeout(() => this.loader.show(), 0);

    this.http.get(
      `https://localhost:7086/api/reports/work-order/${this.id}`,
      { responseType: 'blob' }
    ).subscribe(
      pdfBlob => {
        const fileURL = URL.createObjectURL(pdfBlob);
        this.printPdfInIframe(fileURL);
      },
      error => {
        this.loader.hide();
        this.alert.showError('Error loading PDF');
      }
    );
  }
  openEstimationPdf() {

    // Force loader to render immediately
    setTimeout(() => this.loader.show(), 0);

    this.http.get(
      `https://localhost:7086/api/reports/estimate/${this.id}`,
      { responseType: 'blob' }
    ).subscribe(
      pdfBlob => {
        const fileURL = URL.createObjectURL(pdfBlob);
        this.printPdfInIframe(fileURL);
      },
      error => {
        this.loader.hide();
        this.alert.showError('Error loading PDF');
      }
    );
  }
  openGatePassPdf() {

    // Force loader to render immediately
    setTimeout(() => this.loader.show(), 0);

    this.http.get(
      `https://localhost:7086/api/reports/gatepass/${this.id}`,
      { responseType: 'blob' }
    ).subscribe(
      pdfBlob => {
        const fileURL = URL.createObjectURL(pdfBlob);
        this.printPdfInIframe(fileURL);
      },
      error => {
        this.loader.hide();
        this.alert.showError('Error loading PDF');
      }
    );
  }

  private printPdfInIframe(fileURL: string): void {
    // Remove old iframe if exists
    const oldIframe = document.getElementById('pdf-print-iframe') as HTMLIFrameElement;
    if (oldIframe) {
      oldIframe.remove();
    }

    // Create hidden iframe
    const iframe = document.createElement('iframe');
    iframe.id = 'pdf-print-iframe';
    iframe.style.position = 'fixed';
    iframe.style.width = '0';
    iframe.style.height = '0';
    iframe.style.border = '0';
    iframe.src = fileURL;

    document.body.appendChild(iframe);

    const minLoaderTime = 800; // ms
    const startTime = Date.now();

    iframe.onload = () => {
      const elapsed = Date.now() - startTime;
      const remaining = minLoaderTime - elapsed;

      setTimeout(() => {
        this.loader.hide();
        iframe.contentWindow?.focus();
        iframe.contentWindow?.print();

        // Cleanup after print
        iframe.contentWindow!.onafterprint = () => {
          iframe.remove();
          URL.revokeObjectURL(fileURL);
        };
      }, remaining > 0 ? remaining : 0);
    };
  }

}
