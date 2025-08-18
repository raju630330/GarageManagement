import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { WorkshopProfile } from './models/workshop-profile';


@Injectable({
  providedIn: 'root'
})
export class WorkshopProfileService {

  constructor(private http: HttpClient) { }

  saveProfile(profile: any): Observable<any> {
    return this.http.post('http://localhost:5238/api/workshopProfile/save', profile);
  }

  //getProfile(id: number): Observable<WorkshopProfile> {
  //  return this.http.get<WorkshopProfile>(`${this.apiUrl}/${id}`);
  //}

  saveBookAppointment(bookingAppointmentdata: any): Observable<any> {
    return this.http.post('http://localhost:5238/api/bookingAppointment/saveBookAppointment', bookingAppointmentdata);
  }
}
