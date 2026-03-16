import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit,
  OnDestroy,
  ChangeDetectorRef,
  ChangeDetectionStrategy
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
  standalone: false,
  changeDetection: ChangeDetectionStrategy.Default
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

  // ── Idle config ───────────────────────────────────────────────────────
  readonly idleTimeout = 120_000;  // ms of inactivity before popup (1 min)
  readonly countdownFrom = 30;      // seconds before auto-logout

  // ── Idle state ────────────────────────────────────────────────────────
  showIdlePopup = false;
  idleCountdown = this.countdownFrom;

  private idleTimer: any = null;
  private countdownTimer: any = null;
  private listenersAdded = false;

  private readonly onActivity = () => this.resetIdleTimer();
  private readonly activityEvents = ['click', 'keydown', 'touchstart', 'scroll', 'mousemove'];

  constructor(
    private authService: AuthService,
    private router: Router,
    private rolePermissionService: RolePermissionService,
    private cdr: ChangeDetectorRef
  ) { }

  /* ─────────────── MAIN TABS ──────────────────────────────────────────── */

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

  /* ─────────────── SIDEBAR ────────────────────────────────────────────── */

  sidebarTabs: AppTab[] = [
    {
      name: 'Dashboard', icon: 'bi bi-speedometer2',
      route: '/dashboard', module: 'Dashboard', permission: 'V'
    },
    {
      name: 'Admin', icon: 'bi bi-shield-lock', module: 'Admin', permission: 'V',
      children: [
        { name: 'Roles', icon: 'fas fa-user-shield', route: '/roles', module: 'Role', permission: 'V' },
        { name: 'Role Permission', icon: 'fas fa-lock', route: '/rolepermission', module: 'RolePermission', permission: 'V' },
        { name: 'Users', icon: 'fas fa-user', route: '/userlist', module: 'User', permission: 'V' },
        {
          name: 'Workshop', icon: 'fas fa-tools', module: 'RolePermission', permission: 'V',
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
      name: 'Job Cards', icon: 'bi bi-clipboard-check', module: 'Jobcards', permission: 'V',
      children: [
        { name: 'Add', icon: 'bi bi-plus-circle', route: '/newjobcard', module: 'JobCardAdd', permission: 'V' },
        { name: 'List', icon: 'bi bi-journal-text', route: '/jobcardlist', module: 'JobCardList', permission: 'V' }
      ]
    },
    {
      name: 'Parts', icon: 'bi bi-box-seam', module: 'Parts', permission: 'V',
      children: [
        { name: 'Stock', icon: 'bi bi-boxes', route: '/stock', module: 'Stock', permission: 'V' },
        { name: 'Inward', icon: 'bi bi-box-arrow-in-down', route: '/inward', module: 'Inward', permission: 'V' }
      ]
    }
  ];

  /* ─────────────── LIFECYCLE ──────────────────────────────────────────── */

  ngOnInit(): void {

    this.authService.loggedIn$
      .pipe(takeUntil(this.destroy$))
      .subscribe(isLogged => {
        this.isLoggedIn = isLogged;

        if (!isLogged) {
          this.stopIdleWatcher();
          this.resetState();
          return;
        }

        this.registerActivityListeners();
        this.resetIdleTimer();

        this.authService.getUserProfile()
          .pipe(takeUntil(this.destroy$))
          .subscribe(profile => {
            this.user = profile;
            this.role = profile.role;
            const roleId = this.authService.getRoleId();
            if (roleId) this.loadTabsByPermission(roleId);
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
      .pipe(filter(e => e instanceof NavigationEnd), takeUntil(this.destroy$))
      .subscribe(() => {
        if (window.innerWidth < 768) this.isSidebarOpen = false;
      });
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.checkScroll(), 200);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.removeActivityListeners();
    this.stopIdleWatcher();
  }

  /* ─────────────── IDLE TIMER ─────────────────────────────────────────── */

  private registerActivityListeners(): void {
    if (this.listenersAdded) return;    // Guard: never double-register
    this.activityEvents.forEach(evt =>
      window.addEventListener(evt, this.onActivity, { passive: true })
    );
    this.listenersAdded = true;
  }

  private removeActivityListeners(): void {
    this.activityEvents.forEach(evt =>
      window.removeEventListener(evt, this.onActivity)
    );
    this.listenersAdded = false;
  }

  resetIdleTimer(): void {
    if (this.showIdlePopup) return;     // Popup showing — let countdown run

    if (this.idleTimer) {
      clearTimeout(this.idleTimer);
      this.idleTimer = null;
    }

    this.idleTimer = setTimeout(() => {
      this.showIdlePopup = true;
      this.idleCountdown = this.countdownFrom;
      // Force Angular to re-render — the setTimeout callback may run
      // outside zone depending on how zone.js is configured in this app
      this.cdr.detectChanges();
      this.startCountdown();
    }, this.idleTimeout);
  }

  private startCountdown(): void {
    if (this.countdownTimer) {
      clearInterval(this.countdownTimer);
      this.countdownTimer = null;
    }

    this.countdownTimer = setInterval(() => {
      this.idleCountdown--;
      // Force re-render on every tick so countdown number updates in template
      this.cdr.detectChanges();

      if (this.idleCountdown <= 0) {
        clearInterval(this.countdownTimer);
        this.countdownTimer = null;
        this.logout();
      }
    }, 1000);
  }

  private stopIdleWatcher(): void {
    if (this.idleTimer) { clearTimeout(this.idleTimer); this.idleTimer = null; }
    if (this.countdownTimer) { clearInterval(this.countdownTimer); this.countdownTimer = null; }
    this.showIdlePopup = false;
  }

  /* ─────────────── POPUP ACTIONS ──────────────────────────────────────── */

  stayLoggedIn(): void {
    this.showIdlePopup = false;
    if (this.countdownTimer) { clearInterval(this.countdownTimer); this.countdownTimer = null; }
    this.resetIdleTimer();
  }

  logoutFromPopup(): void {
    if (this.countdownTimer) { clearInterval(this.countdownTimer); this.countdownTimer = null; }
    this.logout();
  }

  /* ─────────────── PERMISSION LOADING ────────────────────────────────── */

  private loadTabsByPermission(roleId: number): void {
    forkJoin([
      this.rolePermissionService.getRolePermissions(roleId),
      this.rolePermissionService.getPermissions()
    ])
      .pipe(takeUntil(this.destroy$))
      .subscribe(([rolePermissions, permissions]) => {

        const permissionMap = new Map(permissions.map((p: any) => [p.id, p.name]));

        const hasPermission = (code: string, mod?: string): boolean => {
          if (!mod) return false;
          return rolePermissions.some((p: any) =>
            p.moduleName === mod && permissionMap.get(p.permissionId) === code
          );
        };

        this.topTabs = this.mainTabs.filter(t => hasPermission(t.permission, t.module));
        this.sidebarTabsFiltered = this.filterTabs(this.sidebarTabs, hasPermission);
        setTimeout(() => this.checkScroll(), 100);
      });
  }

  /* ─────────────── RECURSIVE MENU FILTER ─────────────────────────────── */

  private filterTabs(tabs: AppTab[], hp: (c: string, m?: string) => boolean): AppTab[] {
    return tabs
      .map(tab => {
        const t = { ...tab };
        if (t.children) t.children = this.filterTabs(t.children, hp);
        return (hp(t.permission, t.module) || (t.children && t.children.length > 0)) ? t : null;
      })
      .filter(Boolean) as AppTab[];
  }

  /* ─────────────── TAB SCROLL ─────────────────────────────────────────── */

  scrollTabs(amount: number): void {
    const el = this.tabScroll?.nativeElement;
    if (!el) return;
    el.scrollBy({ left: amount, behavior: 'smooth' });
    setTimeout(() => this.checkScroll(), 300);
  }

  checkScroll(): void {
    const el = this.tabScroll?.nativeElement;
    if (!el) return;
    this.showLeftArrow = el.scrollLeft > 0;
    this.showRightArrow = Math.ceil(el.scrollLeft + el.clientWidth) < el.scrollWidth;
  }

  /* ─────────────── UI ACTIONS ─────────────────────────────────────────── */

  toggleUserPopup(): void { this.showUserPopup = !this.showUserPopup; }
  toggleSidebar(): void { this.isSidebarOpen = !this.isSidebarOpen; }
  closePopup(): void { this.showUserPopup = false; }
  toggleTab(tab: AppTab): void { tab.expanded = !tab.expanded; }

  logout(): void {
    this.stopIdleWatcher();
    this.removeActivityListeners();
    this.authService.logout();
    this.resetState();
    this.router.navigate(['/login']);
  }

  /* ─────────────── HELPERS ────────────────────────────────────────────── */

  private resetState(): void {
    this.user = null;
    this.role = null;
    this.topTabs = [];
    this.sidebarTabsFiltered = [];
    this.isSidebarOpen = false;
    this.showUserPopup = false;
    this.showIdlePopup = false;
    this.idleCountdown = this.countdownFrom;
  }

  private formatTime(seconds: number): string {
    if (!seconds || seconds <= 0) return '0:00';
    const m = Math.floor(seconds / 60);
    const s = seconds % 60;
    return `${m}:${s.toString().padStart(2, '0')}`;
  }
}
