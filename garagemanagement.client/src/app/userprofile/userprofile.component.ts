import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-userprofile',
  standalone: false,
  templateUrl: './userprofile.component.html',
  styleUrls: ['./userprofile.component.css']
})
export class UserprofileComponent implements OnInit {

  user: any = null;
  isLoggedIn = false;
  role: string | null = null;


  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.authService.loggedIn$.subscribe(status => {
      this.isLoggedIn = status;

    if(status) {
      this.authService.getUserProfile().subscribe({
        next: (profile) => {
          console.log("API user response:", profile);

          this.user = profile;
          this.role = profile.role;
          
        },
        error: (err) => console.error("Profile fetch failed", err)
      });
    } else {
      this.user = null;
      this.role = null;
    }
  });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
    this.user = null;
  }


  book() {
    this.router.navigate(['/Calendar']);
  }

  viewProfile() {
    this.router.navigate(['//workshop']);

  }

  goToRepairOrder() {
    this.router.navigate(['/repair-order']);

  }
  goToSparePartDetails() {
    this.router.navigate(['/spare-part']);

  }
  goToJobObserveDetails() {
    this.router.navigate(['/jobobservedetails']);

  }
  goToSupervisorDetails() {
    this.router.navigate(['/garagemanagement']);

  }
  goToLabourDetails() {
    this.router.navigate(['/labour-details']);

  }
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

  showLeftArrow = true;
  showRightArrow = false;

  @ViewChild('tabScroll') tabScroll!: ElementRef;

  ngAfterViewInit() {
    this.checkScroll();
  }

  scrollTabs(amount: number) {
    if (this.tabScroll) {
      this.tabScroll.nativeElement.scrollBy({ left: amount, behavior: 'smooth' });
      setTimeout(() => this.checkScroll(), 300);
    }
  }

  checkScroll() {
    if (!this.tabScroll) return;
    const scrollElem = this.tabScroll.nativeElement;
    this.showLeftArrow = scrollElem.scrollLeft > 0;
    this.showRightArrow = scrollElem.scrollWidth > scrollElem.clientWidth + scrollElem.scrollLeft;
  }



}


