import { Injectable } from '@angular/core';
import { JobCard, JobCardDto } from '../models/job-card';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class JobCardService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  getJobCards(): JobCard[] {
    const jobCards: JobCard[] = [];
    const vehicles = ['Honda City', 'Suzuki Swift', 'Toyota Innova', 'Hyundai Verna', 'Maruti Alto', 'Toyota Fortuner', 'Hyundai Creta', 'Maruti Swift', 'Honda Amaze', 'Toyota Yaris', 'Hyundai i20', 'Maruti Baleno', 'Hyundai Venue', 'Honda WR-V', 'Maruti Dzire', 'Toyota Corolla', 'Hyundai Santro', 'Maruti Celerio'];
    const serviceTypes = ['Running Repair', 'Maintenance'];
    const statuses = ['Request for Estimation', 'Estimate', 'Spares Pending', 'Work-In-Progress', 'Ready for Delivery', 'Invoice', 'Delivered', 'In Workshop', 'Estimation Rejected', 'Ins Approval Pending', 'Approval Pending'];
    const customers = ['Srinivasa Garage Main', 'Srinivasa Garage Tcs', 'Srinivasa Garage Madhapur'];
    const insurance = ['SRT', 'HDFC', 'ICICI', 'Bajaj'];

    for (let i = 1; i <= 100; i++) {
      const refNo = 'REF' + i.toString().padStart(3, '0');
      const jobCardNo = `SRT-J${(2992 + i).toString().padStart(6, '0')}`;
      const regNo = `TS07UE${8530 + i}`;
      const invoiceNo = `SRT-I${2214 + i}`;
      const serviceType = serviceTypes[Math.floor(Math.random() * serviceTypes.length)];
      const vehicle = vehicles[Math.floor(Math.random() * vehicles.length)];
      const status = statuses[Math.floor(Math.random() * statuses.length)];
      const customerName = customers[Math.floor(Math.random() * customers.length)];
      const mobileNo = '******' + (3200 + i);
      const insuranceCorporate = insurance[Math.floor(Math.random() * insurance.length)];
      const claimNo = `CLM${i.toString().padStart(3, '0')}`;

      const arrivalDate = new Date(2025, 11, 1 + (i % 30)); // Dec 1-30
      const estDeliveryDate = new Date(arrivalDate);
      estDeliveryDate.setDate(arrivalDate.getDate() + Math.floor(Math.random() * 7) + 1); // 1-7 days later
      const accidentDate = new Date(arrivalDate);
      accidentDate.setDate(arrivalDate.getDate() - Math.floor(Math.random() * 3)); // 0-2 days before

      const formatDate = (d: Date) => d.toISOString().split('T')[0];
      const formatTime = () => `${Math.floor(Math.random() * 12 + 8)}:${Math.floor(Math.random() / 2 * 60).toString().padStart(2, '0')} ${Math.random() > 0.5 ? 'AM' : 'PM'}`;

      jobCards.push({
        refNo,
        jobCardNo,
        regNo,
        invoiceNo,
        serviceType,
        vehicle,
        status,
        customerName,
        mobileNo,
        insuranceCorporate,
        claimNo,
        arrivalDate: formatDate(arrivalDate),
        arrivalTime: formatTime(),
        estDeliveryDate: formatDate(estDeliveryDate),
        accidentDate: formatDate(accidentDate)
      });
    }

    return jobCards;
  }

  saveJobCard(dto: JobCardDto): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/NewJobCard/save-jobcard`, dto);
  }

  searchRegistration(query: string) {
    return this.http.get<any[]>(`${this.baseUrl}/NewJobCard/search-registration?query=${query}`);
  }

  getJobCardDetails(id: number) {
    return this.http.get<any>(`${this.baseUrl}/NewJobCard/get-jobcard/${id}`);
  }
  private previousJobCardsMock = [
    {id:1, regNo: "REG001", jobCardNo: "SRT-J003210", date: "2025-02-05T10:30:00", status: "Delivered" },
    { id: 1, regNo: "REG002", jobCardNo: "SRT-J003145", date: "2025-01-18T15:10:00", status: "Invoice" },
    { id: 1, regNo: "REG003", jobCardNo: "SRT-J003099", date: "2025-01-02T11:45:00", status: "Ready for Delivery" },

    { id: 1, regNo: "REG004", jobCardNo: "SRT-J002980", date: "2024-12-22T09:20:00", status: "Work-In-Progress" },
    { id: 1, regNo: "REG005", jobCardNo: "SRT-J002910", date: "2024-12-01T14:00:00", status: "Spares Pending" }
  ];

  // 🔹 GET Previous Job Cards by Registration Number
  getPreviousJobCards(id: number) {
    const filtered = this.previousJobCardsMock.filter(x => x.id === id);
    return filtered; // return array (mock)
  }


  getEstimationDetails(jobCardId: number) {
    return this.http.get<any>(
      `${this.baseUrl}/NewJobCard/get-estimation/${jobCardId}`
    );
  }


  saveJobCardEstimation(model: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/NewJobCard/save-estimation`, model);
  }
}
