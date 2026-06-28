import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReferenceDataDropdownComponent } from '../../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { CaseService } from '../../services/case.service';
import { CaseDto, UpdateCaseRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-primary-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './case-primary-tab.component.html',
  styleUrls: ['./case-primary-tab.component.scss']
})
export class CasePrimaryTabComponent implements OnInit {
  @Input({ required: true }) caseData!: CaseDto;
  @Input({ required: true }) caseId!: number;

  private readonly fb = inject(FormBuilder);
  private readonly caseService = inject(CaseService);
  private readonly snackBar = inject(MatSnackBar);

  form = this.fb.group({
    authNumber: [''],
    caseStatusId: [null as number | null],
    caseTypeId: [null as number | null],
    caseCategoryId: [null as number | null],
    diagnosis: [''],
    comments: ['']
  });

  ngOnInit(): void {
    if (this.caseData) {
      this.form.patchValue({
        authNumber: this.caseData.authNumber || '',
        caseStatusId: this.caseData.caseStatusId || null,
        caseTypeId: this.caseData.caseTypeId || null,
        caseCategoryId: this.caseData.caseCategoryId || null,
        diagnosis: this.caseData.diagnosis || '',
        comments: this.caseData.comments || ''
      });
    }
  }

  onSave(): void {
    if (this.form.invalid) return;

    const request: UpdateCaseRequest = {
      id: this.caseId,
      ...this.form.value as any
    };

    this.caseService.update(this.caseId, request).subscribe({
      next: () => this.snackBar.open('Case updated', 'Close', { duration: 3000 }),
      error: () => this.snackBar.open('Failed to update case', 'Close', { duration: 3000 })
    });
  }
}
