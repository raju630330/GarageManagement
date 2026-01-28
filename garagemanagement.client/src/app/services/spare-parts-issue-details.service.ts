import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface SparePart {
  description: string;
  partNumber: string;
  make: string;
  unitCost: number;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class SparePartsIssueDetailsService {
  private baseUrl = environment.apiUrl;
  private partsTotalSubject = new BehaviorSubject(0);
  partsTotal$ = this.partsTotalSubject.asObservable();
  constructor(private http: HttpClient) { }

  // For total calculation
  setPartsTotal(value: number) {
    this.partsTotalSubject.next(value);
  }

  getPartsTotal(): number {
    return this.partsTotalSubject.value;
  }

  createSpareParts(parts: SparePart[]): Observable<any> {
    return this.http.post(`${this.baseUrl}/SparePartsIssueDetails/CreateSpareParts` , parts);
  }
  getSpareParts(repairOrderId: number) {
    return this.http.get<any>(
      `${this.baseUrl}/SparePartsIssueDetails/${repairOrderId}`
    );
  }
}
