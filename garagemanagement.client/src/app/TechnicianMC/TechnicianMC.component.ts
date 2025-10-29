import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-TechnicianMC',
  standalone: false,
  templateUrl: './TechnicianMC.component.html',
  styleUrls: ['./TechnicianMC.component.css']
})
export class TechnicianMCComponent implements OnInit {
  repairTForm!: FormGroup;
  isSubmitted = false;
  showSuccess = false;

  checkList = [
    { label: 'Engine Oil Level', control: 'engineOil', status: null },
    { label: 'G Oil Level', control: 'gOil', status: null },
    { label: 'Housing Oil Level', control: 'housingOil', status: null },
    { label: 'Steering Oil Level', control: 'steeringOil', status: null },
    { label: 'Clutch Oil Level', control: 'clutchOil', status: null },
    { label: 'Other Oil Leakages', control: 'otherLeak', status: null },
    { label: 'Battery & Lights Check', control: 'battery', status: null },
    { label: 'Tyres & Stephany Condition', control: 'tyres', status: null },
    { label: 'General Checks Done', control: 'generalCheck', status: null },
  ];

  TMCLeft = this.checkList.slice(0, 5);
  TMCRight = this.checkList.slice(5, 9);

  constructor(private fb: FormBuilder, private http: HttpClient) { }

  ngOnInit(): void {
    this.repairTForm = this.fb.group({
      remarks: ['', Validators.required],
      technicianSign: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern(/^[A-Za-z ]+$/)]],
      driverSign: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern(/^[A-Za-z ]+$/)]],
      floorSign: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern(/^[A-Za-z ]+$/)]],
      authSign: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern(/^[A-Za-z ]+$/)]],
      confirmation: [false, Validators.requiredTrue]
    });
  }

  setStatus(item: any, newStatus: 'ok' | 'notOk') {
    item.status = newStatus;
  }

  checklistValid(): boolean {
    return this.checkList.every(item => item.status != null);
  }

  onSubmit() {
    this.isSubmitted = true;

    if (this.repairTForm.invalid || !this.checklistValid()) {
      this.repairTForm.markAllAsTouched();
      return;
    }

    const formData = {
      ...this.repairTForm.value,
      checkList: this.checkList
    };

    this.http.post('https://localhost:7086/api/TechnicianMC/save', formData)
      .subscribe({
        next: () => {
          this.showSuccess = true;
          setTimeout(() => {
            this.showSuccess = false;
            window.location.reload();
          }, 2000);
        },
        error: err => console.error('Save Error:', err)
      });
  }

  get remarks() { return this.repairTForm.get('remarks'); }
  get confirmation() { return this.repairTForm.get('confirmation'); }
  get technicianSign() { return this.repairTForm.get('technicianSign'); }
  get driverSign() { return this.repairTForm.get('driverSign'); }
  get floorSign() { return this.repairTForm.get('floorSign'); }
  get authSign() { return this.repairTForm.get('authSign'); }
}
