import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Subject, interval, takeUntil } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

/**
 * Service to track user activity and handle session timeout
 */
@Injectable({
  providedIn: 'root'
})
export class ActivityTrackingService {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  private readonly TIMEOUT_MINUTES = environment.sessionTimeoutMinutes;
  private readonly WARNING_MINUTES = 5; // Show warning 5 minutes before timeout
  private readonly CHECK_INTERVAL_MS = 1000; // Check every second

  private lastActivityTime: Date = new Date();
  private warningTimer: any;
  private timeoutTimer: any;
  private checkInterval: any;
  
  private destroy$ = new Subject<void>();
  private showWarningSubject = new BehaviorSubject<boolean>(false);
  private timeRemainingSubject = new BehaviorSubject<number>(0);

  public showWarning$ = this.showWarningSubject.asObservable();
  public timeRemaining$ = this.timeRemainingSubject.asObservable();

  constructor() { }

  /**
   * Start tracking user activity
   */
  startTracking(): void {
    this.resetActivity();
    this.startCheckInterval();
  }

  /**
   * Stop tracking user activity
   */
  stopTracking(): void {
    this.clearTimers();
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Record user activity (call this on HTTP requests)
   */
  recordActivity(): void {
    if (!this.authService.isAuthenticated()) {
      return;
    }

    this.lastActivityTime = new Date();
    
    // If warning is showing, hide it
    if (this.showWarningSubject.value) {
      this.showWarningSubject.next(false);
    }
  }

  /**
   * Reset activity timer
   */
  resetActivity(): void {
    this.lastActivityTime = new Date();
    this.showWarningSubject.next(false);
  }

  /**
   * User confirmed they're still there (from warning dialog)
   */
  confirmActivity(): void {
    this.resetActivity();
  }

  /**
   * Get minutes since last activity
   */
  private getMinutesSinceLastActivity(): number {
    const now = new Date();
    const diffMs = now.getTime() - this.lastActivityTime.getTime();
    return diffMs / (1000 * 60);
  }

  /**
   * Start interval to check for timeout
   */
  private startCheckInterval(): void {
    this.checkInterval = setInterval(() => {
      if (!this.authService.isAuthenticated()) {
        return;
      }

      const minutesSinceActivity = this.getMinutesSinceLastActivity();
      const warningThreshold = this.TIMEOUT_MINUTES - this.WARNING_MINUTES;

      // Calculate time remaining until timeout
      const timeRemaining = Math.max(0, this.TIMEOUT_MINUTES - minutesSinceActivity);
      this.timeRemainingSubject.next(Math.ceil(timeRemaining));

      // Show warning if past warning threshold but before timeout
      if (minutesSinceActivity >= warningThreshold && minutesSinceActivity < this.TIMEOUT_MINUTES) {
        if (!this.showWarningSubject.value) {
          this.showWarningSubject.next(true);
        }
      }

      // Logout if timeout reached
      if (minutesSinceActivity >= this.TIMEOUT_MINUTES) {
        this.handleTimeout();
      }
    }, this.CHECK_INTERVAL_MS);
  }

  /**
   * Handle session timeout
   */
  private handleTimeout(): void {
    this.clearTimers();
    this.showWarningSubject.next(false);
    this.authService.logout();
    this.router.navigate(['/login'], {
      queryParams: {
        message: 'Your session has timed out due to inactivity.'
      }
    });
  }

  /**
   * Clear all timers
   */
  private clearTimers(): void {
    if (this.warningTimer) {
      clearTimeout(this.warningTimer);
      this.warningTimer = null;
    }
    if (this.timeoutTimer) {
      clearTimeout(this.timeoutTimer);
      this.timeoutTimer = null;
    }
    if (this.checkInterval) {
      clearInterval(this.checkInterval);
      this.checkInterval = null;
    }
  }
}
