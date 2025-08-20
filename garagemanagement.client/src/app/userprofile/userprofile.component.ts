import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-userprofile',
  standalone: false,
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent {

  users: any = null;

  constructor(private authService: AuthService, private router: Router) { }

  get isLoggedIn() {
    return this.authService.isLoggedIn();
  }
   
  get role() {
    return this.authService.role;
  }

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    //if (!this.authService.isLoggedIn()) {
    //  return;
    //}

    this.authService.getUserProfile().subscribe({
      next: res => {
        this.users = res;
      },
      error: err => {
        console.error('Error fetching user details:', err);
      }
    });
    
  }
 
  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
    this.users = null;
  }


  book() {
    this.router.navigate(['/dashaboard']);
  }

  viewProfile() {
    this.router.navigate(['//workshop']);

  }

   
}


