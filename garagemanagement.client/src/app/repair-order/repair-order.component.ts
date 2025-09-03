import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-repair-order',
  standalone: true,
  templateUrl: './repair-order.component.html',
  styleUrls: ['./repair-order.component.css'],
  imports: [CommonModule, ReactiveFormsModule]
})
export class RepairOrderComponent implements OnInit {
  repairForm!: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.repairForm = this.fb.group({
      registrationNumber: ['', Validators.required],
      vinNumber: [''],
      kms: [''],
      date: [''],
      make: [''],
      driverName: [''],
      vehicleFromSite: [''],
      siteInchargeName: [''],
      driverPermanent: [''],
      roadTest: ['No'],
      serviceType: [''],
      repairCost: [''],
      warranty: ['No'],
      expectedDateTime: ['']
    });
  }

  onSubmit() {
    if (this.repairForm.valid) {
      console.log(this.repairForm.value);
      alert('Form Submitted Successfully!');
    }
  }
}
