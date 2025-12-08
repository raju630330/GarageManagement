import { Component } from '@angular/core';

@Component({
  selector: 'app-estimation',
  standalone: false,
  templateUrl: './estimation.component.html',
  styleUrl: './estimation.component.css'
})
export class EstimationComponent {
  // top jobcard label visible under global header
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
    regNo: 'TS09 AB 1234',
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
    { key: 'parts', label: 'Relevant Parts', icon: 'bi-tools' },
    { key: 'services', label: 'All Services', icon: 'bi-gear' },
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
  getColumnTotal(column: string): number {
    return this.items.reduce((sum, item) => sum + (item[column] || 0), 0);
  }
  calculateTotals() {
    this.grandTotal = this.items.reduce((a, b) => a + b.total, 0);

    this.roundOff = Math.round(this.grandTotal) - this.grandTotal;

    this.balance = this.grandTotal - (this.paidAmount || 0);
  }

  get totalDiscount() {
    return this.items.reduce((sum, item) => sum + (item.discount || 0), 0);
  }

  get totalTax() {
    return this.items.reduce((sum, item) => sum + (item.taxAmount || 0), 0);
  }

  get totalAmount() {
    return this.items.reduce((sum, item) => sum + (item.total || 0), 0);
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
