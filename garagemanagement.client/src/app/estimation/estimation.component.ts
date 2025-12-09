import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-estimation',
  standalone: false,
  templateUrl: './estimation.component.html',
  styleUrl: './estimation.component.css'
})
export class EstimationComponent implements OnInit {
  // top jobcard label visible under global header
  regNo: string = '';

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    // Subscribe to query param changes
    this.route.queryParamMap.subscribe(params => {
      this.regNo = params.get('registrationNo') || '';
      console.log('Current registration number:', this.regNo);

      // Fetch or refresh estimation data whenever regNo changes
      this.loadEstimationData(this.regNo);
    });
  }

  loadEstimationData(regNo: string) {
    // Use regNo to fetch data from server or update table
    console.log('Load estimation data for:', regNo);
  }

  jobCardLabel = 'Job Card: JC-0001';
  totalDue = 10000;

  // whether to show canvas (vehicle details)
  showCanvas = false;

  // whether estimation table is visible
  showEstimation = true;

  // right-side menu
  showMenu = false;

  // search + inputs
  searchText = '';

  // jobcard / vehicle stub
  est = {
    regNo: this.regNo,
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
  // categories shown as buttons
  categories = [
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


    toggleMenu() {
    this.showMenu = !this.showMenu;
  }
  openDetails() {
    alert('Open "Details" popup (not implemented)');
  }

  openPreviousJobCards() {
    alert('Open previous job cards (not implemented)');
  }
  toggleContent() {
    this.showCanvas = !this.showCanvas;
  }
  selectCategory(c: any) {
    // simple behavior: set search text to category label
    this.searchText = c.label;
  }

  /*Table section */

  workshopStatus = "In Workshop";
  qty: any;
  price: any;
  serviceType = "Part";

  items: any[] = [];

  applyDiscount: any;
  grandTotal = 0;
  roundOff = 0;
  paidAmount: any;
  balance = 0;


  addItem() {
    if (!this.searchText || !this.qty || !this.price) return;

    let taxPercent = 18;
    let taxAmount = (this.price * this.qty * taxPercent) / 100;
    let total = (this.price * this.qty) + taxAmount;

    this.items.push({
      name: this.searchText,
      type: this.serviceType,
      partNo: "PN-" + Math.floor(Math.random() * 1000),
      rate: this.price,
      discount: 0,
      hsn: "9987",
      taxPercent: taxPercent,
      taxAmount: taxAmount,
      total: total,
      approval: "Pending",
      reason: ""
    });

    this.calculateTotals();
  }

  grossTotal: number = 0;
  netTotal: number = 0;
  paid: number = 0;

  totalDiscount: number = 0;
  totalTax: number = 0;
  calculateTotals() {
    // Sum of discount column
    this.totalDiscount = this.items.reduce((s, x) => s + (+x.discount || 0), 0);

    // Sum of tax column
    this.totalTax = this.items.reduce((s, x) => s + (+x.taxAmount || 0), 0);

    // Sum of total column
    this.grossTotal = this.items.reduce((s, x) => s + (+x.total || 0), 0);

    // Apply discount entered by user
    const afterDiscount = this.grossTotal - (this.applyDiscount || 0);

    // Round Off
    this.roundOff = +(Math.round(afterDiscount) - afterDiscount).toFixed(2);

    // Final net total
    this.netTotal = afterDiscount + this.roundOff;

    // Balance (Paid assumed 0)
    this.balance = this.netTotal - (this.paid || 0);
  }


  openTyreBattery() {
    alert("Tyre/Battery clicked!");
  }

  openCancelledInvoices() {
    alert("Cancelled Invoices clicked!");
  }

  openSuggestions() {
    alert("Service Suggestions clicked!");
  }

  openCollections() {
    alert("Collections clicked!");
  }
}
