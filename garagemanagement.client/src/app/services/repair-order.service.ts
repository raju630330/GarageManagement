import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RepairOrderService {

  private baseUrl = environment.apiUrl;
  private repairOrderIdSubject = new BehaviorSubject<number | null>(null);
  repairOrderId$ = this.repairOrderIdSubject.asObservable();

  constructor(private http: HttpClient) { }

  // --- Create a new repair order ---
  createRepairOrder(order: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/RepairOrders/createRepairOrder`, order);
  }

  // --- Set the repairOrderId in the service ---
  setRepairOrderId(id: number) {
    this.repairOrderIdSubject.next(id);
  }

  // --- Get the current repairOrderId synchronously ---
  getRepairOrderId(): number | null {
    return this.repairOrderIdSubject.getValue();
  }

  // --- Optional: Get all repair orders ---
  getOrders(): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}`);
  }
  getRepairOrderByBooking(bookingId: number) {
    return this.http.get<any>(
      `${this.baseUrl}/RepairOrders/by-booking/${bookingId}`
    );
  }
  searchBookingAppointment(query: string) {
    return this.http.get<any[]>(
      `${this.baseUrl}/RepairOrders/search-booking-appointment?query=${query}`
    );
  }
  searchRepairOrder(query: string) {
    return this.http.get<any[]>(
      `${this.baseUrl}/RepairOrders/search-repair-order?query=${query}`
    );
  }
  getRepairOrderByRepairId(repairOrderId: number) {
    return this.http.get<any>(
      `${this.baseUrl}/RepairOrders/by-repairOrderId/${repairOrderId}`
    );
  }
}
