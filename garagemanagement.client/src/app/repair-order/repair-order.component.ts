import { Component, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-repair-order',
  standalone: false,
  templateUrl: './repair-order.component.html',
  styleUrls: ['./repair-order.component.css'],
})
export class RepairOrderComponent implements OnInit {
  openPanel: string | null = null;
  readonly panelOpenState = signal(false);
  repairForm!: FormGroup;
  minDateTime!: string;

  makeList: string[] = [
    'Tata Motors',
    'Ashok Leyland',
    'Mahindra',
    'Eicher',
    'Volvo',
    'BharatBenz',
    'Force Motors',
    'Isuzu',
    'Scania',
    'Swaraj Mazda',
  ];

  serviceList: string[] = [
    'Oil Change',
    'Engine Repair',
    'Brake Service',
    'Tire Replacement',
    'Battery Check',
    'Full Service',
    'PMS service',
    'Electric check',
  ];

  allTechnicians: string[] = [
    'Abhilash',
    'Technician A',
    'Technician B',
    'Technician C',
    'Technician D',
    'Technician E',
  ];




  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    // --- Set current min date-time ---
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    this.minDateTime = `${year}-${month}-${day}T${hours}:${minutes}`;

    // --- Main Repair Form ---
    this.repairForm = this.fb.group({
      registrationNumber: [
        '',
        [Validators.required, Validators.pattern(/^[A-Z]{4}[0-9]{6}$/)],
      ],
      vinNumber: ['', [Validators.required, Validators.pattern(/^[0-9]+$/)]],
      kms: ['', [Validators.required]],
      vehicleInDateTime: ['', Validators.required],
      make: ['', Validators.required],
      model: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z0-9 ]+$/),
          Validators.minLength(2),
          Validators.maxLength(40),
        ],
      ],
      driverName: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z ]+$/),
          Validators.minLength(2),
          Validators.maxLength(40),
        ],
      ],
      mobileNumber: [
        '',
        [Validators.required, Validators.pattern(/^\+91[0-9]{10}$/)],
      ],
      vehicleFromSite: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z ]+$/),
          Validators.maxLength(25),
        ],
      ],
      siteInchargeName: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z ]+$/),
          Validators.maxLength(25),
        ],
      ],
      driverPermanent: ['', [Validators.required]],
      roadTest: ['', [Validators.required]],
      serviceType: ['', Validators.required],
      repairCost: [
        '',
        [Validators.required, Validators.pattern(/^\d{1,6}(\.\d{0,2})?$/)],
      ],
      warranty: ['', [Validators.required]],
      expectedDateTime: ['', Validators.required],
      allottedTechnician: ['', Validators.required],

    });


  }

  // --- Registration Input Formatting ---
  onRegistrationInput(event: any) {
    let input = event.target.value.toUpperCase().replace(/[^A-Z0-9]/g, '');
    if (input.length > 10) {
      input = input.substring(0, 10);
    }
    this.repairForm.get('registrationNumber')?.setValue(input, { emitEvent: false });
  }


  // --- Form Submit ---
  onSubmit() {
    if (this.repairForm.valid) {
      console.log(this.repairForm.value);
      alert('✅ Form submitted successfully!');
    } else {
      alert('⚠️ Please fix validation errors before submitting!');
    }
  }

  // --- Repair Cost Input Restriction ---
  onRepairCostInput(event: any) {
    let input = event.target.value;
    input = input.replace(/[^0-9.]/g, '');
    const parts = input.split('.');

    if (parts[0].length > 6) parts[0] = parts[0].substring(0, 6);
    if (parts[1]) parts[1] = parts[1].substring(0, 2);

    input = parts.join('.');
    this.repairForm.get('repairCost')?.setValue(input, { emitEvent: false });
  }

  // --- VIN Only Digits ---
  onVinInput(event: any) {
    let input = event.target.value.replace(/[^0-9]/g, '');
    this.repairForm.get('vinNumber')?.setValue(input, { emitEvent: false });
  }

  // --- Driver Name Input ---
  onDriverNameInput(event: any) {
    let input = event.target.value.replace(/[^a-zA-Z ]/g, '');
    this.repairForm.get('driverName')?.setValue(input, { emitEvent: false });
  }

  // --- Site Incharge Name Input ---
  onSiteInchargeNameInput(event: any) {
    let input = event.target.value.replace(/[^A-Za-z ]/g, '');
    if (input.length > 25) input = input.substring(0, 25);
    this.repairForm.get('siteInchargeName')?.setValue(input, { emitEvent: false });
  }

  // --- Mobile Number Input ---
  onMobileInput(event: any) {
    let input = event.target.value;
    if (!input.startsWith('+91')) {
      input = '+91' + input.replace(/\D/g, '');
    } else {
      input = '+91' + input.slice(3).replace(/\D/g, '');
    }
    if (input.length > 13) input = input.substring(0, 13);
    this.repairForm.get('mobileNumber')?.setValue(input, { emitEvent: false });
  }

  // --- Vehicle From Site Input ---
  onVehicleFromSiteInput(event: any) {
    let input = event.target.value.replace(/[^A-Za-z ]/g, '');
    if (input.length > 25) input = input.substring(0, 25);
    this.repairForm.get('vehicleFromSite')?.setValue(input, { emitEvent: false });
  }

  // --- Getters ---
  get repairCost() {
    return this.repairForm.get('repairCost')!;
  }
  get registrationNumber() {
    return this.repairForm.get('registrationNumber')!;
  }
  get serviceType() {
    return this.repairForm.get('serviceType')!;
  }
  get vinNumber() {
    return this.repairForm.get('vinNumber')!;
  }
  get driverName() {
    return this.repairForm.get('driverName')!;
  }
  get mobileNumber() {
    return this.repairForm.get('mobileNumber')!;
  }
  get vehicleFromSite() {
    return this.repairForm.get('vehicleFromSite')!;
  }
  get siteInchargeName() {
    return this.repairForm.get('siteInchargeName')!;
  }
  get make() {
    return this.repairForm.get('make')!;
  }
  get model() {
    return this.repairForm.get('model')!;
  }
}

