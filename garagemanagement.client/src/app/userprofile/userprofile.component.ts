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
  route: string;
  module: string;        // module name
  permission: string;    // e.g., 'V'
  icon?: string;
  emoji?: string;
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
    { name: 'Associated Workshops', route: '/associated-workshops', module: 'Subscription', permission: 'V' },
    { name: 'Activate e-payment now', route: '/activate-epayment', module: 'Subscription', permission: 'V' },
    { name: 'Integrations', route: '/integrations', module: 'Subscription', permission: 'V' },
    { name: 'Templates', route: '/templates', module: 'Templates', permission: 'V' }
  ];

  sidebarTabs: AppTab[] = [
    { name: 'Roles', icon: 'fas fa-user-shield', route: '/roles', module: 'Role', permission: 'V' },
    { name: 'Permissions', icon: 'fas fa-key', route: '/permission', module: 'Permission', permission: 'V' },
    { name: 'Role Permission', icon: 'fas fa-lock', route: '/rolepermission', module: 'RolePermission', permission: 'V' },
    { name: 'Booking Appointment', icon: 'fas fa-calendar-check', route: '/Calendar', module: 'JobCard', permission: 'V' },
    { name: 'Repair Order', icon: 'bi bi-tools', route: '/repair-order', module: 'RepairOrder', permission: 'V' },
    { name: 'Job Cards', emoji: '📝', route: '/jobcardlist', module: 'JobCard', permission: 'V' }
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

    // First, get all modules from backend to build name → ID map
    this.rolePermissionService.getModules().subscribe(modules => {
      this.moduleMap = {};
      modules.forEach(m => this.moduleMap[m.name] = m.id);

      // Prepare observables for topTabs
      const topObs = this.mainTabs.map(tab => {
        const moduleId = this.moduleMap[tab.module];
        if (!moduleId) return null;

        return this.rolePermissionService.getRoleModulePermissions(roleId, moduleId)
          .pipe(filter(perms => perms.includes(tab.permission)));
      }).filter(Boolean) as any[];

      forkJoin(topObs).subscribe(() => {
        this.topTabs = this.mainTabs.filter(tab => {
          const moduleId = this.moduleMap[tab.module];
          return moduleId && topObs.some(obs => true); // All tabs that passed permission
        });
        setTimeout(() => this.checkScroll(), 50);
      });

      // Prepare observables for sidebarTabs
      const sideObs = this.sidebarTabs.map(tab => {
        const moduleId = this.moduleMap[tab.module];
        if (!moduleId) return null;

        return this.rolePermissionService.getRoleModulePermissions(roleId, moduleId)
          .pipe(filter(perms => perms.includes(tab.permission)));
      }).filter(Boolean) as any[];

      forkJoin(sideObs).subscribe(() => {
        this.sidebarTabsFiltered = this.sidebarTabs.filter(tab => {
          const moduleId = this.moduleMap[tab.module];
          return moduleId && sideObs.some(obs => true);
        });
      });
    });
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
