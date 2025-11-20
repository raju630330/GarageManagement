import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../environments/environment';
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
  private baseUrl = environment.apiUrl;
  private labourChargesTotalSubject = new BehaviorSubject<number>(0);
  private outsideLabourTotalSubject = new BehaviorSubject<number>(0);

  labourChargesTotal$ = this.labourChargesTotalSubject.asObservable();
  outsideLabourTotal$ = this.outsideLabourTotalSubject.asObservable();
  constructor(private http: HttpClient) { }

  getLabourDetails(): Observable<LabourDetail[]> {
    return this.http.get<LabourDetail[]>(this.baseUrl);
  }

  addLabourDetails(details: LabourDetail[]): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/LabourDetails/CreateLabourDetails`, details);
  }

  setLabourChargesTotal(value: number) {
    this.labourChargesTotalSubject.next(value);
  }

  setOutsideLabourTotal(value: number) {
    this.outsideLabourTotalSubject.next(value);
  }
}
