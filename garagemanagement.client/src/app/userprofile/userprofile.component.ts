import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy
} from '@angular/core';

import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { RolePermissionService } from '../services/role-permission.service';

import { filter, forkJoin, Subject, takeUntil } from 'rxjs';

interface AppTab {
  name: string;
  icon?: string;
  emoji?: string;
  route?: string;
  module?: string;
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
export class UserprofileComponent implements OnInit, AfterViewInit, OnDestroy {

  user: any = null;
  role: string | null = null;
  isLoggedIn = false;

  topTabs: AppTab[] = [];
  sidebarTabsFiltered: AppTab[] = [];

  showLeftArrow = false;
  showRightArrow = false;

  showUserPopup = false;
  isSidebarOpen = false;

  permissions: any[] = [];

  remainingTime = 0;
  formattedTime = '';

  private destroy$ = new Subject<void>();

  @ViewChild('tabScroll') tabScroll!: ElementRef;

  constructor(
    private authService: AuthService,
    private router: Router,
    private rolePermissionService: RolePermissionService
  ) { }

  /* ---------- MAIN TABS ---------- */

  mainTabs: AppTab[] = [

    { name: 'Workshop', route: '/workshop', module: 'Workshop', permission: 'V' },
    { name: 'Users', route: '/userlist', module: 'User', permission: 'V' },
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

  /* ---------- SIDEBAR ---------- */

  sidebarTabs: AppTab[] = [

    {
      name: 'Dashboard',
      icon: 'bi bi-speedometer2',
      route: '/dashboard',
      module: 'Dashboard',
      permission: 'V'
    },

    {
      name: 'Admin',
      icon: 'bi bi-shield-lock',
      module: 'Admin',
      permission: 'V',
      children: [

        { name: 'Roles', icon: 'fas fa-user-shield', route: '/roles', module: 'Role', permission: 'V' },

        { name: 'Role Permission', icon: 'fas fa-lock', route: '/rolepermission', module: 'RolePermission', permission: 'V' },

        { name: 'Users', icon: 'fas fa-user', route: '/userlist', module: 'User', permission: 'V' },

        {
          name: 'Workshop',
          icon: 'fas fa-tools',
          module: 'RolePermission',
          permission: 'V',
          children: [
            { name: 'Add', icon: 'fas fa-plus-circle', route: '/workshop', module: 'RolePermission', permission: 'V' },
            { name: 'List', icon: 'fas fa-list-ul', route: '/workshoplist', module: 'RolePermission', permission: 'V' }
          ]
        },

        { name: 'Assign User', icon: 'fas fa-user-plus', route: '/assignuser', module: 'RolePermission', permission: 'V' }

      ]
    },

    { name: 'Booking Appointment', icon: 'fas fa-calendar-check', route: '/Calendar', module: 'BookAppointment', permission: 'V' },

    { name: 'Repair Order', icon: 'bi bi-tools', route: '/repair-order', module: 'RepairOrder', permission: 'V' },

    {
      name: 'Job Cards',
      icon: 'bi bi-clipboard-check',
      module: 'Jobcards',
      permission: 'V',
      children: [

        { name: 'Add', icon: 'bi bi-plus-circle', route: '/newjobcard', module: 'JobCardAdd', permission: 'V' },

        { name: 'List', icon: 'bi bi-journal-text', route: '/jobcardlist', module: 'JobCardList', permission: 'V' }

      ]
    },

    {
      name: 'Parts',
      icon: 'bi bi-box-seam',
      module: 'Parts',
      permission: 'V',
      children: [
        { name: 'Stock', icon: 'bi bi-boxes', route: '/stock', module: 'Stock', permission: 'V' }
      ]
    }

  ];

  /* ---------- INIT ---------- */

  ngOnInit(): void {

    this.authService.loggedIn$
      .pipe(takeUntil(this.destroy$))
      .subscribe(isLogged => {

        this.isLoggedIn = isLogged;

        if (!isLogged) {
          this.resetState();
          return;
        }

        this.authService.getUserProfile()
          .pipe(takeUntil(this.destroy$))
          .subscribe(profile => {

            this.user = profile;
            this.role = profile.role;

            const roleId = this.authService.getRoleId();

            if (roleId) {
              this.loadTabsByPermission(roleId);
            }

          });

      });

    this.authService.remainingTime$
      .pipe(takeUntil(this.destroy$))
      .subscribe(seconds => {

        if (seconds == null) return;

        this.remainingTime = seconds;
        this.formattedTime = this.formatTime(seconds);

      });

    this.router.events
      .pipe(
        filter(e => e instanceof NavigationEnd),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {

        if (window.innerWidth < 768) {
          this.isSidebarOpen = false;
        }

      });

  }

  ngAfterViewInit(): void {
    setTimeout(() => this.checkScroll(), 200);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /* ---------- PERMISSION LOGIC ---------- */

  private loadTabsByPermission(roleId: number) {

    forkJoin([
      this.rolePermissionService.getRolePermissions(roleId),
      this.rolePermissionService.getPermissions()
    ])
      .pipe(takeUntil(this.destroy$))
      .subscribe(([rolePermissions, permissions]) => {

        const permissionMap = new Map(
          permissions.map((p: any) => [p.id, p.name])
        );

        const hasPermission = (permissionCode: string, moduleName?: string) => {

          if (!moduleName) return false;

          return rolePermissions.some((p: any) =>
            p.moduleName === moduleName &&
            permissionMap.get(p.permissionId) === permissionCode
          );

        };

        this.topTabs = this.mainTabs.filter(tab =>
          hasPermission(tab.permission, tab.module)
        );

        this.sidebarTabsFiltered = this.filterTabs(this.sidebarTabs, hasPermission);

        setTimeout(() => this.checkScroll(), 100);

      });

  }

  /* ---------- RECURSIVE MENU FILTER ---------- */

  private filterTabs(tabs: AppTab[], hasPermission: any): AppTab[] {

    return tabs
      .map(tab => {

        const newTab = { ...tab };

        if (newTab.children) {
          newTab.children = this.filterTabs(newTab.children, hasPermission);
        }

        const allowed =
          hasPermission(newTab.permission, newTab.module) ||
          (newTab.children && newTab.children.length > 0);

        return allowed ? newTab : null;

      })
      .filter(Boolean) as AppTab[];

  }

  /* ---------- TAB SCROLL ---------- */

  scrollTabs(amount: number) {

    const el = this.tabScroll?.nativeElement;

    if (!el) return;

    el.scrollBy({ left: amount, behavior: 'smooth' });

    setTimeout(() => this.checkScroll(), 300);

  }

  checkScroll() {

    const el = this.tabScroll?.nativeElement;

    if (!el) return;

    this.showLeftArrow = el.scrollLeft > 0;

    this.showRightArrow =
      Math.ceil(el.scrollLeft + el.clientWidth) < el.scrollWidth;

  }

  /* ---------- UI ACTIONS ---------- */

  toggleUserPopup() {
    this.showUserPopup = !this.showUserPopup;
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  closePopup() {
    this.showUserPopup = false;
  }

  toggleTab(tab: AppTab) {
    tab.expanded = !tab.expanded;
  }

  logout() {

    this.authService.logout();

    this.resetState();

    this.router.navigate(['/login']);

  }

  /* ---------- HELPERS ---------- */

  private resetState() {

    this.user = null;
    this.role = null;

    this.topTabs = [];
    this.sidebarTabsFiltered = [];

    this.isSidebarOpen = false;
    this.showUserPopup = false;

  }

  private formatTime(seconds: number): string {

    if (!seconds || seconds <= 0) return '0:00';

    const minutes = Math.floor(seconds / 60);
    const secs = seconds % 60;

    return `${minutes}:${secs.toString().padStart(2, '0')}`;

  }

}
