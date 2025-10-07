import { Component } from '@angular/core';


@Component({
  selector: 'app-garage-management',
  standalone: false,
  templateUrl: './garage-management.component.html',
  styleUrl: './garage-management.component.css'
})
export class GarageManagementComponent {
  rows = new Array(4);
  hours = Array.from({ length: 12 }, (_, i) => this.pad(i + 1));  // ["01", "02", ..., "12"]

  startHour = '12';
  startMinute = '00';
  startAMPM = 'AM';

  pad(n: number): string {
    return n < 10 ? '0' + n : n.toString();
  }

  formatTime(hour: string, minute: number | string, ampm: string): string {
    const minStr = this.pad(Number(minute));
    return `${hour}:${minStr} ${ampm}`;
  }
}
