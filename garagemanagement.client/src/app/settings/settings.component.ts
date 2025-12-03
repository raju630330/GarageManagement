import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ROLES } from '../constants/roles.constants';

@Component({
  selector: 'app-settings',
  standalone: false,
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent {
  ROLES = ROLES;
  constructor(private router: Router) { }
  bookappointment() {
    this.router.navigate(['/Calendar']);
  }
  repairOrder() {
    this.router.navigate(['/repair-order']);
  }
  jobcard() {
    this.router.navigate(['/jobcardlist']);
  }
}
