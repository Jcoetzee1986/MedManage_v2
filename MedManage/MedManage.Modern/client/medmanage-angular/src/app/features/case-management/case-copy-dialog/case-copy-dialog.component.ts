import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { CaseCopyRequest } from '../models/case.models';

export interface CaseCopyDialogData {
  caseId: number;
  caseNumber?: string;
  admissionDate?: string;
}

@Component({
  selector: 'app-case-copy-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './case-copy-dialog.component.html',
  styleUrls: ['./case-copy-dialog.component.scss']
})
export class CaseCopyDialogComponent {
  private readonly dialogRef = inject(MatDialogRef<CaseCopyDialogComponent>);
  readonly data: CaseCopyDialogData = inject(MAT_DIALOG_DATA);
  private readonly fb = inject(FormBuilder);

  form = this.fb.group({
    newAdmissionDate: [this.data.admissionDate ? new Date(this.data.admissionDate) : new Date()],
    useSameAuthNumber: [false],
    linkToParentCase: [true],
    includeCptCodes: [true],
    includeIcdCodes: [true],
    includeTariffs: [true],
    includeFacilityTypes: [true],
    includeExclusions: [true],
    includeNotes: [true],
    includeComments: [true],
    includeNappiCodes: [true],
    includeChecklist: [false],
    includeLetterNotes: [true]
  });

  onCopy(): void {
    const val = this.form.value;
    const request: CaseCopyRequest = {
      includeCptCodes: val.includeCptCodes ?? true,
      includeIcdCodes: val.includeIcdCodes ?? true,
      includeTariffs: val.includeTariffs ?? true,
      includeFacilityTypes: val.includeFacilityTypes ?? true,
      includeExclusions: val.includeExclusions ?? true,
      includeNotes: val.includeNotes ?? true,
      includeComments: val.includeComments ?? true,
      includeNappiCodes: val.includeNappiCodes ?? true,
      includeChecklist: val.includeChecklist ?? false,
      includeLetterNotes: val.includeLetterNotes ?? true,
      useSameAuthNumber: val.useSameAuthNumber ?? false,
      linkToParentCase: val.linkToParentCase ?? true,
      newAdmissionDate: val.newAdmissionDate
        ? val.newAdmissionDate.toISOString().split('T')[0]
        : undefined
    };
    this.dialogRef.close(request);
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }
}
