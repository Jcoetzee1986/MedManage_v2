import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Subject, takeUntil, filter } from 'rxjs';
import { ActivityTrackingService } from './core/services/activity-tracking.service';
import { AuthService } from './core/services/auth.service';
import { SessionTimeoutWarningComponent } from './shared/components/session-timeout-warning.component';
import { ClientSwitcherComponent } from './shared/components/client-switcher/client-switcher.component';

@Component({
    selector: 'app-root',
    imports: [CommonModule, RouterOutlet, MatToolbarModule, MatDialogModule, ClientSwitcherComponent],
    templateUrl: './app.component.html',
    styles: [`
    .app-container {
      height: 100vh;
      width: 100%;
      display: flex;
      flex-direction: column;
    }
    .app-header {
      display: flex;
      justify-content: flex-end;
      align-items: center;
      padding: 8px 16px;
      min-height: 60px;
      background: #f5f5f5;
      border-bottom: 1px solid #e0e0e0;
      overflow: visible;
    }
    .app-content {
      flex: 1;
      overflow: auto;
    }
  `]
})
export class AppComponent implements OnInit, OnDestroy {
  private readonly activityTrackingService = inject(ActivityTrackingService);
  private readonly authService = inject(AuthService);
  private readonly dialog = inject(MatDialog);
  private readonly destroy$ = new Subject<void>();
  
  private warningDialogRef: any = null;

  title = 'MedManage';

  /** Whether the user is currently authenticated (for showing app-level UI) */
  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  ngOnInit(): void {
    // Start activity tracking if user is authenticated
    if (this.authService.isAuthenticated()) {
      this.activityTrackingService.startTracking();
    }

    // Subscribe to authentication state changes
    this.authService.currentUser$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(user => {
      if (user) {
        // User logged in, start tracking
        this.activityTrackingService.startTracking();
      } else {
        // User logged out, stop tracking
        this.activityTrackingService.stopTracking();
        this.closeWarningDialog();
      }
    });

    // Subscribe to session timeout warnings
    this.activityTrackingService.showWarning$.pipe(
      takeUntil(this.destroy$),
      filter(show => show === true && this.warningDialogRef === null)
    ).subscribe(() => {
      this.showTimeoutWarning();
    });

    // Auto-close dialog when warning is dismissed
    this.activityTrackingService.showWarning$.pipe(
      takeUntil(this.destroy$),
      filter(show => show === false && this.warningDialogRef !== null)
    ).subscribe(() => {
      this.closeWarningDialog();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.activityTrackingService.stopTracking();
    this.closeWarningDialog();
  }

  private showTimeoutWarning(): void {
    if (this.warningDialogRef) {
      return; // Dialog already shown
    }

    this.warningDialogRef = this.dialog.open(SessionTimeoutWarningComponent, {
      width: '450px',
      disableClose: true, // Prevent closing by clicking outside
      panelClass: 'session-timeout-dialog'
    });

    this.warningDialogRef.afterClosed().subscribe((continueSession: boolean) => {
      this.warningDialogRef = null;
      
      if (!continueSession) {
        // User chose to logout
        this.authService.logout();
      }
    });
  }

  private closeWarningDialog(): void {
    if (this.warningDialogRef) {
      this.warningDialogRef.close();
      this.warningDialogRef = null;
    }
  }
}
