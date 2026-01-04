import { Directive, Input, TemplateRef, ViewContainerRef, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { RolePermissionService } from '../services/role-permission.service';

@Directive({
  selector: '[appHasRole]',
  standalone: false,
})
export class HasRoleDirective implements OnInit {

  @Input() moduleName!: string;   // e.g., 'JobCard'
  @Input() permission!: string;   // e.g., 'V' (View), 'A' (Add), 'D' (Delete), 'E' (Edit)

  private userRoleId: number = 0;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private rolePermService: RolePermissionService,
    private auth: AuthService
  ) { }

  ngOnInit() {
    this.userRoleId = this.auth.getRoleId() ?? 0;  // fallback to 0 if null
    if (!this.userRoleId) {
      this.viewContainer.clear(); // no role, clear the view
      return;
    }
    this.checkPermission();
  }

  private checkPermission() {
    if (!this.moduleName || !this.permission) {
      console.warn('appHasRole directive requires moduleName and permission');
      this.viewContainer.clear();
      return;
    }

    this.rolePermService.getRoleModulePermissions(this.userRoleId, this.moduleName)
      .subscribe({
        next: perms => {
          if (perms.includes(this.permission)) {
            this.viewContainer.createEmbeddedView(this.templateRef);
          } else {
            this.viewContainer.clear();
          }
        },
        error: () => this.viewContainer.clear()
      });
  }
}
