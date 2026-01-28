import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Jobobservationdetail {
  technicianVoice: string,
  supervisorInstructions: string,
  actionTaken: string,
  startTime: string,
  endTime: string
}

@Injectable({
  providedIn: 'root'
})
export class AdditionalJobObserveDetailsService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  createAdditionalJobObserveDetails(details: Jobobservationdetail[]): Observable<any> {
    return this.http.post(`${this.baseUrl}/AdditionalJobObserveDetail/additionaljobobservedetails`, details);
  }
  getAdditionalJobObserveDetails(repairOrderId: number) {
    return this.http.get<any[]>(
      `${this.baseUrl}/AdditionalJobObserveDetail/getadditionaljobobservedetails/${repairOrderId}`
    );
  }
}
