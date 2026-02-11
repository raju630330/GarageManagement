import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit
} from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { RolePermissionService, PermissionModule } from '../services/role-permission.service';
import { filter, forkJoin } from 'rxjs';

interface AppTab {
  name: string;
  icon?: string;
  emoji?: string;
  route?: string;     // Make optional with ?
  module?: string;    // Make optional with ?
  permission: string;
  children?: AppTab[];
  expanded?: boolean;
}

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css'],
  standalone: false
})
export class UserprofileComponent implements OnInit, AfterViewInit {

  user: any = null;
  isLoggedIn = false;
  role: string | null = null;

  topTabs: AppTab[] = [];
  sidebarTabsFiltered: AppTab[] = [];

  showLeftArrow = false;
  showRightArrow = false;
  showUserPopup = false;
  isSidebarOpen = false;

  @ViewChild('tabScroll') tabScroll!: ElementRef;

  moduleMap: Record<string, number> = {}; // Map module name → moduleId

  /* ---------- ALL TABS CONFIG (STATIC) ---------- */
  mainTabs: AppTab[] = [
    { name: 'Profile', route: '/profile', module: 'Profile', permission: 'V' },
    { name: 'Workshop', route: '/workshop', module: 'Workshop', permission: 'V' },
    { name: 'Users', route: '/users', module: 'User', permission: 'V' },
    { name: 'MMVY', route: '/mmvy', module: 'MMVY', permission: 'V' },
    { name: 'Settings', route: '/settings', module: 'Settings', permission: 'V' },
    { name: 'Subscription', route: '/subscription', module: 'Subscription', permission: 'V' },
    { name: 'Terms & Conditions', route: '/terms', module: 'Subscription', permission: 'V' },
    { name: 'Reminders', route: '/reminders', module: 'Subscription', permission: 'V' },
    { name: 'Associated Workshops', route: '/associatedworkshops', module: 'Subscription', permission: 'V' },
    { name: 'Activate e-payment now', route: '/activate-epayment', module: 'Subscription', permission: 'V' },
    { name: 'Integrations', route: '/integrations', module: 'Subscription', permission: 'V' },
    { name: 'Templates', route: '/templates', module: 'Templates', permission: 'V' }
  ];

  sidebarTabs: AppTab[] = [
    {
      name: 'Admin', icon: 'bi bi-shield-lock', module : 'Admin', permission: 'V', children: [
        { name: 'Roles', icon: 'fas fa-user-shield', route: '/roles', module: 'Role', permission: 'V' },
        { name: 'Role Permission', icon: 'fas fa-lock', route: '/rolepermission', module: 'RolePermission', permission: 'V' },
        { name: 'Workshop', icon: 'fas fa-tools', route: '/workshop', module: 'RolePermission', permission: 'V' },
        { name: 'Assign User', icon: 'fas fa-user-plus', route: '/assignuser', module: 'RolePermission', permission: 'V' },
      ]
    },

    { name: 'Booking Appointment', icon: 'fas fa-calendar-check', route: '/Calendar', module: 'BookAppointment', permission: 'V' },
    { name: 'Repair Order', icon: 'bi bi-tools', route: '/repair-order', module: 'RepairOrder', permission: 'V' }, //assignuser
    {
      name: 'Jobcards', icon: 'bi bi-clipboard-check', module: 'Jobcards', permission: 'V', children: [
        { name: 'Add', icon: 'bi bi-clipboard-check', route: '/newjobcard', module: 'JobCardAdd', permission: 'V' },
        { name: 'List', icon: 'bi bi-journal-text', route: '/jobcardlist', module: 'JobCardList', permission: 'V' }
      ]
    },

  ];


  constructor(
    private authService: AuthService,
    private router: Router,
    private rolePermissionService: RolePermissionService
  ) { }

  ngOnInit(): void {
    this.authService.loggedIn$.subscribe(isLogged => {
      this.isLoggedIn = isLogged;
      if (!isLogged) {
        this.resetState();
        return;
      }

      this.authService.getUserProfile().subscribe(profile => {
        this.user = profile;
        this.role = profile.role;

        const roleId = this.authService.getRoleId();
        if (!roleId) return;

        this.loadTabsByPermission(roleId);
      });
    });

    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe(() => {
        if (window.innerWidth < 768) this.isSidebarOpen = false;
      });
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.checkScroll(), 200);
  }

  /* ---------- CORE PERMISSION LOGIC ---------- */
  private loadTabsByPermission(roleId: number) {
    this.topTabs = [];
    this.sidebarTabsFiltered = [];

    this.rolePermissionService.getRolePermissions(roleId).subscribe((permissions) => {

      const hasPermission = (permissionCode: string, moduleName?: string): boolean => {
        // Parent tabs without module = no permission (hidden)
        if (!moduleName) return false;

        return permissions.some(p =>
          p.moduleName === moduleName &&
          this.mapPermissionIdToCode(p.permissionId) === permissionCode
        );
      };

      // Swap order in filter calls too
      this.topTabs = this.mainTabs.filter(tab => hasPermission(tab.permission, tab.module));
      this.sidebarTabsFiltered = this.sidebarTabs.filter(tab => hasPermission(tab.permission, tab.module));

      setTimeout(() => this.checkScroll(), 50);
    });
  }


  /** Map numeric permissionId to 'V', 'A', 'E', 'D' */
  private mapPermissionIdToCode(permissionId?: number): string {
    switch (permissionId) {
      case 1: return 'V';
      case 2: return 'A';
      case 3: return 'E';
      case 4: return 'D';
      default: return '';
    }
  }

  /* ---------- UI HELPERS ---------- */
  scrollTabs(amount: number) {
    const el = this.tabScroll?.nativeElement;
    if (!el) return;
    el.scrollBy({ left: amount, behavior: 'smooth' });
    setTimeout(() => this.checkScroll(), 300);
  }

  checkScroll() {
    const el = this.tabScroll?.nativeElement;
    if (!el) return;
    this.showLeftArrow = el.scrollLeft > 3;
    this.showRightArrow = el.scrollLeft + el.clientWidth < el.scrollWidth - 3;
  }

  toggleUserPopup() { this.showUserPopup = !this.showUserPopup; }
  toggleSidebar() { this.isSidebarOpen = !this.isSidebarOpen; }
  closePopup() { this.showUserPopup = false; }
  toggleTab(tab: AppTab) {
    tab.expanded = !tab.expanded;
  }

  logout() {
    this.authService.logout();
    this.resetState();
    this.router.navigate(['/login']);
  }

  private resetState() {
    this.user = null;
    this.role = null;
    this.topTabs = [];
    this.sidebarTabsFiltered = [];
    this.isSidebarOpen = false;
    this.showUserPopup = false;
  }
}
