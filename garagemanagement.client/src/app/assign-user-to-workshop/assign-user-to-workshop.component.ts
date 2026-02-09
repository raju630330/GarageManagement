import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AlertService } from '../services/alert.service';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-assign-user-to-workshop',
  standalone: false,
  templateUrl: './assign-user-to-workshop.component.html',
  styleUrl: './assign-user-to-workshop.component.css'
})
export class AssignUserToWorkshopComponent implements OnInit {

  constructor(private http: HttpClient, private alert: AlertService, private fb: FormBuilder) { }

  assignuser!: FormGroup;

  ngOnInit() {


  }

}
