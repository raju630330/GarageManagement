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



@Component({
  selector: 'app-estimation',
  standalone: false,
  templateUrl: './estimation.component.html',
  styleUrl: './estimation.component.css',
})
export class EstimationComponent implements OnInit {

  ROLES = ROLES;

  id!: number;

  vehicleDetails: VehicleDetailsUI = {
    jobCardNumberForEstimation:'',
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
    private alert: AlertService, private jobcardService: JobCardService, private dialog: MatDialog, private router: Router
  ) { }

  ngOnInit(): void {

    this.estimationForm = this.fb.group({
      addItemForm: this.fb.group({
        search: ['', Validators.required],
        workshopState: ['In Workshop', Validators.required],
        quantity: [null, Validators.required],
        unitPrice: [null, Validators.required],
        serviceType: ['Part', Validators.required]
      }),

      discountInput: [0],
      paidAmount: [0],

      items: this.fb.array([])
    });

    this.route.queryParamMap.subscribe(params => {
      const idParam = params.get('id');
      if (idParam) {
        this.id = Number(idParam);
        this.loadEstimationData(this.id);
        this.loadPreviousJobCards(this.id);
      }
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

    const { search, quantity, unitPrice, serviceType } = this.addItemForm.value;

    const taxPercent = 18;
    const baseAmount = quantity * unitPrice;
    const taxAmount = (baseAmount * taxPercent) / 100;
    const total = baseAmount + taxAmount;

    const item = this.fb.group({
      name: [search],
      type: [serviceType],
      partNo: [`PN-${Math.floor(Math.random() * 1000)}`],
      rate: [unitPrice],
      discount: [0],
      hSN: ['9987'],
      taxPercent: [taxPercent],
      taxAmount: [taxAmount],
      total: [total],
      approval: ['Pending'],
      reason: ['']
    });

    this.items.push(item);
    this.calculateTotals();
    this.addItemForm.reset({ workshopState: 'In Workshop', serviceType: 'Part' });
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
    if (!this.popupData) {
      this.alert.showError('Please fill the Job Card popup details before saving.');
      return;
    }

    const requiredFields: Record<string, string[]> = {
      tyreBattery: ['type', 'brand', 'model', 'manufactureDate', 'expiryDate', 'condition'],
      cancelledInvoices: ['invoiceNo', 'date', 'amount'],
      collections: ['type', 'bank', 'chequeNo', 'amount', 'date', 'invoiceNo', 'remarks'],
      serviceSuggestions: ['serviceSuggestions'],
      remarks: ['remarks']
    };

    for (const tabKey of Object.keys(requiredFields)) {
      const tabData = this.popupData[tabKey];
      const tabLabel = this.getTabLabel(tabKey);

      // 🔹 TABLE TABS
      if (Array.isArray(tabData)) {

        // ✅ REQUIRED: At least 1 row
        if (tabData.length === 0) {
          this.alert.showError(
            `Please enter ${tabLabel} details.<br><br>
             • At least one entry is required<br>
             • Click the bottom menu (☰)<br>
             • Enter the required details`
                    );
          return;
        }

        for (let i = 0; i < tabData.length; i++) {
          const row = tabData[i];

          if (!row) {
            this.alert.showError(`${tabLabel} → Row ${i + 1} is empty`);
            return;
          }

          for (const field of requiredFields[tabKey]) {
            const value = row[field];

            if (
              value === null ||
              value === undefined ||
              value === '' ||
              (typeof value === 'string' && value.trim() === '')
            ) {
              const fieldLabel = this.getFieldLabel(tabKey, field);
              this.alert.showError(
                `${tabLabel} → Row ${i + 1}: ${fieldLabel} is required`
              );
              return;
            }
          }
        }
      }
      // 🔹 TEXTAREA TABS
      else {
        if (!tabData || tabData.toString().trim() === '') {
          this.alert.showError(`Please enter ${tabLabel}`);
          return;
        }
      }
    }



    const { addItemForm, ...estimationDetails } = this.estimationForm.value;

    const payload = {
      jobCardId: this.id,  
      estimation: estimationDetails,
      popup: this.popupData
    };

    console.log(payload);

    this.jobcardService.saveJobCardEstimation(payload).subscribe({
      next: (res) => {
        this.router.navigate(['/jobcardlist']);
      },
      error: (err) => {
        console.error(err);

        if (err.status === 400 && err.error?.errors) {

          Object.keys(err.error.errors).forEach((field) => {
            const control = this.estimationForm.get(field);
            if (control) {
              control.setErrors({ backend: err.error.errors[field] });
            }
          });
        } else {
          alert(err.error?.message || 'Something went wrong!');
        }
      }
    });
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
}
