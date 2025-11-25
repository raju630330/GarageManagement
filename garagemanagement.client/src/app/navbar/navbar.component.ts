import { Component, ViewChild, ElementRef, AfterViewInit, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit,AfterViewInit {

  constructor(private authService: AuthService) { }
  

  filteredTabs: any = [];

  ngOnInit() {
    const role: any = this.authService.getRole();
    this.filteredTabs = this.mainTabs.filter(tab => tab.roles.includes(role));
  }


  mainTabs = [
    { name: 'Profile', route: '/profile', roles: ['Admin', 'Manager', 'Driver', 'Technician'] },
    { name: 'Workshop', route: '/workshop', roles: ['Admin', 'Manager'] },
    { name: 'Users', route: '/users', roles: ['Admin'] },
    { name: 'MMVY', route: '/mmvy', roles: ['Admin', 'Manager'] },
    { name: 'Settings', route: '/settings', roles: ['Admin'] },
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

  ngAfterViewInit() {
    this.checkScroll();
  }

  scrollTabs(amount: number) {
    if (this.tabScroll) {
      this.tabScroll.nativeElement.scrollBy({ left: amount, behavior: 'smooth' });
      setTimeout(() => this.checkScroll(), 300); // Re-check after scrolling
    }
  }

  checkScroll() {
    if (!this.tabScroll) return;
    const scrollElem = this.tabScroll.nativeElement;
    this.showLeftArrow = scrollElem.scrollLeft > 0;
    this.showRightArrow = scrollElem.scrollWidth > scrollElem.clientWidth + scrollElem.scrollLeft;
  }
}


