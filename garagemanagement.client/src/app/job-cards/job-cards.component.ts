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

  // Status Slider drag
  isDown = false;
  startX = 0;
  scrollLeft = 0;

  // Forms & search
  dateForm!: FormGroup;
  searchText: string = '';

  // Job cards data
  jobCards: JobCard[] = [];
  filteredJobCards: JobCard[] = [];

  // Popup
  showPopup = false;

  // Table columns
  displayedColumns: string[] = [
    'REF No.', 'Job Card No.', 'Reg. No.', 'Invoice No.', 'Service Type',
    'Vehicle', 'Status', 'Customer Name', 'Mobile No.',
    'Arrival Date', 'Arrival Time', 'Insurance Corporate',
    'Claim No.', 'Est Delivery Date', 'Accident Date'
  ];

  // Status list for slider
  statusList = [
    { name: 'Request for Estimation', color: '#2196F3', iconClass: 'bi bi-file-earmark-text', count: 0, amount: 0 },
    { name: 'Estimate', color: '#3F51B5', iconClass: 'bi bi-calculator', count: 0, amount: 0 },
    { name: 'Spares Pending', color: '#FF9800', iconClass: 'bi bi-tools', count: 0, amount: 0 },
    { name: 'Work-In-Progress', color: '#9C27B0', iconClass: 'bi bi-hourglass-split', count: 0, amount: 0 },
    { name: 'Ready for Delivery', color: '#4CAF50', iconClass: 'bi bi-truck', count: 0, amount: 0 },
    { name: 'Invoice', color: '#795548', iconClass: 'bi bi-receipt', count: 0, amount: 0 },
    { name: 'Delivered', color: '#009688', iconClass: 'bi bi-check-circle', count: 0, amount: 0 },
    { name: 'In Workshop', color: '#607D8B', iconClass: 'bi bi-garage', count: 0, amount: 0 },
    { name: 'Estimation Rejected', color: '#F44336', iconClass: 'bi bi-x-circle', count: 0, amount: 0 },
    { name: 'Ins Approval Pending', color: '#673AB7', iconClass: 'bi bi-hourglass', count: 0, amount: 0 },
    { name: 'Approval Pending', color: '#E91E63', iconClass: 'bi bi-patch-exclamation', count: 0, amount: 0 }
  ];

  // Pagination
  rowsPerPageOptions = [10, 20, 30];
  rowsPerPage = 10;
  currentPage = 1;
  totalPages = 1;
  paginatedJobCards: JobCard[] = [];
  maxPageButtons = 3;


  // For Estimation

  showPopupForEstimation: boolean = false;
  estimateForm!: FormGroup;
  selectedJobCard: any = null; 

  constructor(private fb: FormBuilder, public jobCardService: JobCardService, private router: Router) { }

  ngOnInit(): void {

    this.dateForm = this.fb.group({
      fromDate: [''],
      toDate: ['']
    });

    // ✅ API call
    this.jobCardService.getJobCards().subscribe({
      next: (res) => {
        this.jobCards = res;
        this.filteredJobCards = [...res];
        this.updatePagination();
      },
      error: (err) => {
        console.error('Failed to load job cards', err);
      }
    });
  }


  // =================== Status Slider Drag ===================
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

  // =================== Search Filter ===================
  applyFilter() {
    const text = this.searchText.toLowerCase();
    this.filteredJobCards = this.jobCards.filter(card =>
      (card.customerName?.toLowerCase().includes(text) ?? false) ||
      (card.regNo?.toLowerCase().includes(text) ?? false) ||
      (card.claimNo?.toLowerCase().includes(text) ?? false)
    );
    this.currentPage = 1;
    this.updatePagination();
  }

  // =================== Popup Toggle ===================
  togglePopup() {
    this.showPopup = !this.showPopup;
  }

  createNewJobCard() {
    this.showPopup = false;
    this.router.navigate(['/newjobcard']);
  }

  // =================== Pagination ===================
  updatePagination() {
    const totalItems = this.filteredJobCards.length;
    this.totalPages = Math.ceil(totalItems / this.rowsPerPage);

    const start = (this.currentPage - 1) * this.rowsPerPage;
    const end = start + this.rowsPerPage;

    this.paginatedJobCards = this.filteredJobCards.slice(start, end);
  }

  changeRowsPerPage(event: any) {
    this.rowsPerPage = +event.target.value;
    this.currentPage = 1;
    this.updatePagination();
  }

  goToPage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.currentPage = page;
    this.updatePagination();
  }

  // Safe template call
  goToPageSafe(page: number | string) {
    if (typeof page === 'number') {
      this.goToPage(page);
    }
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePagination();
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePagination();
    }
  }

  // =================== Get Visible Pages (max 3 + ellipsis) ===================
  getVisiblePages(): (number | string)[] {
    const total = this.totalPages;
    const current = this.currentPage;
    const pages: (number | string)[] = [];

    if (total <= this.maxPageButtons) {
      for (let i = 1; i <= total; i++) pages.push(i);
    } else {
      if (current <= 2) {
        pages.push(1, 2, 3, '...');
      } else if (current >= total - 1) {
        pages.push('...', total - 2, total - 1, total);
      } else {
        pages.push('...', current - 1, current, current + 1, '...');
      }
    }

    return pages;
  }


  // For Estimation

  createNewEstimate() {
    this.showPopupForEstimation = true;
    this.selectedJobCard = null; // reset previous selection
    this.estimateForm = this.fb.group({
      id: [''] // hidden field for selected job card id
    });
  }

  onSelectedJobCardDetailForEstimation(event: any) {
    // Set hidden id
    this.estimateForm.patchValue({ id: event.id });

    // Fetch Job Card details from service
    this.jobCardService.getJobCardDetails(event.id).subscribe(res => {
      // res should contain 4 fields for table display
      this.selectedJobCard = {
        showEstimateButton: true,
        displayDate: new Date(res.customerInfo.deliveryDate).toLocaleDateString(),
        jobCardNo: res.vehicleData.registrationNo,
        customer: res.customerInfo.customerName,
        status: 'Estimation Pending'

      };
    });
  }

  navigateToEstimate() {
      let jobCardId =  this.estimateForm.get('id')?.value

    if (this.selectedJobCard && jobCardId != null) {
      this.router.navigate(['/estimate'], { queryParams: { id: jobCardId } });
      this.showPopupForEstimation = false;
    }
  }

  closeEstimatePopup() {
    this.showPopupForEstimation = false;
    this.selectedJobCard = null;
  }

}
