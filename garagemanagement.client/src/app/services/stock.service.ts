import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

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

@Injectable({
  providedIn: 'root'
})
export class StockService {

  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // 🔹 Get stock list with filter & search
  // stockType: ALL | IN | OUT
  getStockList(
    stockType: 'ALL' | 'IN' | 'OUT' = 'ALL',
    search: string = ''
  ): Observable<StockItem[]> {

    let params = new HttpParams()
      .set('stockType', stockType)
      .set('search', search);

    return this.http.get<StockItem[]>(
      `${this.baseUrl}/stock/list`,
      { params }
    );
  }

  // 🔹 Get stock statistics (dashboard cards)
  getStockStats(): Observable<StockStats> {
    return this.http.get<StockStats>(
      `${this.baseUrl}/stock/stats`
    );
  }
}
