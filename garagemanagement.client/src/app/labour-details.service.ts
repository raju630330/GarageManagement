import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

export interface LabourDetail {
  id?: number;
  serialNo: number;
  description: string;
  labourCharges: number;
  outsideLabour: number;
  amount: number;
}

@Injectable({
  providedIn: 'root'
})
export class LabourDetailsService {
  private apiUrl = 'https://localhost:5001/api/LabourDetails';
  private labourChargesTotalSubject = new BehaviorSubject<number>(0);
  private outsideLabourTotalSubject = new BehaviorSubject<number>(0);

  labourChargesTotal$ = this.labourChargesTotalSubject.asObservable();
  outsideLabourTotal$ = this.outsideLabourTotalSubject.asObservable();
  constructor(private http: HttpClient) { }

  getLabourDetails(): Observable<LabourDetail[]> {
    return this.http.get<LabourDetail[]>(this.apiUrl);
  }

  addLabourDetail(detail: LabourDetail): Observable<LabourDetail> {
    return this.http.post<LabourDetail>(this.apiUrl, detail);
  }

  setLabourChargesTotal(value: number) {
    this.labourChargesTotalSubject.next(value);
  }

  setOutsideLabourTotal(value: number) {
    this.outsideLabourTotalSubject.next(value);
  }
}
