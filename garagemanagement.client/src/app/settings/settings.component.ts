import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-settings',
  standalone: false,
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.css'
})
export class SettingsComponent {

  constructor(private router: Router) { }
  bookappointment() {
    this.router.navigate(['/dashaboard']);

  }
}
