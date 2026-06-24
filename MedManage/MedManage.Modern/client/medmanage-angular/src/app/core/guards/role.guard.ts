import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

/**
 * Role-based route guard factory.
 * Checks if the authenticated user has at least one of the required roles.
 *
 * Usage in routes:
 *   canActivate: [roleGuard('Admin', 'CaseManager')]
 */
export function roleGuard(...allowedRoles: string[]): CanActivateFn {
  return (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (!authService.isAuthenticated()) {
      router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }

    const hasRequiredRole = allowedRoles.some(role => authService.hasRole(role));

    if (!hasRequiredRole) {
      // User is authenticated but lacks the required role
      router.navigate(['/dashboard']);
      return false;
    }

    return true;
  };
}
