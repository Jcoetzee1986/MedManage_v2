import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { CaseCopyRequest } from '../models/case.models';

export interface CaseCopyDialogData {
  caseId: number;
  caseNumber?: string;
}

@Component({
  selector: 'app-case-copy-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatCheckboxModule
  ],
  templateUrl: './case-copy-dialog.component.html',
  styleUrls: ['./case-copy-dialog.component.scss']
})
export class CaseCopyDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<CaseCopyDialogComponent>);
  readonly data: CaseCopyDialogData = inject(MAT_DIALOG_DATA);
  private readonly fb = inject(FormBuilder);

  form = this.fb.group({
    includeNotes: [true],
    includeComments: [true],
    includeCpt: [true],
    includeIcd: [true],
    includeTariffs: [true],
    includeFacilityTypes: [true],
    includeExclusions: [true],
    includeNappi: [true],
    includeChecklist: [false]
  });

  onCopy(): void {
    const request: CaseCopyRequest = this.form.value as CaseCopyRequest;
    this.dialogRef.close(request);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
