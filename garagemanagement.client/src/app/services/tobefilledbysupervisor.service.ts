import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface TobeFilledBySupervisor {
  id: number;
  vehicleNumber: string;
  driverName: string;
  supervisorName: string;
  remarks: string;
  status: string;
}

@Injectable({
  providedIn: 'root'
})
export class ToBeFilledBySupervisorService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // CREATE record
  createSupervisor(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/ToBeFilledBySupervisor/savedetails`, data);
  }

}
