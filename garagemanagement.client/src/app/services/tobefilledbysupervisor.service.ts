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

//@Injectable({
//  providedIn: 'root'
//})
//export class TobeFilledBySupervisorService {

//  baseUrl = environment.apiUrl;
//  private supervisorSubject = new BehaviorSubject<TobeFilledBySupervisor[]>([]);
//  supervisor$ = this.supervisorSubject.asObservable();

//  constructor(private http: HttpClient) { }

//  setSupervisorList(value: TobeFilledBySupervisor[]) {
//    this.supervisorSubject.next(value);
//  }

//  //  GET all records
//  getAllTobeFilledBySupervisor(): Observable<any> {
//    return this.http.get<TobeFilledBySupervisor[]>(`${this.baseUrl}/TobeFilledBySupervisor/TobeFilledBySupervisor`,);
//  }
//  //Create 
//  createTobeFilledBySupervisor(data:any): Observable<any> {
//    return this.http.post(`${this.baseUrl}/TobeFilledBySupervisor/TobeFilledBySupervisor`, data);
//  }
//}
@Injectable({
  providedIn: 'root'
})
export class ToBeFilledBySupervisorService {

  private baseUrl = environment.apiUrl + '/ToBeFilledBySupervisor/savedetails';

  constructor(private http: HttpClient) { }

  // CREATE record
  createSupervisor(data: any): Observable<any> {
    return this.http.post(this.baseUrl, data);
  }

  // GET all records
  getAllSupervisors(): Observable<any> {
    return this.http.get(this.baseUrl);
  }
}
