import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements AfterViewInit {

  mainTabs = [
    { name: 'Profile', route: '/profile' },
    { name: 'Workshop', route: '' },
    { name: 'Users', route: '/users' },
    { name: 'MMVY', route: '/mmvy' },
    { name: 'Settings', route: '/settings' },
    { name: 'Subscription', route: '/subscription' },
    { name: 'Terms & Conditions', route: '/terms' },
    { name: 'Reminders', route: '/reminders' },
    { name: 'Associated Workshops', route: '/ Associated Workshops' },
    { name: 'Activate e-payment now', route: '/Activate e-payment now' },
    { name: 'Integrations', route: '/Integrations' },
    { name: 'Templates', route: '/Templates' },
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


