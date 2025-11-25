import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { AuthService } from './services/auth.service';

export const authGuard: CanActivateChildFn = (route) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  // Public routes
  if (route.data['public']) return true;

  // Check token
  if (!auth.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  // Check roles (if specified)
  const allowedRoles = route.data['roles'];
  if (allowedRoles) {
    const role = auth.getRole();
    if (!allowedRoles.includes(role)) {
      router.navigate(['/unauthorized']);
      return false;
    }
  }

  return true;
};
