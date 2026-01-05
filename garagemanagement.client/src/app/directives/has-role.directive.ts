import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  OnInit
} from '@angular/core';
import { AuthService } from '../services/auth.service';
import { RolePermissionService } from '../services/role-permission.service';

@Directive({
  selector: '[appHasRole]',
  standalone: false
})
export class HasRoleDirective implements OnInit {

  // Permission name → READ / WRITE / DELETE
  @Input('appHasRole') permission!: string;

  // 🔥 MODULE ID (NOT NAME)
  @Input() moduleId!: number;

  private userRoleId = 0;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private rolePermService: RolePermissionService,
    private auth: AuthService
  ) { }

  ngOnInit(): void {
    this.userRoleId = this.auth.getRoleId() ?? 0;

    if (!this.userRoleId || !this.permission || !this.moduleId) {
      this.viewContainer.clear();
      return;
    }

    this.checkPermission();
  }

  private checkPermission(): void {
    this.rolePermService
      .hasPermission(this.userRoleId, this.moduleId, this.permission)
      .subscribe({
        next: hasAccess => {
          this.viewContainer.clear();
          if (hasAccess) {
            this.viewContainer.createEmbeddedView(this.templateRef);
          }
        },
        error: () => this.viewContainer.clear()
      });
  }
}
