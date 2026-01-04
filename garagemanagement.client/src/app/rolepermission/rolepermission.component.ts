import { Component } from '@angular/core';
import { Permission, Role, RolePermission, RolePermissionService } from '../services/role-permission.service';

@Component({
  selector: 'app-rolepermission',
  standalone: false,
  templateUrl: './rolepermission.component.html',
  styleUrl: './rolepermission.component.css',
})
export class RolepermissionComponent {
  roles: Role[] = [];
  permissions: Permission[] = [];
  selectedRoleId: number | null = null;
  selectedModule: string = 'JobCards';
  roleModulePermissions: string[] = []; // e.g., ['A','V']

  constructor(private rbac: RolePermissionService) { }

  ngOnInit() {
    this.rbac.getRoles().subscribe(res => this.roles = res);
    this.rbac.getPermissions().subscribe(res => this.permissions = res);
  }

  loadRolePermissions() {
    if (!this.selectedRoleId) return;
    this.rbac.getRoleModulePermissions(this.selectedRoleId, this.selectedModule)
      .subscribe(res => this.roleModulePermissions = res);
  }

  togglePermission(permissionName: string) {
    if (!this.selectedRoleId) return;

    const hasPerm = this.roleModulePermissions.includes(permissionName);

    if (hasPerm) {
      // remove
      const perm = this.permissions.find(p => p.name === permissionName);
      if (!perm) return;
      this.rbac.removeRolePermission(this.selectedRoleId, perm.id, this.selectedModule)
        .subscribe(() => this.loadRolePermissions());
    } else {
      // add
      const perm = this.permissions.find(p => p.name === permissionName);
      if (!perm) return;
      const rp: RolePermission = {
        roleId: this.selectedRoleId,
        permissionId: perm.id,
        moduleName: this.selectedModule
      };
      this.rbac.addRolePermission(rp).subscribe(() => this.loadRolePermissions());
    }
  }

  hasPermission(permissionName: string): boolean {
    return this.roleModulePermissions.includes(permissionName);
  }
}
