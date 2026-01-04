import { Component } from '@angular/core';
import { Permission, RolePermissionService } from '../services/role-permission.service';

@Component({
  selector: 'app-permissions',
  standalone: false,
  templateUrl: './permissions.component.html',
  styleUrl: './permissions.component.css'
})
export class PermissionsComponent {

  permissions: Permission[] = [];
  newPermissionName: string = '';
  newPermissionDescription: string = '';

  constructor(private rbac: RolePermissionService) { }

  ngOnInit() {
    this.loadPermissions();
  }

  loadPermissions() {
    this.rbac.getPermissions().subscribe(res => this.permissions = res);
  }

  addPermission() {
    if (!this.newPermissionName) return;
    this.rbac.createPermission({
      id: 0,
      name: this.newPermissionName,
      description: this.newPermissionDescription
    }).subscribe(() => {
      this.newPermissionName = '';
      this.newPermissionDescription = '';
      this.loadPermissions();
    });
  }

  deletePermission(id: number) {
    this.rbac.deletePermission(id).subscribe(() => this.loadPermissions());
  }
}
