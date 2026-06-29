import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Subject, takeUntil, filter } from 'rxjs';
import { ActivityTrackingService } from './core/services/activity-tracking.service';
import { AuthService } from './core/services/auth.service';
import { SessionTimeoutWarningComponent } from './shared/components/session-timeout-warning.component';
import { ClientSwitcherComponent } from './shared/components/client-switcher/client-switcher.component';
import { LoadingSpinnerComponent } from './shared/components/loading-spinner/loading-spinner.component';

@Component({
    selector: 'app-root',
    imports: [
      CommonModule,
      RouterOutlet,
      RouterLink,
      RouterLinkActive,
      MatToolbarModule,
      MatIconModule,
      MatButtonModule,
      MatTooltipModule,
      MatDialogModule,
      ClientSwitcherComponent,
      LoadingSpinnerComponent
    ],
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private readonly activityTrackingService = inject(ActivityTrackingService);
  private readonly authService = inject(AuthService);
  private readonly dialog = inject(MatDialog);
  private readonly destroy$ = new Subject<void>();
  
  private warningDialogRef: any = null;

  title = 'MedManage';

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  get currentUserName(): string {
    const user = this.authService.getCurrentUserValue();
    return user?.username || user?.email || 'User';
  }

  onLogout(): void {
    this.authService.logout();
  }

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      this.activityTrackingService.startTracking();
    }

    this.authService.currentUser$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(user => {
      if (user) {
        this.activityTrackingService.startTracking();
      } else {
        this.activityTrackingService.stopTracking();
        this.closeWarningDialog();
      }
    });

    this.activityTrackingService.showWarning$.pipe(
      takeUntil(this.destroy$),
      filter(show => show === true && this.warningDialogRef === null)
    ).subscribe(() => {
      this.showTimeoutWarning();
    });

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
    if (this.warningDialogRef) return;

    this.warningDialogRef = this.dialog.open(SessionTimeoutWarningComponent, {
      width: '450px',
      disableClose: true,
      panelClass: 'session-timeout-dialog'
    });

    this.warningDialogRef.afterClosed().subscribe((continueSession: boolean) => {
      this.warningDialogRef = null;
      if (!continueSession) {
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
