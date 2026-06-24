import { Component, inject, OnInit, OnDestroy, HostListener } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Subject, takeUntil, interval } from 'rxjs';
import { CaseService } from '../services/case.service';
import { CaseDto } from '../models/case.models';
import { CasePrimaryTabComponent } from '../tabs/case-primary-tab/case-primary-tab.component';
import { CaseMemberTabComponent } from '../tabs/case-member-tab/case-member-tab.component';
import { CaseProviderTabComponent } from '../tabs/case-provider-tab/case-provider-tab.component';
import { CaseDatesTabComponent } from '../tabs/case-dates-tab/case-dates-tab.component';
import { CaseCptTabComponent } from '../tabs/case-cpt-tab/case-cpt-tab.component';
import { CaseIcdTabComponent } from '../tabs/case-icd-tab/case-icd-tab.component';
import { CaseTariffsTabComponent } from '../tabs/case-tariffs-tab/case-tariffs-tab.component';
import { CaseFacilityTypesTabComponent } from '../tabs/case-facility-types-tab/case-facility-types-tab.component';
import { CaseExclusionsTabComponent } from '../tabs/case-exclusions-tab/case-exclusions-tab.component';
import { CaseNappiTabComponent } from '../tabs/case-nappi-tab/case-nappi-tab.component';
import { CaseNotesTabComponent } from '../tabs/case-notes-tab/case-notes-tab.component';
import { CaseCommentsTabComponent } from '../tabs/case-comments-tab/case-comments-tab.component';
import { CaseChecklistTabComponent } from '../tabs/case-checklist-tab/case-checklist-tab.component';
import { CaseDocumentsTabComponent } from '../tabs/case-documents-tab/case-documents-tab.component';
import { CaseLinkedCasesTabComponent } from '../tabs/case-linked-cases-tab/case-linked-cases-tab.component';
import { CaseCopyDialogComponent } from '../case-copy-dialog/case-copy-dialog.component';

@Component({
  selector: 'app-case-detail',
  standalone: true,
  imports: [
    CommonModule,
    MatTabsModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatDialogModule,
    MatSnackBarModule,
    CasePrimaryTabComponent,
    CaseMemberTabComponent,
    CaseProviderTabComponent,
    CaseDatesTabComponent,
    CaseCptTabComponent,
    CaseIcdTabComponent,
    CaseTariffsTabComponent,
    CaseFacilityTypesTabComponent,
    CaseExclusionsTabComponent,
    CaseNappiTabComponent,
    CaseNotesTabComponent,
    CaseCommentsTabComponent,
    CaseChecklistTabComponent,
    CaseDocumentsTabComponent,
    CaseLinkedCasesTabComponent
  ],
  templateUrl: './case-detail.component.html',
  styleUrls: ['./case-detail.component.scss']
})
export class CaseDetailComponent implements OnInit, OnDestroy {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly caseService = inject(CaseService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);
  private readonly destroy$ = new Subject<void>();

  caseData: CaseDto | null = null;
  caseId!: number;
  loading = true;

  /** Heartbeat interval: send lock refresh every 5 minutes to prevent expiry */
  private readonly HEARTBEAT_INTERVAL_MS = 5 * 60 * 1000;
  private lockAcquired = false;

  /** Release lock when user closes/refreshes the browser tab */
  @HostListener('window:beforeunload')
  onBeforeUnload(): void {
    if (this.caseId && this.lockAcquired) {
      // Use sendBeacon for reliable fire-and-forget on page unload
      const url = `${(this.caseService as any).baseUrl}/${this.caseId}/lock`;
      // navigator.sendBeacon doesn't support DELETE, so use fetch with keepalive
      fetch(url, {
        method: 'DELETE',
        keepalive: true,
        headers: { 'Authorization': `Bearer ${localStorage.getItem('access_token') || ''}` }
      }).catch(() => {});
    }
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.caseId = +idParam;
      this.loadCase();
      this.startHeartbeat();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    // Release lock if we had one
    if (this.caseId && this.lockAcquired) {
      this.caseService.unlockCase(this.caseId).subscribe();
    }
  }

  /** Send periodic heartbeats to keep the lock alive while the user is on the page */
  private startHeartbeat(): void {
    interval(this.HEARTBEAT_INTERVAL_MS)
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        if (this.caseId && this.lockAcquired) {
          this.caseService.refreshLock(this.caseId).subscribe({
            error: () => {
              // Lock expired or was taken — notify the user
              this.lockAcquired = false;
              this.snackBar.open(
                'Your lock on this case has expired. Another user may now edit it.',
                'Dismiss',
                { duration: 10000 }
              );
            }
          });
        }
      });
  }

  private loadCase(): void {
    this.loading = true;
    this.caseService.getById(this.caseId)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.caseData = data;
          this.loading = false;
          // Acquire lock
          this.caseService.lockCase(this.caseId).subscribe({
            next: () => { this.lockAcquired = true; },
            error: () => {
              this.snackBar.open(
                'This case is currently being edited by another user.',
                'Close',
                { duration: 5000 }
              );
            }
          });
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Failed to load case', 'Close', { duration: 3000 });
        }
      });
  }

  onBackToList(): void {
    this.router.navigate(['/cases']);
  }

  onCopyCase(): void {
    const dialogRef = this.dialog.open(CaseCopyDialogComponent, {
      width: '500px',
      data: { caseId: this.caseId, caseNumber: this.caseData?.caseNumber }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.caseService.copyCase(this.caseId, result)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: (newCase) => {
              this.snackBar.open(`Case copied successfully. New case #${newCase.caseNumber}`, 'View', { duration: 5000 })
                .onAction().subscribe(() => {
                  this.router.navigate(['/cases', newCase.id]);
                });
            },
            error: () => {
              this.snackBar.open('Failed to copy case', 'Close', { duration: 3000 });
            }
          });
      }
    });
  }

  onDeleteCase(): void {
    if (confirm('Are you sure you want to delete this case?')) {
      this.caseService.delete(this.caseId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.snackBar.open('Case deleted', 'Close', { duration: 3000 });
            this.router.navigate(['/cases']);
          },
          error: () => {
            this.snackBar.open('Failed to delete case', 'Close', { duration: 3000 });
          }
        });
    }
  }
}
