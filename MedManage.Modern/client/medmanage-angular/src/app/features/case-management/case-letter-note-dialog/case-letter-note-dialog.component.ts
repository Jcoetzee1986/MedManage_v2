import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';

export interface CaseLetterNoteDialogData {
  caseId: number;
  existingNote?: string;
  includeDischargeForm?: boolean;
  includeReferralLetter?: boolean;
}

export interface CaseLetterNoteResult {
  note: string;
  includeDischargeForm: boolean;
  includeReferralLetter: boolean;
}

@Component({
  selector: 'app-case-letter-note-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule
  ],
  template: `
    <h2 mat-dialog-title>Letter Note</h2>
    <mat-dialog-content>
      <p style="margin-bottom: 8px; color: #666;">Please add your note below:</p>
      <mat-form-field appearance="outline" class="full-width">
        <mat-label>Note</mat-label>
        <textarea matInput [formControl]="noteControl" rows="8" placeholder="Enter case letter note..."></textarea>
      </mat-form-field>
      <div style="display: flex; gap: 24px; margin-top: 8px;">
        <mat-checkbox [formControl]="dischargeControl">Include Discharge Form</mat-checkbox>
        <mat-checkbox [formControl]="referralControl">Include Down Referral Form</mat-checkbox>
      </div>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-stroked-button (click)="onCancel()">Cancel</button>
      <button mat-raised-button color="primary" (click)="onConfirm()">Generate Letter</button>
    </mat-dialog-actions>
  `
})
export class CaseLetterNoteDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<CaseLetterNoteDialogComponent>);
  private readonly data: CaseLetterNoteDialogData = inject(MAT_DIALOG_DATA);
  private readonly fb = inject(FormBuilder);

  noteControl = this.fb.control(this.data.existingNote || '');
  dischargeControl = this.fb.control(this.data.includeDischargeForm || false);
  referralControl = this.fb.control(this.data.includeReferralLetter || false);

  onConfirm(): void {
    this.dialogRef.close({
      note: this.noteControl.value || '',
      includeDischargeForm: this.dischargeControl.value || false,
      includeReferralLetter: this.referralControl.value || false
    } as CaseLetterNoteResult);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
