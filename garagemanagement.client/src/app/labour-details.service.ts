import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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

  constructor(private http: HttpClient) { }

  getLabourDetails(): Observable<LabourDetail[]> {
    return this.http.get<LabourDetail[]>(this.apiUrl);
  }

  addLabourDetail(detail: LabourDetail): Observable<LabourDetail> {
    return this.http.post<LabourDetail>(this.apiUrl, detail);
  }
}
