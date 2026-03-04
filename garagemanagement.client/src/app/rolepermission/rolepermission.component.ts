import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import {
  RolePermissionService,
  Role,
  PermissionModule,
  RolePermissionDto,
  BaseResultDto,
  Permission
} from '../services/role-permission.service';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-rolepermission',
  templateUrl: './rolepermission.component.html',
  styleUrls: ['./rolepermission.component.css'],
  standalone: false
})
export class RolepermissionComponent implements OnInit {

  roles: Role[] = [];
  modules: PermissionModule[] = [];
  permissions: Permission[] = [];

  rolePermissions: RolePermissionDto[] = [];

  newModuleName = '';
  isSaving = false;

  constructor(
    private rbac: RolePermissionService,
    private alert: AlertService
  ) { }

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {

    forkJoin([
      this.rbac.getRoles(),
      this.rbac.getModules(),
      this.rbac.getPermissions()
    ]).subscribe(([roles, modules, permissions]) => {

      this.roles = roles;

      this.modules = modules.sort((a, b) =>
        a.name.toLowerCase().localeCompare(b.name.toLowerCase())
      );

      this.permissions = permissions;

      const calls = roles.map(r => this.rbac.getRolePermissions(r.id));

      forkJoin(calls).subscribe(results => {

        this.rolePermissions = [];

        results.forEach(list => {

          list.forEach(p => {
            if (!p.moduleName) {
              const module = this.modules.find(m => m.id === p.permissionModuleId);
              if (module) p.moduleName = module.name;
            }
          });

          this.rolePermissions.push(...list);

        });

      });

    });

  }
  /** Check if a permission is selected */
  isChecked(roleId: number, moduleId: number, permissionId: number): boolean {

    return this.rolePermissions.some(rp =>
      rp.roleId === roleId &&
      rp.permissionModuleId === moduleId &&
      rp.permissionId === permissionId
    );

  }
  /** Toggle a permission */
  toggle(roleId: number, moduleId: number, permissionId: number): void {

    const module = this.modules.find(m => m.id === moduleId);
    if (!module) return;

    const index = this.rolePermissions.findIndex(rp =>
      rp.roleId === roleId &&
      rp.permissionModuleId === moduleId &&
      rp.permissionId === permissionId
    );

    if (index >= 0) {

      this.rolePermissions.splice(index, 1);

    } else {

      this.rolePermissions.push({
        roleId,
        permissionModuleId: moduleId,
        permissionId,
        moduleName: module.name
      });

    }

  }
  /** Save all permissions */
  save(): void {
    this.isSaving = true;

    this.rbac.saveAllRolePermissions(this.rolePermissions).subscribe({
      next: (res: BaseResultDto) => {
        this.isSaving = false;

        if (res.isSuccess) {
          this.alert.showInfo('Permissions saved successfully', () => {
            window.location.reload();
          });
        } else {
          this.alert.showError(res.message);
        }
      },
      error: () => {
        this.isSaving = false;
        this.alert.showError('Save failed');
      }
    });
  }



  /** Add a new module */
  addModule(): void {
    const name = this.newModuleName.trim();
    if (!name) return this.alert.showError('Module name required');

    this.rbac.addModule(name).subscribe(res => {
      if (res.isSuccess) {
        this.alert.showSuccess('Module added');
        this.newModuleName = '';
        this.rbac.getModules().subscribe(m => this.modules = m);
      } else {
        this.alert.showError(res.message);
      }
    });
  }


  getRoleColor(index: number): string {
    const colors = [
      '#0d6efd', // blue
      '#198754', // green
      '#fd7e14', // orange
      '#6f42c1', // purple
      '#dc3545', // red
      '#0dcaf0', // cyan
      '#ffc107', // yellow
      '#6610f2', // dark purple
    ];
    return colors[index % colors.length];
  }

}
