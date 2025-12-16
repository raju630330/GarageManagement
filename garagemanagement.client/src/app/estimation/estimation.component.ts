import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../services/alert.service';
import { ROLES } from '../constants/roles.constants';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JobCardService } from '../services/job-card.service';
import { MatDialog } from '@angular/material/dialog';
import { GlobalPopupComponent } from '../global-popup/global-popup.component';
import { TablePopupComponent } from '../table-popup/table-popup.component';
import { JobCardDto, VehicleDetailsUI } from '../models/job-card';



@Component({
  selector: 'app-estimation',
  standalone: false,
  templateUrl: './estimation.component.html',
  styleUrl: './estimation.component.css'
})
export class EstimationComponent implements OnInit {

  ROLES = ROLES;

  id!: number;

  vehicleDetails: VehicleDetailsUI = {
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
   /* this.jobcardService.getJobCardDetails(id).subscribe({
      next: (res) => {

        this.vehicleDetails = {
          regNo: res.vehicleData.registrationNo,
          jobCardNo: id.toString(),

          customerName: res.customerInfo.customerName,
          mobile: res.customerInfo.mobile,
          email: res.customerInfo.email,

          odometer: +res.vehicleData.odometerIn,
          model: res.vehicleData.serviceType,   // or actual model field
          fuelType: res.vehicleData.fuelType,
          vin: res.vehicleData.vin,
          engineNo: res.vehicleData.engineNo
        };

      },
      error: err => console.error(err)
    });*/

    this.jobcardService.getEstimationDetails(id).subscribe({
      next: (res) => {

        /* =============================
           1. Estimation Items
        ============================= */
        const itemsArray = this.items;
        itemsArray.clear(); // 🔴 IMPORTANT – avoid duplicates

        if (res.estimation?.items?.length) {
          res.estimation.items.forEach((i: any) => {
            itemsArray.push(this.fb.group({
              name: [i.name],
              type: [i.type],
              partNo: [i.partNo],
              rate: [i.rate],
              discount: [i.discount],
              hsn: [i.hSN],               // ✅ matches backend
              taxPercent: [i.taxPercent],
              taxAmount: [i.taxAmount],
              total: [i.total],
              approval: ['Pending'],
              reason: ['']
            }));
          });
        }

        /* =============================
           2. Patch Estimation Totals
        ============================= */
        this.estimationForm.patchValue({
          discountInput: res.estimation?.discountInput ?? 0,
          paidAmount: res.estimation?.paidAmount ?? 0
        });

        /* =============================
           3. Popup Data (IMPORTANT FIX)
        ============================= */
        this.popupData = {
          tyreBattery: [...(res.popup?.tyreBattery || [])],
          cancelledInvoices: [...(res.popup?.cancelledInvoices || [])],
          collections: [...(res.popup?.collections || [])],
          serviceSuggestions: res.popup?.serviceSuggestions || '',
          remarks: res.popup?.remarks || ''
        };

        /* =============================
           4. Recalculate Totals
        ============================= */
        this.calculateTotals(); // ✅ MUST
      },
      error: err => console.error(err)
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
      hsn: ['9987'],
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

    this.totalDiscountAmount = items.reduce((sum, item) => sum + (item.value.discount || 0), 0);
    this.totalTaxAmount = items.reduce((sum, item) => sum + item.value.taxAmount, 0);
    this.grossAmount = items.reduce((sum, item) => sum + item.value.total, 0);

    this.estimationForm.get('discountInput')?.setValue(this.totalDiscountAmount);
    const discountInput = this.estimationForm.value.discountInput || 0;
    const paidAmount = this.estimationForm.value.paidAmount || 0;

    const afterDiscount = this.grossAmount - discountInput;
    this.roundOffAmount = +(Math.round(afterDiscount) - afterDiscount).toFixed(2);

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

    const formatted: any = {};
    this.previousJobCards.forEach((jc: any, i: number) => {
      formatted[`#${i + 1} - Job Card No`] = jc.jobCardNo;
      formatted[`Date (${i + 1})`] = jc.date?.split('T')[0];
      formatted[`Status (${i + 1})`] = jc.status;
    });

    this.openPopup("Previous Job Cards (R)", formatted);
  }

  // Reusable popup open function
  openPopup(title: string, fields: any) {
    this.dialog.open(GlobalPopupComponent, {
      data: { title, fields },
      width: '800px',
      maxWidth: '95vw',
      autoFocus: false,
      disableClose: true,
      panelClass: 'custom-dialog-zindex'
    });
  }

  openPopupForBottomMenu(activeTab: string) {
    console.log('BOTTOM MENU CLICKED:', activeTab);

    const dialogRef = this.dialog.open(TablePopupComponent, {
      width: '95%',
      maxWidth: '95vw',
      autoFocus: false,
      disableClose: true,
      data: {
        title: 'Job Card Details',
        tabs: [
          'TYRE/BATTERY',
          'CANCELLED INVOICES',
          'SERVICE SUGGESTIONS',
          'COLLECTIONS',
          'REMARKS'
        ],
        activeTab: activeTab,
        // send previously saved data to the popup
        tyreBattery: this.popupData.tyreBattery || [],
        cancelledInvoices: this.popupData.cancelledInvoices || [],
        serviceSuggestions: this.popupData.serviceSuggestions || '',
        collections: this.popupData.collections || [],
        remarks: this.popupData.remarks || ''
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log(`POPUP DATA FOR ${activeTab}:`, result);
        // store the result in popupData
        this.popupData = { ...this.popupData, ...result };
      } else {
        console.log(`POPUP CLOSED WITHOUT DATA FOR ${activeTab}`);
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

      if (Array.isArray(tabData)) {
        const invalidRow = tabData.find((row: any) =>
          requiredFields[tabKey].some(field => !row[field] && row[field] !== 0)
        );
        if (invalidRow) {
          this.alert.showError(`Please fill all required fields in ${tabKey}.`);
          return;
        }
      } else {
        if (!tabData || tabData.toString().trim() === '') {
          this.alert.showError(`Please enter ${tabKey}.`);
          return;
        }
      }
    }

    // Exclude addItemForm
    const { addItemForm, ...estimationDetails } = this.estimationForm.value;

    // Include JobCardId in payload
    const payload = {
      jobCardId: this.id,    // <--- HERE
      estimation: estimationDetails,
      popup: this.popupData
    };

    console.log(payload);

    // Call API
    this.jobcardService.saveJobCardEstimation(payload).subscribe({
      next: (res) => {
        this.router.navigate(['/jobcardlist']);
      },
      error: (err) => {
        console.error(err);

        // Check if backend returned validation errors
        if (err.status === 400 && err.error?.errors) {
          // err.error.errors should be a dictionary { fieldName: errorMessage }
          Object.keys(err.error.errors).forEach((field) => {
            const control = this.estimationForm.get(field);
            if (control) {
              control.setErrors({ backend: err.error.errors[field] });
            }
          });
        } else {
          // Generic error alert
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
}
