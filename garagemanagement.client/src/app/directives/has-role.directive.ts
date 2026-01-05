import { Directive, Input, TemplateRef, ViewContainerRef, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { RolePermissionService } from '../services/role-permission.service';

@Directive({
  selector: '[appHasRole]',
  standalone: false
})
export class HasRoleDirective implements OnInit {

  @Input('appHasRole') moduleName!: string;
  @Input() permission!: string;

  private userRoleId = 0;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private rolePermService: RolePermissionService,
    private auth: AuthService
  ) { }

  ngOnInit(): void {
    this.userRoleId = this.auth.getRoleId() ?? 0;

    if (!this.userRoleId) {
      this.viewContainer.clear();
      return;
    }

    this.checkPermission();
  }

  private checkPermission(): void {
    if (!this.moduleName || !this.permission) {
      this.viewContainer.clear();
      return;
    }

    this.rolePermService
      .getRoleModulePermissions(this.userRoleId, this.moduleName)
      .subscribe({
        next: perms => {
          this.viewContainer.clear();
          if (perms.includes(this.permission)) {
            this.viewContainer.createEmbeddedView(this.templateRef);
          }
        },
        error: () => this.viewContainer.clear()
      });
  }
}
