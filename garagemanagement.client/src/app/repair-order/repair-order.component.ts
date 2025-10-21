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
    'Swaraj Mazda'
  ];
  serviceList: string[] = [
    'Oil Change',
    'Engine Repair',
    'Brake Service',
    'Tire Replacement',
    'Battery Check',
    'Full Service'
  ];
  allTechnicians: string[] = [
    'Technician A',
    'Technician B',
    'Technician C',
    'Technician D',
    'Technician E'
  ];





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
    const now = new Date();
    const year = now.getFullYear();
    const month = String(now.getMonth() + 1).padStart(2, '0');
    const day = String(now.getDate()).padStart(2, '0');
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    this.minDateTime = `${year}-${month}-${day}T${hours}:${minutes}`;
    this.repairForm = this.fb.group({
      registrationNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Z]{4}[0-9]{6}$/)
        ]
      ],
      vinNumber: ['', [Validators.required, Validators.pattern(/^[0-9]+$/)]],
      kms: [''],
      date: ['', Validators.required],
      make: ['', Validators.required],
      model: [
        '',
        [
          Validators.required,
          Validators.pattern(/^[A-Za-z0-9 ]+$/),
          Validators.minLength(2),
          Validators.maxLength(40)
        ]
      ],
      driverName: ['', [
        Validators.required,
        Validators.pattern(/^[A-Za-z ]+$/),
        Validators.minLength(2),
        Validators.maxLength(40)
      ]],
      mobileNumber: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\+91[0-9]{10}$/)
        ]
      ],
      vehicleFromSite: ['', [
        Validators.required,
        Validators.pattern(/^[A-Za-z ]+$/),
        Validators.maxLength(25)
      ]],
      siteInchargeName: ['', [
        Validators.required,
        Validators.pattern(/^[A-Za-z ]+$/),
        Validators.maxLength(25)
      ]],
      driverPermanent: [''],
      roadTest: [''],
      serviceType: ['', Validators.required],
      repairCost: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d+(\.\d{1,2})?$/) // digits with up to 2 decimals
        ]
      ],
      warranty: [''],
      expectedDateTime: [this.minDateTime, Validators.required]
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
 


  onRegistrationInput(event: any) {
    let input = event.target.value.toUpperCase().replace(/[^A-Z0-9]/g, '');
    if (input.length > 10) {
      input = input.substring(0, 10);
    }
    this.repairForm.get('registrationNumber')?.setValue(input, { emitEvent: false });
  }

  onSubmit() {
    if (this.repairForm.valid) {
      console.log(this.repairForm.value);
      alert('✅ Form submitted successfully!');
    } else {
      alert('⚠️ Please fix validation errors before submitting!');
    }
  }

  onRepairCostInput(event: any) {
    let input = event.target.value;

    // allow only digits and max one decimal
    input = input.replace(/[^0-9.]/g, '');

    // allow only two decimal places
    if (input.includes('.')) {
      const parts = input.split('.');
      parts[1] = parts[1].substring(0, 2);
      input = parts.join('.');
    }

    this.repairForm.get('repairCost')?.setValue(input, { emitEvent: false });
  }
  get repairCost() {
    return this.repairForm.get('repairCost')!;
  }


  get registrationNumber() {
    return this.repairForm.get('registrationNumber')!;
  }
  

  get serviceType() {
    return this.repairForm.get('serviceType')!;
  }

  // Handle VIN input – allow only digits
  onVinInput(event: any) {
    let input = event.target.value.replace(/[^0-9]/g, ''); // remove non-numeric
    this.repairForm.get('vinNumber')?.setValue(input, { emitEvent: false });
  }

  // Getter for easy access in template
  get vinNumber() {
    return this.repairForm.get('vinNumber')!;
  }
  onDriverNameInput(event: any) {
    let input = event.target.value.replace(/[^a-zA-Z ]/g, ''); // allow letters & spaces only
    this.repairForm.get('driverName')?.setValue(input, { emitEvent: false });
  }
  onSiteInchargeNameInput(event: any) {
    let input = event.target.value.replace(/[^A-Za-z ]/g, ''); // allow letters & spaces only
    if (input.length > 25) {
      input = input.substring(0, 25);
    }
    this.repairForm.get('siteInchargeName')?.setValue(input, { emitEvent: false });
  }

  get driverName() {
    return this.repairForm.get('driverName')!;
  }
  onMobileInput(event: any) {
    let input = event.target.value;

    // Automatically add +91 if not typed
    if (!input.startsWith('+91')) {
      input = '+91' + input.replace(/\D/g, ''); // remove non-digits
    } else {
      input = '+91' + input.slice(3).replace(/\D/g, '');
    }

    if (input.length > 13) {
      input = input.substring(0, 13); // +91 + 10 digits
    }

    this.repairForm.get('mobileNumber')?.setValue(input, { emitEvent: false });
  }
  get mobileNumber() {
    return this.repairForm.get('mobileNumber')!;
  }
  onVehicleFromSiteInput(event: any) {
    let input = event.target.value.replace(/[^A-Za-z ]/g, ''); // allow letters and spaces only
    if (input.length > 25) {
      input = input.substring(0, 25);
    }
    this.repairForm.get('vehicleFromSite')?.setValue(input, { emitEvent: false });
  }
  get siteInchargeName() {
    return this.repairForm.get('siteInchargeName')!;
  }
  

  get vehicleFromSite() {
    return this.repairForm.get('vehicleFromSite')!;
  }

  get make() {
    return this.repairForm.get('make')!;
  }
  get model() {
    return this.repairForm.get('model')!;
  }


}
