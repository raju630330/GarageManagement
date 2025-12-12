import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from '../services/alert.service';
import { ROLES } from '../constants/roles.constants';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { JobCardService } from '../services/job-card.service';
import { MatDialog } from '@angular/material/dialog';
import { GlobalPopupComponent } from '../global-popup/global-popup.component';

interface EstimationItem {
  name: string;
  type: string;
  partNo: string;
  rate: number;
  discount: number;
  hsn: string;
  taxPercent: number;
  taxAmount: number;
  total: number;
  approval: string;
  reason: string;
}

@Component({
  selector: 'app-estimation',
  standalone: false,
  templateUrl: './estimation.component.html',
  styleUrl: './estimation.component.css'
})
export class EstimationComponent implements OnInit {

  ROLES = ROLES;

  registrationNumber: string = '';
  totalDue = 10000;
  showCanvas = false;
  showEstimation = true;
  showMenu = false;

  /* -----------------------------------------
     Reactive Form Setup
  ----------------------------------------- */
  estimationForm!: FormGroup;
  previousJobCards: any[] = [];
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private alert: AlertService, private jobcardService: JobCardService, private dialog: MatDialog,
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
      this.registrationNumber = params.get('registrationNo') || '';
      this.loadEstimationData(this.registrationNumber);
      this.loadPreviousJobCards(this.registrationNumber);
    });
  }

  get addItemForm() {
    return this.estimationForm.get('addItemForm') as FormGroup;
  }

  get items(): FormArray {
    return this.estimationForm.get('items') as FormArray;
  }

  loadEstimationData(regNo: string): void {
    console.log('Load estimation data for:', regNo);
  }

  /* -----------------------------------------
   Vehicle / Customer Details
----------------------------------------- */
  vehicleDetails = {
    regNo: '',
    jobCardNo: 'JC-0001',
    customerName: 'Ramesh',
    mobile: '9876543210',
    email: 'ramesh@mail.test',
    odometer: 45200,
    model: 'i20 Magna',
    fuelType: 'Petrol',
    vin: 'KMH1VIN0001',
    engineNo: 'ENG-12345'
  };

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

  loadPreviousJobCards(regNo: string) {
    this.previousJobCards = this.jobcardService.getPreviousJobCards(regNo);
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

}
