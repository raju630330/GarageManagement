import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AlertService } from '../services/alert.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { WorkshopProfileService } from '../services/workshop-profile.service';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-assign-user-to-workshop',
  standalone: false,
  templateUrl: './assign-user-to-workshop.component.html',
  styleUrl: './assign-user-to-workshop.component.css'
})
export class AssignUserToWorkshopComponent implements OnInit {

  constructor(private http: HttpClient, private alert: AlertService, private fb: FormBuilder, public workshopservice: WorkshopProfileService) { }
  private baseUrl = environment.apiUrl;
  assignuser!: FormGroup;
  assignedUsers!: any[];
  ngOnInit() {
    this.assignuser = this.fb.group({
      id: [0],
      workshopSearch: ['', [Validators.required]],
      workshopId: [0, [Validators.required]],
      userSearch: ['', [Validators.required]],
      userId: [0, [Validators.required]]
    });
    this.getassignedusers();
  }

  getworkshopdetails(result: any) {
    this.assignuser.patchValue({
      workshopId: Number(result.id)
    })
  }

  getuserdetails(result: any) {
    this.assignuser.patchValue({
      userId: Number(result.id)
    })
  }
  resetEntireForm() {  }
  submit() {
    if (this.assignuser.invalid) {
      this.assignuser.markAllAsTouched();
      this.alert.showError("Please fill all required fields!");
      return;
    }

    this.http.post(`${this.baseUrl}/WorkshopProfile/assignuser`, this.assignuser.value).subscribe({
      next: (res: any) => {
        if (res.isSuccess) {
          this.alert.showInfo('Assined Succesfully', () => {
            this.assignuser.reset();
            this.getassignedusers();
          });
        }
      },
      error: (error: any) => {
        this.alert.showError('Internal Server Error');
        return;
      }

    })
  }

  getassignedusers() {

    this.workshopservice.getAllAssignedUsers()
      .subscribe(res => {
        if (res.isSuccess) {
          this.assignedUsers = res.data;
        }
      });
  }
  edit(id: any) {
    if (id > 0) {
      this.workshopservice.getAssignedUser(id).subscribe({

        next: (res: any) => {
          this.assignuser.patchValue(res.data)
        },
        error: (error: any) => {
          this.alert.showError("Internal server error");
        }
      })
    }
  }
}
