import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

// ── Stock ─────────────────────────────────────────────────────────────────────

export interface StockItem {
  partNo: string;
  partName: string;
  brand: string;
  category: string;
  qtyOnHand: number;
  avgPurchasePrice: number;
  avgSellingPrice: number;
  taxType: string;
  taxPercent: number;
  taxAmount: number;
  rackNo: string;
  ageing: number;
  barcode: string;
}

export interface StockStats {
  uniquePartNos: number;
  totalStockItems: number;
  stockValue: number;
}

export interface PagedResult<T> {
  totalRecords: number;
  page: number;
  pageSize: number;
  items: T[];
}

// ── Purchase Orders ───────────────────────────────────────────────────────────

export interface PurchaseOrderListItem {
  orderId: number;
  orderNo: string;
  orderDate: string;    // ISO string — re-parsed from repo's dd-MM-yyyy
  supplierName: string;
  regNo: string;
  jobCardNo: string;
  paymentType: string;
  stockType: string;
  /** PENDING | SHIPMENT | CLOSED | CANCELLED */
  status: string;
  totalAmount: number;    // mapped from repo's orderValue
  itemCount: number;    // mapped from repo's orderedParts
  // Extra inward-tracking fields from the repo
  orderedParts: number;
  inwardedParts: number;
  pendingParts: number;
}

export interface PurchaseOrderItem {
  partId: number;
  partName: string;
  partNo: string;
  brand: string;
  hsnCode: string;
  taxPercent: number;
  qty: number;
  unitPrice: number;
  discount: number;
  taxAmount: number;
  totalPurchasePrice: number;
  serviceType: string;
  remarks: string;
  sellerInfo: string;
  inwardedQty: number;
}

export interface PurchaseOrderDetail {
  orderId: number;
  orderNo: string;
  orderDate: string;
  supplierName: string;
  supplierPhone: string;
  regNo: string;
  jobCardNo: string;
  paymentType: string;
  stockType: string;
  remarks: string;
  status: string;
  items: PurchaseOrderItem[];
}

export interface PurchaseOrderFilters {
  search?: string;
  /** All | PENDING | SHIPMENT | CLOSED | CANCELLED */
  status?: string;
  fromDate?: string;
  toDate?: string;
  page?: number;
  pageSize?: number;
}

@Injectable({ providedIn: 'root' })
export class StockService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // ── Stock ─────────────────────────────────────────────────────────────────

  getStockList(
    stockType: 'ALL' | 'IN' | 'OUT',
    search: string,
    page: number,
    pageSize: number
  ): Observable<PagedResult<StockItem>> {
    const params = new HttpParams()
      .set('stockType', stockType)
      .set('search', search)
      .set('page', page)
      .set('pageSize', pageSize);
    return this.http.get<PagedResult<StockItem>>(
      `${this.baseUrl}/stock/list`, { params }
    );
  }

  getStockStats(): Observable<StockStats> {
    return this.http.get<StockStats>(`${this.baseUrl}/stock/stats`);
  }

  searchstock(query: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/stock/search?query=${query}`);
  }

  getPartById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/stock/get/${id}`);
  }

  // ── Purchase Orders ───────────────────────────────────────────────────────

  getPurchaseOrders(
    filters: PurchaseOrderFilters = {}
  ): Observable<PagedResult<PurchaseOrderListItem>> {
    const params = new HttpParams()
      .set('search', filters.search ?? '')
      .set('status', filters.status ?? 'All')
      .set('fromDate', filters.fromDate ?? '')
      .set('toDate', filters.toDate ?? '')
      .set('page', filters.page ?? 1)
      .set('pageSize', filters.pageSize ?? 10);

    return this.http.get<PagedResult<PurchaseOrderListItem>>(
      `${this.baseUrl}/purchaseorder`, { params }
    );
  }

  getPurchaseOrderById(orderId: number): Observable<PurchaseOrderDetail> {
    return this.http.get<PurchaseOrderDetail>(
      `${this.baseUrl}/purchaseorder/${orderId}`
    );
  }

  updatePurchaseOrderStatus(orderId: number, status: string): Observable<any> {
    return this.http.put(
      `${this.baseUrl}/purchaseorder/${orderId}/status`, { status }
    );
  }
}
