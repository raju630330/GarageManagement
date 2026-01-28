import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { environment } from '../../environments/environment';

export interface InventoryAccessory {
  label: string;
  checked: boolean;
}

export interface InventoryDto {
  id: number;
  repairOrderId: number;
  accessories: InventoryAccessory[];
}

export interface InventoryResponse {
  repairOrderId: number;
  accessories: InventoryAccessory[];
  id: number;
}
@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  // 🔹 SAVE INVENTORY (Create / Update)
  saveInventory(payload: InventoryDto): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Inventory/save`, payload);
  }

  // 🔹 GET INVENTORY BY REPAIR ORDER ID
  getInventoryByRepairOrderId(repairOrderId: number): Observable<InventoryResponse> {
    return this.http.get<InventoryResponse>(
      `${this.baseUrl}/Inventory/get/${repairOrderId}`
    );
  }
}
