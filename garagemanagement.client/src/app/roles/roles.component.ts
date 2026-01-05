import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Role, RolePermissionService } from '../services/role-permission.service';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  standalone: false
})
export class RolesComponent implements OnInit {

  roles: Role[] = [];
  roleForm!: FormGroup;

  currentRoleId = 0; // 0 = Add | >0 = Edit

  constructor(
    private rbac: RolePermissionService,
    private fb: FormBuilder,
    private alert: AlertService
  ) { }

  ngOnInit(): void {
    this.roleForm = this.fb.group({
      roleName: ['', Validators.required]
    });

    this.loadRoles();
  }

  loadRoles() {
    this.rbac.getRoles().subscribe(res => this.roles = res);
  }

  editRole(role: Role) {
    this.currentRoleId = role.id;
    this.roleForm.patchValue({ roleName: role.roleName });
  }

  saveRole() {
    if (this.roleForm.invalid) {
      this.alert.showError('Please enter a valid role name.');
      return;
    }

    const role: Role = {
      id: this.currentRoleId,
      roleName: this.roleForm.value.roleName.trim()
    };

    this.rbac.saveRole(role).subscribe({
      next: () => {
        this.alert.showSuccess(
          this.currentRoleId === 0 ? 'Role added successfully' : 'Role updated successfully'
        );
        this.resetForm();
        this.loadRoles();
      },
      error: (err) => {
        this.alert.showError(err?.error?.message || 'Operation failed');
      }
    });
  }

  resetForm() {
    this.currentRoleId = 0;
    this.roleForm.reset();
  }

  deleteRole(id: number) {
    this.alert.confirm('Delete this role?', () => {
      this.rbac.deleteRole(id).subscribe({
        next: () => this.loadRoles(),
        error: (err) => {
          this.alert.showError(err?.error?.message || 'Cannot delete role');
        }
      });
    });
  }
}


