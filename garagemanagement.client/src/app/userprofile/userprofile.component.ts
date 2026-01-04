import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit
} from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { filter } from 'rxjs/operators';
import { RolePermissionService } from '../services/role-permission.service';

/* ---------- Interfaces (VERY IMPORTANT) ---------- */
interface MainTab {
  name: string;
  route: string;
  roles: string[];
}

interface SidebarTab {
  name: string;
  icon: string;
  route: string;
  module: string;
  permission: string;
}

@Component({
  selector: 'app-userprofile',
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css'],
  standalone : false
})
export class UserprofileComponent implements OnInit, AfterViewInit {

  user: any = null;
  isLoggedIn = false;
  role: string | null = null;

  filteredTabs: MainTab[] = [];
  filteredSidebarTabs: SidebarTab[] = [];

  showLeftArrow = false;
  showRightArrow = false;
  showUserPopup = false;
  isSidebarOpen = false;

  @ViewChild('tabScroll') tabScroll!: ElementRef;

  /* ---------- TOP TABS (ROLE BASED – KEEP OLD LOGIC) ---------- */
  mainTabs: MainTab[] = [
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

  /* ---------- SIDEBAR (ROLE + PERMISSION BASED) ---------- */
  sidebarTabs: SidebarTab[] = [
    {
      name: 'Repair Order',
      icon: 'bi bi-tools me-2',
      route: '/repair-order',
      module: 'RepairOrder',
      permission: 'V'
    },
    {
      name: 'Job Cards',
      icon: 'fas fa-pen-to-square',
      route: '/jobcardlist',
      module: 'JobCard',
      permission: 'V'
    },
    {
      name: 'Roles',
      icon: 'fas fa-user-shield',
      route: '/roles',
      module: 'Role',
      permission: 'V'
    },
    {
      name: 'Permissions',
      icon: 'fas fa-key',
      route: '/permission',
      module: 'Permission',
      permission: 'V'
    },
    {
      name: 'Role Permission',
      icon: 'fas fa-lock',
      route: '/rolepermission',
      module: 'RolePermission',
      permission: 'V'
    }
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
        this.user = null;
        this.role = null;
        this.filteredTabs = [];
        this.filteredSidebarTabs = [];
        return;
      }

      this.authService.getUserProfile().subscribe(profile => {
        this.user = profile;
        this.role = profile.role;

        /* ---------- TOP TAB FILTER (ROLE ONLY) ---------- */
        this.filteredTabs = this.mainTabs.filter(tab =>
          tab.roles.includes(this.role ?? '')
        );

        /* ---------- SIDEBAR FILTER (PERMISSION) ---------- */
        const roleId = this.authService.getRoleId();
        console.log(roleId);
        if (!roleId) return;

        this.filteredSidebarTabs = [];

        this.sidebarTabs.forEach(tab => {
          this.rolePermissionService
            .getRoleModulePermissions(roleId, tab.module)
            .subscribe(perms => {
              if (perms.includes(tab.permission)) {
                this.filteredSidebarTabs.push(tab);
                setTimeout(() => this.checkScroll(), 50);
              }
            });
        });
      });
    });

    /* ---------- AUTO CLOSE SIDEBAR ---------- */
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
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

    this.showLeftArrow = el.scrollLeft > 3;
    this.showRightArrow = el.scrollLeft + el.clientWidth < el.scrollWidth - 3;
  }

  toggleUserPopup() {
    this.showUserPopup = !this.showUserPopup;
  }

  closePopup() {
    this.showUserPopup = false;
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  logout() {
    this.authService.logout();
    this.closePopup();
    this.isSidebarOpen = false;
    this.router.navigate(['/login']);
    this.user = null;
  }
}
