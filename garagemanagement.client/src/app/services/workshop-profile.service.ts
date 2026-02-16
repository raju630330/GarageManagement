import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, tap } from 'rxjs';
import { environment } from '../../environments/environment';


@Injectable({
  providedIn: 'root'
})
export class WorkshopProfileService {
  private baseUrl = environment.apiUrl;
  private refreshNeeded = new Subject<void>();
  refreshNeeded$ = this.refreshNeeded.asObservable();
  constructor(private http: HttpClient) { }

  saveProfile(profile: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/workshopProfile/save`, profile);
  }

  //getProfile(id: number): Observable<WorkshopProfile> {
  //  return this.http.get<WorkshopProfile>(`${this.apiUrl}/${id}`);
  //}

  saveBookAppointment(bookingAppointmentdata: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/BookingAppointment/saveBookAppointment`, bookingAppointmentdata).pipe(
      tap(() => this.refreshNeeded.next()));
  }

  getAllAppointments(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/BookingAppointment/getAllAppointments`);
  }

  DeleteAppointment(id: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/BookingAppointment/deleteAppointment/${id}`);
  }

  searchBookingCustomer(query: string) {
    return this.http.get<any[]>(
      `${this.baseUrl}/BookingAppointment/search-booking-customer?query=${query}`
    );
  }
  getCustomerDetails(customerId: number) {
    return this.http.get<any>(
      `${this.baseUrl}/BookingAppointment/getCustomerDetails/${customerId}`
    );
  }
  getBookingById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/BookingAppointment/getBooking/${id}`);
  }

  searchWorkshops(query: string) {
    return this.http.get<any[]>(
      `${this.baseUrl}/WorkshopProfile/searchWorshops?query=${query}`
    );
  }
  searchUsers(query: string) {
    return this.http.get<any[]>(
      `${this.baseUrl}/WorkshopProfile/searchUsers?query=${query}`
    );
  }
  getAllAssignedUsers(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/WorkshopProfile/list`);
  }
  getAssignedUser(id: number): Observable<any> {
    return this.http.get<any>(
      `${this.baseUrl}/WorkshopProfile/getAssignedUser/${id}`
    );
  }
}
