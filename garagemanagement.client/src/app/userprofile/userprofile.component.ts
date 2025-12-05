import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-userprofile',
  standalone: false,
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent implements OnInit, AfterViewInit {

  user: any = null;
  isLoggedIn = false;
  role: string | null = null;
  filteredTabs: any = [];

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    // Subscribe to login state
    this.authService.loggedIn$.subscribe(status => {
      this.isLoggedIn = status;

      if (status) {
        this.authService.getUserProfile().subscribe({
          next: (profile) => {
            this.user = profile;
            this.role = profile.role;

            // Filter menu tabs by role
            this.filteredTabs = this.mainTabs.filter(tab =>
              tab.roles.includes(this.role ?? '')
            );

            setTimeout(() => this.checkScroll(), 50);
          }
        });
      } else {
        this.user = null;
        this.role = null;
      }
    });
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.checkScroll(), 200);
  }

  logout() {
    this.authService.logout();
    this.showUserPopup = false;
    this.router.navigate(['/']);
    this.user = null;
  }

  mainTabs = [
    { name: 'Profile', route: '/profile', roles: ['Admin', 'Manager'] },
    { name: 'Workshop', route: '/workshop', roles: ['Admin', 'Manager', 'Supervisor'] },
    { name: 'Users', route: '/users', roles: ['Admin'] },
    { name: 'MMVY', route: '/mmvy', roles: ['Admin', 'Manager', 'Supervisor'] },
    { name: 'Settings', route: '/settings', roles: ['Admin', 'Manager', 'Supervisor', 'Driver'] },
    { name: 'Subscription', route: '/subscription', roles: ['Admin', 'Manager'] },
    { name: 'Terms & Conditions', route: '/terms', roles: ['Admin', 'Driver'] },
    { name: 'Reminders', route: '/reminders', roles: ['Admin', 'Manager'] },
    { name: 'Associated Workshops', route: '/associated-workshops', roles: ['Admin'] },
    { name: 'Activate e-payment now', route: '/activate-epayment', roles: ['Admin', 'Manager'] },
    { name: 'Integrations', route: '/integrations', roles: ['Admin'] },
    { name: 'Templates', route: '/templates', roles: ['Admin', 'Manager'] }
  ];

  showLeftArrow = false;
  showRightArrow = false;

  @ViewChild('tabScroll') tabScroll!: ElementRef;

  scrollTabs(amount: number) {
    const el = this.tabScroll?.nativeElement;
    if (!el) return;

    el.scrollBy({ left: amount, behavior: 'smooth' });

    setTimeout(() => this.checkScroll(), 300);
  }

  checkScroll() {
    const el = this.tabScroll?.nativeElement;
    if (!el) return;

    // Universal 2–3px tolerance (Chrome needs this)
    const tolerance = 3;

    this.showLeftArrow = el.scrollLeft > tolerance;

    // Hide right arrow only when REALLY at end
    const atRightEnd = el.scrollLeft + el.clientWidth >= (el.scrollWidth - tolerance);

    this.showRightArrow = !atRightEnd;
  }
  showUserPopup = false;

  toggleUserPopup() {
    this.showUserPopup = !this.showUserPopup;
  }

  // FIX – close popup when clicking Login/Signup
  closePopup() {
    this.showUserPopup = false;
  }

}
