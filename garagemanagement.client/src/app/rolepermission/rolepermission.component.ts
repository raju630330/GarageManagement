import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import {
  RolePermissionService,
  Role,
  PermissionModule,
  RolePermissionDto,
  BaseResultDto
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
  permissionCodes = ['V', 'A', 'E', 'D'];

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
    // Load roles and modules first
    forkJoin([
      this.rbac.getRoles(),
      this.rbac.getModules()
    ]).subscribe(([roles, modules]) => {
      this.roles = roles;
      this.modules = modules;

      // Load permissions for all roles
      const calls = roles.map(r => this.rbac.getRolePermissions(r.id));
      forkJoin(calls).subscribe(results => {
        this.rolePermissions = [];
        results.forEach(list => {
          // Add moduleName if missing
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
  isChecked(roleId: number, moduleId: number, code: string): boolean {
    const pid = this.mapCodeToPermissionId(code);
    return this.rolePermissions.some(rp =>
      rp.roleId === roleId &&
      rp.permissionModuleId === moduleId &&
      rp.permissionId === pid
    );
  }

  /** Toggle a permission */
  toggle(roleId: number, moduleId: number, code: string): void {
    const pid = this.mapCodeToPermissionId(code);
    const module = this.modules.find(m => m.id === moduleId);

    if (!module) return;

    const index = this.rolePermissions.findIndex(rp =>
      rp.roleId === roleId &&
      rp.permissionModuleId === moduleId &&
      rp.permissionId === pid
    );

    if (index >= 0) {
      // Remove existing
      this.rolePermissions.splice(index, 1);
    } else {
      // Add new with moduleName
      this.rolePermissions.push({
        roleId,
        permissionModuleId: moduleId,
        permissionId: pid,
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
        res.isSuccess
          ? this.alert.showSuccess('Permissions saved successfully')
          : this.alert.showError(res.message);
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

  /** Map permission code to backend ID */
  mapCodeToPermissionId(code: string): number {
    switch (code) {
      case 'V': return 1;
      case 'A': return 2;
      case 'E': return 3;
      case 'D': return 4;
      default: return 0;
    }
  }

}
