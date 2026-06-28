import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

export interface ApplyConfirmDialogData {
  tariffPeriodName: number;
  percentageAdded: number;
}

@Component({
  selector: 'app-apply-confirm-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Confirm Propagation</h2>
    <mat-dialog-content>
      <p>Are you sure you want to apply the following tariff percentage?</p>
      <dl class="confirm-details">
        <dt>Period:</dt>
        <dd>{{ data.tariffPeriodName }}</dd>
        <dt>Percentage:</dt>
        <dd>{{ data.percentageAdded | number:'1.2-4' }}%</dd>
      </dl>
      <p class="warning-text">
        This will propagate the percentage to all affected ServiceProvider_Tariff records.
        This operation cannot be undone.
      </p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Cancel</button>
      <button mat-flat-button color="primary" (click)="onConfirm()">Apply</button>
    </mat-dialog-actions>
  `,
  styles: [`
    .confirm-details {
      display: grid;
      grid-template-columns: auto 1fr;
      gap: 4px 12px;
      margin: 16px 0;
      dt { font-weight: 500; }
      dd { margin: 0; }
    }
    .warning-text {
      color: rgba(0, 0, 0, 0.6);
      font-size: 0.875rem;
    }
  `]
})
export class ApplyConfirmDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<ApplyConfirmDialogComponent>);
  readonly data: ApplyConfirmDialogData = inject(MAT_DIALOG_DATA);

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}
