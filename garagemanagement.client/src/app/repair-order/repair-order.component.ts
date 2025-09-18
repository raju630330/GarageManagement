import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-repair-order',
  standalone: false,
  templateUrl: './repair-order.component.html',
  styleUrls: ['./repair-order.component.css'],
})
export class RepairOrderComponent implements OnInit {
  repairForm!: FormGroup;
  readonly panelOpenState = signal(false);
  repairTForm!: FormGroup;



  checkList = [
    { label: 'Engine Oil Level', control: 'engineOil', options: ['OK'] },
    { label: 'G Oil Level', control: 'gOil', options: ['OK'] },
    { label: 'Housing Oil Level', control: 'housingOil', options: ['OK'] },
    { label: 'Steering Oil Level', control: 'steeringOil', options: ['OK'] },
    { label: 'Clutch Oil Level', control: 'clutchOil', options: ['OK'] },
    { label: 'Other Oil Leakages', control: 'otherLeak', options: ['YES'] },
    { label: 'Battery & Lights Check', control: 'battery', options: ['OK'] },
    { label: 'Tyres & Stephany Condition', control: 'tyres', options: ['OK'] },
    { label: 'General Checks Done', control: 'generalCheck', options: ['YES'] },
  ];


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


    this.repairTForm = this.fb.group({
      engineOilOk: [false], engineOilNotOk: [false],
      gOilOk: [false], gOilNotOk: [false],
      housingOilOk: [false], housingOilNotOk: [false],
      steeringOilOk: [false], steeringOilNotOk: [false],
      clutchOilOk: [false], clutchOilNotOk: [false],
      otherLeakYes: [false], otherLeakNo: [false],
      batteryOk: [false], batteryNotOk: [false],
      tyresOk: [false], tyresNotOk: [false],
      generalCheckYes: [false], generalCheckNo: [false],

      remarks: [''],

      technicianSign: [''],
      driverSign: [''],
      floorSign: [''],
      authSign: ['']
    });
  }

  onSubmit() {
    if (this.repairForm.valid) {
      console.log(this.repairForm.value);
      alert('✅ Form submitted successfully!');
    } else {
      alert('⚠️ Please fix validation errors before submitting!');
    }
  }
  
}
