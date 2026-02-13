import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-workshop-list',
  standalone: false,
  templateUrl: './workshop-list.component.html',
  styleUrls: ['./workshop-list.component.css']
})
export class WorkshopListComponent implements OnInit {

  workshopForm!: FormGroup;
  workshops: any[] = [];

  // Pagination metadata
  totalRecords = 0;
  totalPages = 0;
  isLoading = false;

  private apiUrl = 'https://localhost:7086/api/WorkshopProfile/getall';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.loadWorkshops();
  }

  private initializeForm(): void {
    this.workshopForm = this.fb.group({
      pageNumber: [1],
      pageSize: [10]
    });
  }

  loadWorkshops(): void {
    this.isLoading = true;

    const { pageNumber, pageSize } = this.workshopForm.value;

    const params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    this.http.get<any>(this.apiUrl, { params }).subscribe({
      next: (res) => {
        this.workshops = res.data;
        this.totalRecords = res.totalRecords;
        this.totalPages = res.totalPages;
        this.workshopForm.patchValue({
          pageNumber: res.pageNumber
        });
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }

  nextPage(): void {
    const pageNumber = this.workshopForm.get('pageNumber')?.value;
    if (pageNumber < this.totalPages) {
      this.workshopForm.patchValue({ pageNumber: pageNumber + 1 });
      this.loadWorkshops();
    }
  }

  prevPage(): void {
    const pageNumber = this.workshopForm.get('pageNumber')?.value;
    if (pageNumber > 1) {
      this.workshopForm.patchValue({ pageNumber: pageNumber - 1 });
      this.loadWorkshops();
    }
  }


  changePageSize(): void {
    this.workshopForm.patchValue({ pageNumber: 1 });
    this.loadWorkshops();
  }

  editWorkshop(id: number) {
    this.router.navigate(['/workshop', id]);
  }

}
