import { Component } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-workshop',
  standalone: false,
  templateUrl: './workshop.component.html',
  styleUrl: './workshop.component.css'
})
export class WorkshopComponent {
  constructor(private authService: AuthService) { }
  get isAdmin(): boolean {
    return this.authService.isAdmin();
  }
}
