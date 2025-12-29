import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  private loading = new BehaviorSubject<boolean>(false);
  loading$ = this.loading.asObservable();

  private requestCount = 0;

  show() {
    this.requestCount++;
    this.loading.next(true);
  }

  hide() {
    this.requestCount--;

    if (this.requestCount <= 0) {
      this.requestCount = 0;
      this.loading.next(false);
    }
  }
}


/*Scenario: Interceptor + PDF iframe

API call starts → Interceptor calls loader.show()

requestCount = 1 → loader visible

PDF processing starts → you manually call loader.show()

requestCount = 2 → loader stays visible

API returns → Interceptor calls loader.hide()

requestCount = 1 → loader still visible because PDF is still loading

PDF iframe finishes loading → you call loader.hide()

requestCount = 0 → loader disappears*/
