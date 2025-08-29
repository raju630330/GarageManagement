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


}
