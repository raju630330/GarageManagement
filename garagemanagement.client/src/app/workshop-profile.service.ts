import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';


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

  getAllAppointments(): Observable<any[]> {
    return this.http.get<any[]>('http://localhost:5238/api/bookingAppointment/getAllAppointments');
  }

  DeleteAppointment(id: number): Observable<any> {
    return this.http.delete<any>(`http://localhost:5238/api/bookingAppointment/deleteAppointment/${id}`);
  }
}
