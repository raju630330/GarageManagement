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
    'Full Service',
    'PMS service',
    'Eletric check'
  ];
  allTechnicians: string[] = [
    'Abhilash',
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
      kms: ['', [Validators.required]],
      vehicleInDateTime: ['', Validators.required],
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
      driverPermanent: ['', [Validators.required]],
      roadTest: ['', [Validators.required]],
      serviceType: ['', Validators.required],
      repairCost: [
        '',
        [
          Validators.required,
          Validators.pattern(/^\d{1,6}(\.\d{0,2})?$/)  // max 6 digits before dot, max 2 after
        ]
      ],
      warranty: ['', [Validators.required]],
      expectedDateTime: ['', Validators.required],
      allottedTechnician: ['', Validators.required]
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

    // Allow only numbers and one dot
    input = input.replace(/[^0-9.]/g, '');

    // Split integer and decimal parts
    const parts = input.split('.');

    // Limit integer part to 6 digits
    if (parts[0].length > 6) {
      parts[0] = parts[0].substring(0, 6);
    }

    // Limit decimal part to 2 digits
    if (parts[1]) {
      parts[1] = parts[1].substring(0, 2);
    }

    input = parts.join('.');
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
