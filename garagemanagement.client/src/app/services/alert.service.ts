import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  alert$ = new Subject<{
    message: string,
    type?: 'success' | 'error' | 'warning',
    confirmFn?: () => void,
    isConfirm?: boolean // true = Yes/No, false = OK only
  }>();

  // Base method
  open(message: string, type: 'success' | 'error' | 'warning' = 'success', confirmFn?: () => void, isConfirm: boolean = false) {
    this.alert$.next({ message, type, confirmFn, isConfirm });
  }

  // Success message (auto-close)
  showSuccess(message: string) {
    this.open(message, 'success');
  }

  // Error message
  showError(message: string) {
    this.open(message, 'error');
  }

  // Warning message
  showWarning(message: string) {
    this.open(message, 'warning');
  }

  // Confirm dialog (Yes/No)
  confirm(message: string, yesCallback: () => void) {
    this.open(message, 'warning', yesCallback, true);
  }

  // Info message (OK only)
  showInfo(message: string, okCallback?: () => void) {
    this.open(message, 'success', okCallback, false);
  }
}
