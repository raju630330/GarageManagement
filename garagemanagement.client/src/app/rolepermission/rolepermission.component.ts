import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import {
  Permission,
  Role,
  RolePermissionDto,
  RolePermissionService
} from '../services/role-permission.service';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-rolepermission',
  templateUrl: './rolepermission.component.html',
  styleUrls: ['./rolepermission.component.css'],
  standalone : false
})
export class RolepermissionComponent implements OnInit {

  roles: Role[] = [];
  permissions: Permission[] = [];
  roleModulePermissions: string[] = [];
  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private rbac: RolePermissionService,
    private alert: AlertService
  ) { }

  ngOnInit(): void {

    this.form = this.fb.group({
      roleId: [null, Validators.required],
      moduleName: ['', Validators.required],
      permissions: this.fb.group({})
    });

    this.loadInitialData();

    // 🔥 Correct way to react to changes
    this.form.get('roleId')?.valueChanges.subscribe(() => {
      this.onRoleOrModuleChange();
    });

    this.form.get('moduleName')?.valueChanges.subscribe(() => {
      this.onRoleOrModuleChange();
    });
  }

  loadInitialData(): void {
    this.rbac.getRoles().subscribe(res => this.roles = res);

    this.rbac.getPermissions().subscribe(res => {
      this.permissions = res;
      this.buildPermissionControls();
    });
  }

  private buildPermissionControls(): void {
    const permsGroup: any = {};
    this.permissions.forEach(p => permsGroup[p.name] = [false]);
    this.form.setControl('permissions', this.fb.group(permsGroup));
  }

  private onRoleOrModuleChange(): void {
    this.resetPermissions();

    const roleId = this.form.value.roleId;
    const moduleName = this.form.value.moduleName?.trim();

    if (!roleId || !moduleName) {
      return;
    }

    this.loadRolePermissions();
  }

  loadRolePermissions(): void {
    const roleId = this.form.value.roleId;
    const moduleName = this.form.value.moduleName.trim();

    this.rbac.getRoleModulePermissions(roleId, moduleName).subscribe({
      next: res => {
        this.roleModulePermissions = res || [];
        this.syncCheckboxes();
      },
      error: () => {
        this.alert.showError('Failed to load permissions');
      }
    });
  }

  private syncCheckboxes(): void {
    const permsForm = this.form.get('permissions') as FormGroup;

    Object.keys(permsForm.controls).forEach(key => {
      permsForm.get(key)?.setValue(
        this.roleModulePermissions.includes(key),
        { emitEvent: false }
      );
    });
  }

  private resetPermissions(): void {
    this.roleModulePermissions = [];
    const permsForm = this.form.get('permissions') as FormGroup;

    Object.keys(permsForm.controls).forEach(key => {
      permsForm.get(key)?.setValue(false, { emitEvent: false });
    });
  }

  savePermissions(): void {

    if (this.form.invalid) {
      this.alert.showError('Please fill all required fields');
      return;
    }

    const { roleId, moduleName, permissions } = this.form.value;

    const selectedPermissions = Object.keys(permissions)
      .filter(p => permissions[p]);

    this.rbac.clearRoleModulePermissions(roleId, moduleName).subscribe({
      next: () => {

        if (selectedPermissions.length === 0) {
          this.alert.showSuccess('Permissions cleared successfully');
          return;
        }

        const requests = selectedPermissions
          .map(pName => {
            const perm = this.permissions.find(p => p.name === pName);
            if (!perm) return null;

            const dto: RolePermissionDto = {
              roleId,
              permissionId: perm.id,
              moduleName
            };

            return this.rbac.saveRolePermission(dto);
          })
          .filter(req => req !== null);

        forkJoin(requests as any).subscribe({
          next: () => this.alert.showSuccess('Permissions saved successfully'),
          error: err =>
            this.alert.showError(err?.error?.message || 'Save failed')
        });
      },
      error: err =>
        this.alert.showError(err?.error?.message || 'Clear failed')
    });
  }
}
