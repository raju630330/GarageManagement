import { Component, OnInit } from '@angular/core';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-workshop',
  standalone: false,
  templateUrl: './workshop.component.html',
  styleUrls: ['./workshop.component.css']
})
export class WorkshopComponent implements OnInit {

  isAdmin: boolean = false; 
  constructor(private authService: AuthService) { }

  ngOnInit(): void {   
    this.isAdmin = this.authService.isAdmin();
    console.log('User role:', this.authService.getRole());
    console.log('isAdmin flag:', this.isAdmin);

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

}
