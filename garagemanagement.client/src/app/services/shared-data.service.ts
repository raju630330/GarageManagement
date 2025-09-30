import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedDataService {
  private totalAmount: number = 0;
  private readonly STORAGE_KEY = 'totalAmount';

  setTotal(amount: number): void {
    console.log('Setting totalAmount:', amount);
    this.totalAmount = amount;

    // ✅ Store in localStorage as well
    localStorage.setItem(this.STORAGE_KEY, amount.toString());
  }

  getTotal(): number {
    // Load from localStorage every time
    const storedValue = localStorage.getItem(this.STORAGE_KEY);
    if (storedValue !== null) {
      this.totalAmount = +storedValue;
      console.log('Loaded totalAmount from localStorage:', this.totalAmount);
    }

    console.log('Getting totalAmount:', this.totalAmount);
    return this.totalAmount;
  }

}
