import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
export interface PendingIssueItem {
  estimationItemId: number;
  inStock?: boolean;
  partNo: string;
  partName: string;
  brand?: string;
  qtyOnHand?: number;
  requestedQty?: number;
  issuedQty?: number;
  pendingQty?: number;
  sellingPrice: number;
  issueQty?: number;
  issuedTo?: string;
}

@Injectable({ providedIn: 'root' })
export class IssueService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getPendingIssues(jobCardId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/Issue/pending-items/${jobCardId}`);
  }

  getIssuedIssues(jobCardId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/issued-items/${jobCardId}`);
  }

  getReturnedIssues(jobCardId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/returned-items/${jobCardId}`);
  }
}
