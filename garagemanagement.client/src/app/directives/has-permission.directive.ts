import {
  Directive,
  Input,
  TemplateRef,
  ViewContainerRef,
  OnInit
} from '@angular/core';
import { AuthService } from '../services/auth.service';
import { RolePermissionService, RolePermissionDto } from '../services/role-permission.service';
import { map } from 'rxjs/operators';

@Directive({
  selector: '[appHasPermission]',
  standalone: false
})
export class HasPermissionDirective implements OnInit {

  @Input('appHasPermission') permission!: string; // V / A / E / D
  @Input() module!: string;

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

    // fetch all permissions for this role
    this.rbac.getRolePermissions(this.roleId)
      .pipe(
        map((permissions: RolePermissionDto[]) =>
          permissions.some(p =>
            p.moduleName === this.module && p.permissionId?.toString() === this.permission
          )
        )
      )
      .subscribe({
        next: (allowed : any) => {
          this.view.clear();
          if (allowed) {
            this.view.createEmbeddedView(this.template);
          }
        },
        error: () => this.view.clear()
      });
  }
}
