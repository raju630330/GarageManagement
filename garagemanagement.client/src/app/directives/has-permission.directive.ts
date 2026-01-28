import { Directive, Input, TemplateRef, ViewContainerRef, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { RolePermissionService, RolePermissionDto } from '../services/role-permission.service';

@Directive({
  selector: '[appHasPermission]',
  standalone:false
})
export class HasPermissionDirective implements OnInit {

  @Input('appHasPermission') permission!: string; // 'V' | 'A' | 'E' | 'D'
  @Input('appHasPermissionModule') module!: string;

  private roleId = 0;

  constructor(
    private template: TemplateRef<any>,
    private view: ViewContainerRef,
    private auth: AuthService,
    private rbac: RolePermissionService
  ) { }

  ngOnInit(): void {
    this.roleId = this.auth.getRoleId() ?? 0;

    if (!this.roleId || !this.permission || !this.module) {
      this.view.clear();
      return;
    }

    // Fetch permissions once per user (or get from service cache)
    this.rbac.getRolePermissions(this.roleId).subscribe((permissions) => {

      const allowed = permissions.some(p =>
        p.moduleName === this.module &&
        this.mapPermissionIdToCode(p.permissionId) === this.permission
      );

      this.view.clear();
      if (allowed) this.view.createEmbeddedView(this.template);
    });
  }

  /** Map numeric permissionId to 'V', 'A', 'E', 'D' */
  private mapPermissionIdToCode(permissionId?: number): string {
    switch (permissionId) {
      case 1: return 'V';
      case 2: return 'A';
      case 3: return 'E';
      case 4: return 'D';
      default: return '';
    }
  }
}
