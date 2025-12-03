import { Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { JobCardService } from '../services/job-card.service';
import { JobCard } from '../models/job-card';
import { Router } from '@angular/router';

@Component({
  selector: 'app-job-cards',
  templateUrl: './job-cards.component.html',
  styleUrls: ['./job-cards.component.css'],
  standalone: false
})
export class JobCardsComponent implements OnInit {
  @ViewChild('slider') slider!: ElementRef;
  @ViewChild('popupContainer') popupContainer!: ElementRef;

  isDown = false;
  startX = 0;
  scrollLeft = 0;

  dateForm!: FormGroup;
  searchText: string = '';

  jobCards: JobCard[] = [];
  filteredJobCards: JobCard[] = [];

  showPopup = false;

  displayedColumns: string[] = [
    'refNo', 'jobCardNo', 'regNo', 'invoiceNo', 'serviceType',
    'vehicle', 'status', 'customerName', 'mobileNo',
    'arrivalDate', 'arrivalTime', 'insuranceCorporate',
    'claimNo', 'estDeliveryDate', 'accidentDate'
  ];

  statusList = [
    { name: 'Request for Estimation', color: '#2196F3', icon: 'request_quote', count: 0, amount: 0 },
    { name: 'Estimate', color: '#3F51B5', icon: 'edit', count: 0, amount: 0 },
    { name: 'Spares Pending', color: '#FF9800', icon: 'build', count: 0, amount: 0 },
    { name: 'Work-In-Progress', color: '#9C27B0', icon: 'pending', count: 0, amount: 0 },
    { name: 'Ready for Delivery', color: '#4CAF50', icon: 'local_shipping', count: 0, amount: 0 },
    { name: 'Invoice', color: '#795548', icon: 'receipt_long', count: 0, amount: 0 },
    { name: 'Delivered', color: '#009688', icon: 'check_circle', count: 0, amount: 0 },
    { name: 'In Workshop', color: '#607D8B', icon: 'garage', count: 0, amount: 0 },
    { name: 'Estimation Rejected', color: '#F44336', icon: 'cancel', count: 0, amount: 0 },
    { name: 'Ins Approval Pending', color: '#673AB7', icon: 'hourglass_empty', count: 0, amount: 0 },
    { name: 'Approval Pending', color: '#E91E63', icon: 'approval', count: 0, amount: 0 }
  ];

  constructor(private fb: FormBuilder, private jobCardService: JobCardService, private router: Router) { }

  ngOnInit(): void {
    this.dateForm = this.fb.group({
      fromDate: [''],
      toDate: ['']
    });

    this.jobCards = this.jobCardService.getJobCards();
    this.filteredJobCards = [...this.jobCards];
  }

  // Status box drag
  dragStart(e: MouseEvent) {
    this.isDown = true;
    this.startX = e.pageX - this.slider.nativeElement.offsetLeft;
    this.scrollLeft = this.slider.nativeElement.scrollLeft;
    this.slider.nativeElement.style.cursor = 'grabbing';
  }

  dragEnd(e: MouseEvent) {
    this.isDown = false;
    this.slider.nativeElement.style.cursor = 'grab';
  }

  dragMove(e: MouseEvent) {
    if (!this.isDown) return;
    e.preventDefault();
    const x = e.pageX - this.slider.nativeElement.offsetLeft;
    const walk = (x - this.startX) * 2;
    this.slider.nativeElement.scrollLeft = this.scrollLeft - walk;
  }

  // Search filter
  applyFilter() {
    const text = this.searchText.toLowerCase();
    this.filteredJobCards = this.jobCards.filter(card =>
      (card.customerName?.toLowerCase().includes(text) ?? false) ||
      (card.regNo?.toLowerCase().includes(text) ?? false) ||
      (card.claimNo?.toLowerCase().includes(text) ?? false)
    );
  }

  // Popup toggle
  togglePopup() {
    this.showPopup = !this.showPopup;
  }

  createNewJobCard() {
    this.showPopup = false;
    this.router.navigate(['/newjobcard']);
    console.log('New Job Card clicked');
  }

  createNewEstimate() {
    this.showPopup = false;
    console.log('New Estimate clicked');
  }


}
