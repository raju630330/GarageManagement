import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AlertService } from '../services/alert.service';
import { WorkshopProfileService } from '../services/workshop-profile.service';
import { environment } from '../../environments/environment';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-associatedworkshops',
  templateUrl: './associatedworkshops.component.html',
  styleUrl: './associatedworkshops.component.css',
  standalone: false
})
export class AssociatedworkshopsComponent implements OnInit {

  workshops: any[] = [];
  isLoading = false;

  private baseUrl = environment.apiUrl;

  constructor(
    private http: HttpClient,
    private alert: AlertService,
    private auth: AuthService
  ) { }

  ngOnInit(): void {
    this.loadWorkshops();
  }

  loadWorkshops() {

    const userId = this.auth.getUserId();   // ✅ Getting from token/session

    if (!userId) {
      this.alert.showError("User not logged in.");
      return;
    }

    this.isLoading = true;

    this.http.get<any[]>(`${this.baseUrl}/WorkshopProfile/GetWorkshopsByUser?id=${userId}`)
      .subscribe({
        next: (res) => {
          this.workshops = res;
          this.isLoading = false;

          if (res.length === 0) {
            this.alert.showWarning("No workshops assigned to you.");
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.alert.showError("Failed to load workshops.");
          console.error(err);
        }
      });
  }
}
