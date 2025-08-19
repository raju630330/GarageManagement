import { Component } from '@angular/core';

@Component({
  selector: 'app-navbar',
  standalone: false,
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  mainTabs = [
    { name: 'Profile', route: '/profile' },
    { name: 'Workshop', route: '/workshop' },
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
