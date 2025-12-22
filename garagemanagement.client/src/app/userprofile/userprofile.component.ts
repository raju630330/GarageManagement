import { Component, OnInit, ViewChild, ElementRef, AfterViewInit, HostListener } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { filter } from 'rxjs/operators';

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
  showLeftArrow = false;
  showRightArrow = false;
  showUserPopup = false;
  isSidebarOpen = false;

  @ViewChild('tabScroll') tabScroll!: ElementRef;

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

  sidebarTabs = [
    { name: 'Repair Order', icon:'bi bi-tools me-2', route: '/repair-order', roles: ['Admin', 'Manager', 'Supervisor'] },
    { name: 'Job Cards', icon: 'fas fa-pen-to-square', route: '/jobcardlist', roles: ['Admin', 'Manager', 'Supervisor'] },
  ];

  filteredSidebarTabs: any[] = [];

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.loggedIn$.subscribe(status => {
      this.isLoggedIn = status;

      if (status) {
        this.authService.getUserProfile().subscribe(profile => {
          this.user = profile;
          this.role = profile.role;
          this.filteredTabs = this.mainTabs.filter(tab => tab.roles.includes(this.role ?? ''));
          this.filteredSidebarTabs = this.sidebarTabs.filter(tab => tab.roles.includes(this.role ?? ''));
          setTimeout(() => this.checkScroll(), 50);
        });
      } else {
        this.user = null;
        this.role = null;
      }
    });

    // Auto-close sidebar on route change
    this.router.events.pipe(filter(e => e instanceof NavigationEnd))
      .subscribe(() => {
        if (window.innerWidth < 768) {
          this.isSidebarOpen = false;
        }
      });
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.checkScroll(), 200);
  }

  scrollTabs(amount: number) {
    const el = this.tabScroll?.nativeElement;
    if (!el) return;

    el.scrollBy({ left: amount, behavior: 'smooth' });
    setTimeout(() => this.checkScroll(), 300);
  }

  checkScroll() {
    const el = this.tabScroll?.nativeElement;
    if (!el) return;

    const tolerance = 3;
    this.showLeftArrow = el.scrollLeft > tolerance;
    const atRightEnd = el.scrollLeft + el.clientWidth >= (el.scrollWidth - tolerance);
    this.showRightArrow = !atRightEnd;
  }

  toggleUserPopup() {
    this.showUserPopup = !this.showUserPopup;
  }

  closePopup() {
    this.showUserPopup = false;
  }

  logout() {
    this.authService.logout();
    this.closePopup();
    this.isSidebarOpen = false;
    this.router.navigate(['/login']);
    this.user = null;
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

/*  // Optional: Close sidebar on click outside
  @HostListener('document:click', ['$event'])
  onClick(event: any) {
    if (!event.target.closest('.sidebar') && !event.target.closest('.fixed-menu-btn')) {
      this.isSidebarOpen = false;
    }
  }*/
}
