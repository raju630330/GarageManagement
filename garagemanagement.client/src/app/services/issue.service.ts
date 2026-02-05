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
export interface IssuedItem {
  estimationItemId: number;
  partNo: string;
  partName: string;
  brand: string;
  requestedQty: number;
  issueQty: number;
  returnQty: number;
  unitPrice: number;
  issuedDate?: string;
  issuedTo?: string;
  issuedId: string;
}

export interface IssueItemDto {
  estimationId: number;
  issueQty: number;
}

export interface IssuePartsRequestDto {
  jobCardId: number;
  items: IssueItemDto[];
}

export interface BaseResultDto {
  isSuccess: boolean;
  message: string;
}
@Injectable({ providedIn: 'root' })
export class IssueService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getPendingIssues(jobCardId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/Issue/pending-items/${jobCardId}`);
  }

  getIssuedIssues(jobCardId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/Issue/issued-items/${jobCardId}`);
  }

  getReturnedIssues(jobCardId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/returned-items/${jobCardId}`);
  }
  issueParts(request: IssuePartsRequestDto): Observable<any> {
    return this.http.post<any>(
      `${this.baseUrl}/Issue/issue-parts`,
      request
    );
  }
}
