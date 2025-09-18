import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-userprofile',
  standalone: false,
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent implements OnInit {

  user: any = null;
  isLoggedIn = false;
  role: string | null = null;


  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.loggedIn$.subscribe(status => {
      this.isLoggedIn = status;

    if(status) {
      this.authService.getUserProfile().subscribe({
        next: (profile) => {
          console.log("API user response:", profile);

          this.user = profile;
          this.role = profile.role;
          
        },
        error: (err) => console.error("Profile fetch failed", err)
      });
    } else {
      this.user = null;
      this.role = null;
    }
  });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
    this.user = null;
  }


  book() {
    this.router.navigate(['/dashaboard']);
  }

  viewProfile() {
    this.router.navigate(['//workshop']);

  }

  goToRepairOrder() {
    this.router.navigate(['/repair-order']);

  }
  goToSparePartDetails() {
    this.router.navigate(['/spare-part']);

  }
  goToJobObserveDetails() {
    this.router.navigate(['/jobobservedetails']);

  }
  goToSupervisorDetails() {
    this.router.navigate(['/garagemanagement']);

  }
  goToLabourDetails() {
    this.router.navigate(['/labour-details']);

  }
  goToInventory() {
    this.router.navigate(['/inventory']);

  }
   
}


