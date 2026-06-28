import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Observable } from 'rxjs';
import { ActivityTrackingService } from '../../core/services/activity-tracking.service';

@Component({
  selector: 'app-session-timeout-warning',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './session-timeout-warning.component.html'
})
export class SessionTimeoutWarningComponent {
  private readonly dialogRef = inject(MatDialogRef<SessionTimeoutWarningComponent>);
  private readonly activityTrackingService = inject(ActivityTrackingService);

  timeRemaining$: Observable<number> = this.activityTrackingService.timeRemaining$;

  /**
   * User clicked "I'm still here" - reset activity and close dialog
   */
  confirmActivity(): void {
    this.activityTrackingService.confirmActivity();
    this.dialogRef.close(true);
  }

  /**
   * User clicked "Logout" - close dialog and allow timeout
   */
  logout(): void {
    this.dialogRef.close(false);
  }
}
