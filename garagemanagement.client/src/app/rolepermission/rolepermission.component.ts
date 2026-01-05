import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { forkJoin } from 'rxjs';
import {
  Role,
  PermissionModule,
  RolePermissionService
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
  form!: FormGroup;
  isSaving = false;  // Used for save & add module buttons

  constructor(
    private fb: FormBuilder,
    private rbac: RolePermissionService,
    private alert: AlertService
  ) { }

  // ============================
  // INIT
  // ============================
  ngOnInit(): void {
    this.form = this.fb.group({
      roleId: [null, Validators.required],
      newModule: ['', Validators.required],  // ✅ Added for Add Module input
      modules: this.fb.group({})
    });

    this.loadInitialData();

    this.form.get('roleId')?.valueChanges.subscribe(roleId => {
      if (roleId) {
        this.loadRoleModules(roleId);
      } else {
        this.resetModules();
      }
    });
  }

  // ============================
  // LOAD INITIAL DATA
  // ============================
  loadInitialData(): void {
    this.rbac.getRoles().subscribe({
      next: res => this.roles = res,
      error: () => this.alert.showError('Failed to load roles')
    });

    this.rbac.getModules().subscribe({
      next: res => {
        this.modules = res;
        this.buildModuleControls();
      },
      error: () => this.alert.showError('Failed to load modules')
    });
  }

  // ============================
  // BUILD MODULE CHECKBOXES
  // ============================
  private buildModuleControls(): void {
    const group: any = {};
    this.modules.forEach(m => group[m.id] = [false]);
    this.form.setControl('modules', this.fb.group(group));
  }

  // ============================
  // LOAD ROLE → MODULE ACCESS
  // ============================
  loadRoleModules(roleId: number): void {
    this.resetModules();

    this.rbac.getRolePermissions(roleId).subscribe({
      next: res => {
        const fg = this.form.get('modules') as FormGroup;

        const assignedModules = new Set<number>();
        res.forEach(rp => assignedModules.add(rp.permissionModuleId));

        assignedModules.forEach(moduleId => {
          fg.get(moduleId.toString())?.setValue(true, { emitEvent: false });
        });
      },
      error: () => this.alert.showError('Failed to load role permissions')
    });
  }

  // ============================
  // RESET CHECKBOXES
  // ============================
  private resetModules(): void {
    const fg = this.form.get('modules') as FormGroup;
    Object.keys(fg.controls).forEach(k =>
      fg.get(k)?.setValue(false, { emitEvent: false })
    );
  }

  // ============================
  // SAVE ROLE MODULE ACCESS
  // ============================
  save(): void {
    if (this.form.invalid) {
      this.alert.showError('Please select a role');
      return;
    }

    const roleId = this.form.value.roleId;
    this.isSaving = true;

    const selectedModuleIds = Object.entries(this.form.value.modules)
      .filter(([_, checked]) => checked)
      .map(([id]) => Number(id));

    // Clear existing module permissions
    const clearCalls = this.modules.map(m =>
      this.rbac.clearRoleModulePermissions(roleId, m.id)
    );

    forkJoin(clearCalls).subscribe({
      next: () => {
        if (selectedModuleIds.length === 0) {
          this.isSaving = false;
          this.alert.showSuccess('Permissions updated successfully');
          return;
        }

        const saveCalls = selectedModuleIds.map(moduleId =>
          this.rbac.assignDefaultPermission(roleId, moduleId)
        );

        forkJoin(saveCalls).subscribe({
          next: () => {
            this.isSaving = false;
            this.alert.showSuccess('Permissions updated successfully');
          },
          error: () => {
            this.isSaving = false;
            this.alert.showError('Failed to save permissions');
          }
        });
      },
      error: () => {
        this.isSaving = false;
        this.alert.showError('Failed to clear existing permissions');
      }
    });
  }

  // ============================
  // ADD NEW MODULE
  // ============================
  addModule(): void {
    const moduleName = this.form.get('newModule')?.value?.trim();

    if (!moduleName) {
      this.alert.showError('Module name is required');
      return;
    }

    this.isSaving = true;

    this.rbac.addModule({
      id: 0,
      name: moduleName,
      description: ''
    }).subscribe({
      next: () => {
        this.alert.showSuccess('Module added successfully');
        this.form.get('newModule')?.reset();

        // Reload modules so new checkbox appears
        this.rbac.getModules().subscribe(mods => {
          this.modules = mods;
          this.buildModuleControls();
        });

        this.isSaving = false;
      },
      error: () => {
        this.isSaving = false;
        this.alert.showError('Failed to add module');
      }
    });
  }

}
