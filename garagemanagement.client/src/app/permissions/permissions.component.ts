import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Permission, RolePermissionService } from '../services/role-permission.service';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-permissions',
  templateUrl: './permissions.component.html',
  standalone: false
})
export class PermissionsComponent implements OnInit {

  permissions: Permission[] = [];
  permissionForm!: FormGroup;

  currentPermissionId = 0;

  constructor(
    private rbac: RolePermissionService,
    private fb: FormBuilder,
    private alert: AlertService
  ) { }

  ngOnInit(): void {
    this.permissionForm = this.fb.group({
      name: ['', Validators.required],
      description: ['']
    });

    this.loadPermissions();
  }

  loadPermissions(): void {
    this.rbac.getPermissions().subscribe(res => this.permissions = res);
  }

  editPermission(permission: Permission): void {
    this.currentPermissionId = permission.id;
    this.permissionForm.patchValue(permission);
  }

  savePermission(): void {
    if (this.permissionForm.invalid) {
      this.alert.showError('Please fill all fields correctly before saving.');
      return;
    }

    const permission: Permission = {
      id: this.currentPermissionId,
      name: this.permissionForm.value.name.trim(),
      description: this.permissionForm.value.description
    };

    this.rbac.savePermission(permission).subscribe({
      next: () => {
        this.alert.showSuccess(
          this.currentPermissionId === 0
            ? 'Permission added successfully'
            : 'Permission updated successfully'
        );
        this.resetForm();
        this.loadPermissions();
      },
      error: (err) => {
        this.alert.showError(
          err?.error?.message || 'Unable to save permission'
        );
      }
    });
  }

  resetForm(): void {
    this.currentPermissionId = 0;
    this.permissionForm.reset();
  }

  deletePermission(id: number): void {
    this.alert.confirm('Delete this permission?', () => {
      this.rbac.deletePermission(id).subscribe({
        next: () => this.loadPermissions(),
        error: (err) => {
          this.alert.showError(
            err?.error?.message || 'Permission cannot be deleted'
          );
        }
      });
    });
  }
}
