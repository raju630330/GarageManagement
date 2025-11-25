import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
  selector: '[appHasRole]',
  standalone: false,
})
export class HasRoleDirective {

  private allowedRoles: string[] = [];

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private auth: AuthService
  ) { }

  @Input() set appHasRole(roles: string | string[]) {
    this.allowedRoles = Array.isArray(roles) ? roles : [roles];

    const userRole = this.auth.getRole();

    if (userRole && this.allowedRoles.includes(userRole)) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
