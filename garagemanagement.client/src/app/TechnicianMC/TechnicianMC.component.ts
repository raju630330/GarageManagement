import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RepairOrderService } from '../services/repair-order.service';
import { Subscription } from 'rxjs';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-TechnicianMC',
  templateUrl: './TechnicianMC.component.html',
  styleUrls: ['./TechnicianMC.component.css'],
  standalone:false
})
export class TechnicianMCComponent implements OnInit, OnDestroy {

  repairTForm!: FormGroup;
  isSubmitted = false;
  showSuccess = false;

  repairOrderId: number | null = null;
  private sub!: Subscription;

  checkList = [
    { label: 'Engine Oil Level', control: 'engineOil', status: null, touched: false },
    { label: 'G Oil Level', control: 'gOil', status: null, touched: false },
    { label: 'Housing Oil Level', control: 'housingOil', status: null, touched: false },
    { label: 'Steering Oil Level', control: 'steeringOil', status: null, touched: false },
    { label: 'Clutch Oil Level', control: 'clutchOil', status: null, touched: false },
    { label: 'Other Oil Leakages', control: 'otherLeak', status: null, touched: false },
    { label: 'Battery & Lights Check', control: 'battery', status: null, touched: false },
    { label: 'Tyres & Stephany Condition', control: 'tyres', status: null, touched: false },
    { label: 'General Checks Done', control: 'generalCheck', status: null, touched: false }
  ];

  TMCLeft = this.checkList.slice(0, 5);
  TMCRight = this.checkList.slice(5);

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private repairOrderService: RepairOrderService,
    private alert: AlertService
  ) { }

  ngOnInit(): void {
    this.createForm();

    this.sub = this.repairOrderService.repairOrderId$.subscribe(id => {
      if (!id || id === this.repairOrderId) return;
      this.repairOrderId = id;
      this.loadTechnicianMC(id);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  createForm() {
    this.repairTForm = this.fb.group({
      id:[0],
      remarks: ['', Validators.required],
      technicianSign: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern(/^[A-Za-z ]+$/)]],
      driverSign: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern(/^[A-Za-z ]+$/)]],
      floorSign: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern(/^[A-Za-z ]+$/)]],
      authSign: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(20), Validators.pattern(/^[A-Za-z ]+$/)]],
      confirmation: [false, Validators.requiredTrue]
    });
  }

  // ✅ OK / NOT OK behaves like radio
  setStatus(item: any, value: 'ok' | 'notOk') {
    item.status = value;
    item.touched = true;
  }

  checklistValid(): boolean {
    return this.checkList.every(x => x.status !== null);
  }

  // 🔹 Load existing data
  loadTechnicianMC(repairOrderId: number) {
    this.http.get<any>(`https://localhost:7086/api/TechnicianMC/${repairOrderId}`)
      .subscribe(res => {
        if (!res) return;

        this.repairTForm.patchValue({
          id: res.id,
          remarks: res.remarks,
          technicianSign: res.technicianSign,
          driverSign: res.driverSign,
          floorSign: res.floorSign,
          authSign: res.authSign,
          confirmation: true
        });

        res.checkList?.forEach((saved: any) => {
          const item = this.checkList.find(x => x.control === saved.control);
          if (item) item.status = saved.status;
        });
      });
  }

  onSubmit() {
    this.isSubmitted = true;
    this.checkList.forEach(x => x.touched = true);

    if (this.repairTForm.invalid || !this.checklistValid()) {
      this.repairTForm.markAllAsTouched();
      return;
    }

    if (!this.repairOrderId) {
      alert('Please save Repair Order first');
      return;
    }

    const payload = {
      repairOrderId: this.repairOrderId,
      ...this.repairTForm.value,
      checkList: this.checkList.map(({ label, control, status }) => ({ label, control, status }))
    };

    this.http.post<{ message: string, id?: number }>(
      'https://localhost:7086/api/TechnicianMC/save',
      payload
    ).subscribe({
      next: (res) => {
        this.alert.showInfo(res.message || "Saved Successfully", () => {
          if (res.id) {
            this.repairTForm.patchValue({ id: res.id });
          }
        });
      },
      error: err => console.error('Save Error:', err)
    });
  }

  // getters
  get remarks() { return this.repairTForm.get('remarks'); }
  get confirmation() { return this.repairTForm.get('confirmation'); }
  get technicianSign() { return this.repairTForm.get('technicianSign'); }
  get driverSign() { return this.repairTForm.get('driverSign'); }
  get floorSign() { return this.repairTForm.get('floorSign'); }
  get authSign() { return this.repairTForm.get('authSign'); }
}
