import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError, switchMap, BehaviorSubject, filter, take, tap } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ActivityTrackingService } from '../services/activity-tracking.service';

let isRefreshing = false;
const refreshTokenSubject = new BehaviorSubject<string | null>(null);

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const activityTrackingService = inject(ActivityTrackingService);
  const router = inject(Router);
  const token = authService.getToken();

  // Record activity on every HTTP request (except auth endpoints)
  if (authService.isAuthenticated() && !req.url.includes('/auth/')) {
    activityTrackingService.recordActivity();
  }

  // Clone request and add Authorization header if token exists
  let clonedReq = req;
  if (token) {
    clonedReq = req.clone({
      headers: req.headers.set('Authorization', `Bearer ${token}`)
    });
  }

  // Handle the request and catch errors
  return next(clonedReq).pipe(
    catchError((error: HttpErrorResponse) => {
      // If 401 Unauthorized, attempt to refresh token
      if (error.status === 401 && !req.url.includes('/auth/refresh')) {
        const refreshToken = authService.getRefreshToken();
        
        if (refreshToken) {
          // If not already refreshing, start refresh process
          if (!isRefreshing) {
            isRefreshing = true;
            refreshTokenSubject.next(null);

            return authService.refreshToken().pipe(
              switchMap((response) => {
                isRefreshing = false;
                refreshTokenSubject.next(response.token || null);

                // Retry original request with new token
                const newToken = authService.getToken();
                if (newToken) {
                  const retryReq = req.clone({
                    headers: req.headers.set('Authorization', `Bearer ${newToken}`)
                  });
                  return next(retryReq);
                }
                return throwError(() => error);
              }),
              catchError((refreshError) => {
                // Refresh failed, logout user
                isRefreshing = false;
                authService.logout();
                router.navigate(['/login'], {
                  queryParams: {
                    returnUrl: router.url,
                    message: 'Your session has expired. Please login again.'
                  }
                });
                return throwError(() => refreshError);
              })
            );
          } else {
            // Wait for refresh to complete, then retry request
            return refreshTokenSubject.pipe(
              filter(token => token !== null),
              take(1),
              switchMap(() => {
                const newToken = authService.getToken();
                if (newToken) {
                  const retryReq = req.clone({
                    headers: req.headers.set('Authorization', `Bearer ${newToken}`)
                  });
                  return next(retryReq);
                }
                return throwError(() => error);
              })
            );
          }
        } else {
          // No refresh token available, logout
          authService.logout();
          router.navigate(['/login'], {
            queryParams: {
              returnUrl: router.url,
              message: 'Your session has expired. Please login again.'
            }
          });
        }
      }
      
      return throwError(() => error);
    })
  );
};
