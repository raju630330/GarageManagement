import { Component } from '@angular/core';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-global-alert',
  standalone: false,
  templateUrl: './global-alert.component.html',
  styleUrls: ['./global-alert.component.css'],
})
export class GlobalAlertComponent {
  show = false;
  message = '';
  type: 'success' | 'error' | 'warning' = 'success';
  confirmFn: (() => void) | null = null;
  isConfirm = false;

  constructor(private alertService: AlertService) {
    this.alertService.alert$.subscribe(a => {
      this.message = a.message;
      this.type = a.type || 'success';
      this.confirmFn = a.confirmFn || null;
      this.isConfirm = !!a.isConfirm;
      this.show = true;
    });
  }

  close() {
    this.show = false;
  }

  yes() {
    if (this.confirmFn) this.confirmFn();
    this.close();
  }
}
