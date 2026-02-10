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

  ngOnInit() {
    this.assignuser = this.fb.group({
      id: [0],
      workshopSearch: ['', [Validators.required]],
      workshopId: [0, [Validators.required]],
      userSearch: ['', [Validators.required]],
      userId: [0, [Validators.required]]
    });

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
  resetEntireForm() { alert("clear") }
  submit() {
    if (this.assignuser.invalid) {
      this.assignuser.markAllAsTouched();
      this.alert.showError("Please fill all required fields!");
      return;
    }
    alert(JSON.stringify(this.assignuser.value));

    this.http.post(`${this.baseUrl}/WorkshopProfile/assignuser`, this.assignuser.value).subscribe({
      next: (res: any) => {
        if (res.isSuccess) {
          this.alert.showInfo('Assined Succesfully', () => {
            this.assignuser.reset();
          });
        }
      },
      error: (error: any) => {
        this.alert.showError('Internal Server Error');
        return;
      }

    })
  }
}
