import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { Subject, Subscription, interval, switchMap, takeUntil, takeWhile } from 'rxjs';
import { TariffPercentageApiService } from './services/tariff-percentage-api.service';
import { TariffPercentage, TariffUpdateJobStatus, CreateTariffPercentageRequest, UpdateTariffPercentageRequest } from './models/tariff-percentage.models';
import { ApplyConfirmDialogComponent, ApplyConfirmDialogData } from './apply-confirm-dialog.component';

@Component({
  selector: 'app-tariff-percentage-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule
  ],
  templateUrl: './tariff-percentage-management.component.html',
  styleUrls: ['./tariff-percentage-management.component.scss']
})
export class TariffPercentageManagementComponent implements OnInit, OnDestroy {
  private readonly apiService = inject(TariffPercentageApiService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);
  private readonly destroy$ = new Subject<void>();

  percentages: TariffPercentage[] = [];
  displayedColumns = ['tariffPeriodName', 'startActiveDate', 'endActiveDate', 'percentageAdded', 'status', 'recordsAffected', 'actions'];
  loading = false;

  // Form state
  showForm = false;
  editingId: number | null = null;
  saving = false;
  serverError: string | null = null;

  /** Track which rows are currently processing (polling in progress) */
  processingRows = new Map<number, Subscription>();

  form = this.fb.group({
    percentageAdded: [null as number | null, [Validators.required, Validators.min(0.0001), Validators.max(9999.9999)]],
    tariffPeriodName: [null as number | null, [Validators.required, Validators.min(2000), Validators.max(2100), TariffPercentageManagementComponent.yearValidator]],
    startActiveDate: [null as Date | null, [Validators.required]],
    endActiveDate: [null as Date | null],
    notes: ['', [Validators.maxLength(500)]]
  }, { validators: [TariffPercentageManagementComponent.dateRangeValidator] });

  ngOnInit(): void {
    this.loadPercentages();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    // Clean up all polling subscriptions
    this.processingRows.forEach(sub => sub.unsubscribe());
    this.processingRows.clear();
  }

  loadPercentages(): void {
    this.loading = true;
    this.apiService.getAll().subscribe({
      next: (response) => {
        this.percentages = this.sortPercentages(response.data || []);
        this.loading = false;
      },
      error: () => {
        this.percentages = [];
        this.loading = false;
        this.snackBar.open('Failed to load tariff percentages', 'Dismiss', { duration: 3000 });
      }
    });
  }

  // ─── Apply / Propagation Methods ─────────────────────────────

  /** Returns true if the Apply button should be enabled for this row */
  isApplyEnabled(row: TariffPercentage): boolean {
    return (row.status === 'Pending' || row.status === 'Failed') && !this.processingRows.has(row.tariffPercentageId);
  }

  /** Returns true if this row is currently being processed (polling active) */
  isRowProcessing(row: TariffPercentage): boolean {
    return this.processingRows.has(row.tariffPercentageId);
  }

  /** Handle Apply button click - show confirmation dialog */
  onApplyClick(row: TariffPercentage): void {
    const dialogData: ApplyConfirmDialogData = {
      tariffPeriodName: row.tariffPeriodName,
      percentageAdded: row.percentageAdded
    };

    const dialogRef = this.dialog.open(ApplyConfirmDialogComponent, {
      width: '420px',
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.triggerApply(row);
      }
    });
  }

  // ─── Form Methods ────────────────────────────────────────────

  onAdd(): void {
    this.showForm = true;
    this.editingId = null;
    this.serverError = null;
    this.form.reset();
  }

  onEdit(item: TariffPercentage): void {
    this.showForm = true;
    this.editingId = item.tariffPercentageId;
    this.serverError = null;
    this.form.patchValue({
      percentageAdded: item.percentageAdded,
      tariffPeriodName: item.tariffPeriodName,
      startActiveDate: item.startActiveDate ? new Date(item.startActiveDate) : null,
      endActiveDate: item.endActiveDate ? new Date(item.endActiveDate) : null,
      notes: item.notes || ''
    });
  }

  onCancel(): void {
    this.showForm = false;
    this.editingId = null;
    this.serverError = null;
    this.form.reset();
  }

  onSave(): void {
    // Mark all fields as touched to show validation errors
    this.form.markAllAsTouched();

    if (this.form.invalid) {
      return;
    }

    this.saving = true;
    this.serverError = null;

    const formValue = this.form.value;

    if (this.editingId) {
      const request: UpdateTariffPercentageRequest = {
        percentageAdded: formValue.percentageAdded ?? undefined,
        startActiveDate: formValue.startActiveDate ? this.formatDate(formValue.startActiveDate) : undefined,
        endActiveDate: formValue.endActiveDate ? this.formatDate(formValue.endActiveDate) : undefined,
        notes: formValue.notes || undefined
      };

      this.apiService.update(this.editingId, request).subscribe({
        next: () => this.onSaveSuccess('updated'),
        error: (err: HttpErrorResponse) => this.onSaveError(err)
      });
    } else {
      const request: CreateTariffPercentageRequest = {
        percentageAdded: formValue.percentageAdded!,
        tariffPeriodName: formValue.tariffPeriodName!,
        startActiveDate: this.formatDate(formValue.startActiveDate!),
        endActiveDate: formValue.endActiveDate ? this.formatDate(formValue.endActiveDate) : undefined,
        notes: formValue.notes || undefined
      };

      this.apiService.create(request).subscribe({
        next: () => this.onSaveSuccess('created'),
        error: (err: HttpErrorResponse) => this.onSaveError(err)
      });
    }
  }

  canEdit(item: TariffPercentage): boolean {
    return item.status === 'Pending' || item.status === 'Failed';
  }

  get isEditing(): boolean {
    return this.editingId !== null;
  }

  // ─── Status Badge ────────────────────────────────────────────

  getStatusBadgeClass(status: string): string {
    switch (status) {
      case 'Pending': return 'status-badge status-pending';
      case 'Processing': return 'status-badge status-processing';
      case 'Completed': return 'status-badge status-completed';
      case 'Failed': return 'status-badge status-failed';
      default: return 'status-badge';
    }
  }

  // ─── Private Apply / Polling Methods ─────────────────────────

  /** Trigger the apply endpoint and start polling */
  private triggerApply(row: TariffPercentage): void {
    this.apiService.apply(row.tariffPercentageId).subscribe({
      next: (response) => {
        const jobStatus = response.data;
        // Update local row status to Processing
        this.updateRowStatus(row.tariffPercentageId, 'Processing');
        // Start polling with the returned jobId
        this.startPolling(row.tariffPercentageId, jobStatus.jobId);
      },
      error: (err) => {
        const message = err?.error?.message || 'Failed to trigger propagation';
        this.snackBar.open(message, 'Dismiss', { duration: 5000 });
      }
    });
  }

  /** Poll job status every 5 seconds until completion or failure */
  private startPolling(tariffPercentageId: number, jobId: string): void {
    // Cancel any existing polling for this row
    if (this.processingRows.has(tariffPercentageId)) {
      this.processingRows.get(tariffPercentageId)!.unsubscribe();
    }

    const pollSub = interval(5000).pipe(
      takeUntil(this.destroy$),
      switchMap(() => this.apiService.getJobStatus(jobId)),
      takeWhile(response => {
        const status = response.data?.status;
        return status === 'Processing' || status === 'Queued';
      }, true) // inclusive: emit the final value that breaks the condition
    ).subscribe({
      next: (response) => {
        const jobStatus = response.data;
        if (jobStatus.status === 'Completed') {
          this.onJobCompleted(tariffPercentageId, jobStatus);
        } else if (jobStatus.status === 'Failed') {
          this.onJobFailed(tariffPercentageId, jobStatus);
        }
      },
      error: () => {
        this.snackBar.open('Failed to poll job status', 'Dismiss', { duration: 3000 });
        this.processingRows.delete(tariffPercentageId);
      }
    });

    this.processingRows.set(tariffPercentageId, pollSub);
  }

  /** Handle successful job completion */
  private onJobCompleted(tariffPercentageId: number, jobStatus: TariffUpdateJobStatus): void {
    this.stopPolling(tariffPercentageId);
    this.updateRowStatus(tariffPercentageId, 'Completed', jobStatus.recordsAffected);
    this.snackBar.open(
      `Propagation completed. ${jobStatus.recordsAffected ?? 0} records affected.`,
      'Dismiss',
      { duration: 5000 }
    );
  }

  /** Handle job failure */
  private onJobFailed(tariffPercentageId: number, jobStatus: TariffUpdateJobStatus): void {
    this.stopPolling(tariffPercentageId);
    this.updateRowStatus(tariffPercentageId, 'Failed');
    const errorMsg = jobStatus.errorMessage || 'Propagation failed';
    this.snackBar.open(errorMsg, 'Dismiss', { duration: 8000 });
  }

  /** Stop polling for a specific row */
  private stopPolling(tariffPercentageId: number): void {
    const sub = this.processingRows.get(tariffPercentageId);
    if (sub) {
      sub.unsubscribe();
      this.processingRows.delete(tariffPercentageId);
    }
  }

  /** Update a row's status and optionally records affected in the local data */
  private updateRowStatus(tariffPercentageId: number, status: TariffPercentage['status'], recordsAffected?: number | null): void {
    const index = this.percentages.findIndex(p => p.tariffPercentageId === tariffPercentageId);
    if (index >= 0) {
      this.percentages[index] = {
        ...this.percentages[index],
        status,
        ...(recordsAffected !== undefined ? { recordsAffected } : {})
      };
      // Trigger change detection by reassigning the array
      this.percentages = [...this.percentages];
    }
  }

  // ─── Private Form Methods ────────────────────────────────────

  private onSaveSuccess(action: string): void {
    this.saving = false;
    this.showForm = false;
    this.editingId = null;
    this.form.reset();
    this.loadPercentages();
    this.snackBar.open(`Tariff percentage ${action} successfully`, 'Dismiss', { duration: 3000 });
  }

  private onSaveError(err: HttpErrorResponse): void {
    this.saving = false;
    if (err.status === 409) {
      this.serverError = err.error?.message
        || 'A tariff percentage already exists for this period and date range.';
    } else if (err.error?.message) {
      this.serverError = err.error.message;
    } else if (err.error?.errors && err.error.errors.length > 0) {
      this.serverError = err.error.errors.join('. ');
    } else {
      this.serverError = 'An unexpected error occurred. Please try again.';
    }
  }

  private formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  private sortPercentages(data: TariffPercentage[]): TariffPercentage[] {
    return [...data].sort((a, b) => {
      if (b.tariffPeriodName !== a.tariffPeriodName) {
        return b.tariffPeriodName - a.tariffPeriodName;
      }
      return b.startActiveDate.localeCompare(a.startActiveDate);
    });
  }

  // ─── Custom Validators ───────────────────────────────────────

  private static yearValidator(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    if (value === null || value === undefined || value === '') return null;
    if (!Number.isInteger(value) || value < 2000 || value > 2100) {
      return { invalidYear: true };
    }
    return null;
  }

  private static dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const start = group.get('startActiveDate')?.value;
    const end = group.get('endActiveDate')?.value;
    if (start && end && new Date(end) < new Date(start)) {
      return { endBeforeStart: true };
    }
    return null;
  }
}
