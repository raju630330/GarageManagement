import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { AlertService } from '../services/alert.service';
@Component({
  selector: 'app-user',
  standalone: false,
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {
  private baseUrl = environment.apiUrl;
  users: any[] = [];
  userForm!: FormGroup;
  isEditMode = false;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private alert: AlertService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
    this.getUsers();
  }

  // 🔹 Reactive Form Init
  initializeForm(): void {
    this.userForm = this.fb.group({
      userId: [0, Validators.required],
      username: [{ value: '', disabled: true }, Validators.required],
      roleId: ['', Validators.required]
    });
  }

  // 🔹 Fetch users
  getUsers(): void {
    this.http.get<any[]>(`${this.baseUrl}/User`).subscribe(res => {
      this.users = res;
    });
  }

  // 🔹 Click Edit
  editUser(user: any): void {
    this.isEditMode = true;

    this.userForm.patchValue({
      userId: user.id,
      username: user.username,
      roleId: user.roleId
    });
  }

  // 🔹 Update role
  updateUserRole(): void {

    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      this.alert.showError("Please select all required fields!");
      return;
    }

    const payload = {
      userId: this.userForm.get('userId')?.value,
      roleId: this.userForm.get('roleId')?.value
    };

    this.http.put(`${this.baseUrl}/User/update-role`, payload).subscribe({
      next: (res: any) => {
        this.alert.showSuccess(res.message || 'Role updated successfully');
        this.cancelEdit();
        this.getUsers();
      },
      error: () => alert('Failed to update role')
    });
  }

  // 🔹 Cancel
  cancelEdit(): void {
    this.isEditMode = false;
    this.userForm.reset({
      userId: 0,
      roleId: 0
    });
  }
}
