import { Injectable } from '@angular/core';
import { JobCard } from '../models/job-card';

@Injectable({
  providedIn: 'root'
})
export class JobCardService {

  getJobCards(): JobCard[] {
    return [
      {
        refNo: 'REF001',
        jobCardNo: 'JC001',
        regNo: 'TS09AB1234',
        invoiceNo: 'INV001',
        serviceType: 'Repair',
        vehicle: 'Honda City',
        status: 'Work-In-Progress',
        customerName: 'Ravi',
        mobileNo: '9876543210',
        arrivalDate: '2025-12-01',
        arrivalTime: '10:00 AM',
        insuranceCorporate: 'Insurance',
        claimNo: 'CLM001',
        estDeliveryDate: '2025-12-05',
        accidentDate: '2025-11-30'
      },
      {
        refNo: 'REF002',
        jobCardNo: 'JC002',
        regNo: 'AP16CD5678',
        invoiceNo: '',
        serviceType: 'Maintenance',
        vehicle: 'Suzuki Swift',
        status: 'Ready for Delivery',
        customerName: 'Sita',
        mobileNo: '9876501234',
        arrivalDate: '2025-12-02',
        arrivalTime: '11:30 AM',
      }
    ];
  }
}
