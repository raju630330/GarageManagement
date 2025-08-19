import { Component } from '@angular/core';
import { RepairOrderComponent } from './repair-order/repair-order.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RepairOrderComponent], 
  template: `<app-repair-order></app-repair-order>`
})
export class AppComponent { }
