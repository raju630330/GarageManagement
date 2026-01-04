import { Component, OnInit } from '@angular/core';
import { Role, RolePermissionService } from '../services/role-permission.service';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  standalone: false
})
export class RolesComponent implements OnInit {
  roles: Role[] = [];
  newRoleName: string = '';

  constructor(private rbac: RolePermissionService) { }

  ngOnInit() {
    this.loadRoles();
  }

  loadRoles() {
    this.rbac.getRoles().subscribe(res => this.roles = res);
  }

  addRole() {
    if (!this.newRoleName) return;
    this.rbac.createRole({ id: 0, roleName: this.newRoleName }).subscribe(() => {
      this.newRoleName = '';
      this.loadRoles();
    });
  }

  deleteRole(id: number) {
    this.rbac.deleteRole(id).subscribe(() => this.loadRoles());
  }
}
