import { CanActivateFn, Router } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { environment } from '../../../environments/environment';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // In development mode without auth configured, allow access
  if (!environment.production && !authService.getToken() && !authService.getRefreshToken()) {
    return true;
  }

  if (authService.isAuthenticated()) {
    return true;
  }

  // Token expired but refresh token exists — allow navigation
  // and let the interceptor handle the refresh on the next API call
  if (authService.getRefreshToken()) {
    return true;
  }

  router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
  return false;
};
