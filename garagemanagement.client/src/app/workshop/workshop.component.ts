
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-workshop',
  standalone: false,
  templateUrl: './workshop.component.html',
  styleUrls: ['./workshop.component.css']
})
export class WorkshopComponent implements OnInit {

  workshopForm!: FormGroup;

  constructor(private fb: FormBuilder, private http: HttpClient, private alert: AlertService) { }

  ngOnInit() {
    this.workshopForm = this.fb.group({
      workshopName: ['', Validators.required],
      ownerName: ['', Validators.required],
      ownerMobileNo: ['', Validators.required],
      emailID: ['', [Validators.email]],
      contactPerson: [''],
      contactNo: [''],
      landline: [''],
      inBusinessSince: [''],
      avgVehicleInflowPerMonth: [''],
      noOfEmployees: [''],
      dealerCode: [''],
      isGdprAccepted: [false, Validators.requiredTrue],

      address: this.fb.group({
        flatNo: [''],
        street: [''],
        location: [''],
        city: ['', Validators.required],
        state: ['', Validators.required],
        stateCode: [''],
        country: ['India'],
        pincode: ['', Validators.required],
        landmark: [''],
        branchAddress: ['']
      }),

      timing: this.fb.group({
        startTime: ['', Validators.required],
        endTime: ['', Validators.required]
      }),

      businessConfig: this.fb.group({
        websiteLink: [''],
        googleReviewLink: [''],
        externalIntegrationUrl: [''],
        gstin: [''],
        msme: [''],
        sac: [''],
        sacPercentage: [''],
        invoiceCaption: [''],
        invoiceHeader: [''],
        defaultServiceType: ['']
      }),

      serviceIds: this.fb.array([]),
      workingDays: this.fb.array([]),
      paymentModeIds: this.fb.array([]),
      media: this.fb.array([])
    });
  }

  submit() {
    if (this.workshopForm.invalid) {
      this.workshopForm.markAllAsTouched();
      this.alert.showError("Please fill all required fields!");
      return;
    }
    console.log(this.workshopForm.value);
    this.http.post('https://localhost:7086/api/WorkshopProfile/createworkshop', this.workshopForm.value)
      .subscribe({
        next: res => this.alert.showInfo('Workshop Created Successfully', () => { window.location.reload() }),
        error: err => alert('Something went wrong')
      });
  }
}
