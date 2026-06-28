import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { CaseService } from '../services/case.service';
import { CreateCaseRequest } from '../models/case.models';

@Component({
  selector: 'app-case-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatSnackBarModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './case-form.component.html',
  styleUrls: ['./case-form.component.scss']
})
export class CaseFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly caseService = inject(CaseService);
  private readonly router = inject(Router);
  private readonly snackBar = inject(MatSnackBar);

  form = this.fb.group({
    authNumber: [''],
    caseStatusId: [null as number | null],
    caseTypeId: [null as number | null],
    caseCategoryId: [null as number | null],
    memberId: [null as number | null],
    referToId: [null as number | null],
    referFromId: [null as number | null],
    dateOfService: [null as Date | null],
    dateAdmitted: [null as Date | null],
    dateDischarged: [null as Date | null],
    diagnosis: [''],
    comments: ['']
  });

  onSubmit(): void {
    if (this.form.invalid) return;

    const formVal = this.form.value;
    const request: CreateCaseRequest = {
      authNumber: formVal.authNumber || undefined,
      caseStatusId: formVal.caseStatusId || undefined,
      caseTypeId: formVal.caseTypeId || undefined,
      caseCategoryId: formVal.caseCategoryId || undefined,
      memberId: formVal.memberId || undefined,
      referToId: formVal.referToId || undefined,
      referFromId: formVal.referFromId || undefined,
      dateOfService: formVal.dateOfService?.toISOString(),
      dateAdmitted: formVal.dateAdmitted?.toISOString(),
      dateDischarged: formVal.dateDischarged?.toISOString(),
      diagnosis: formVal.diagnosis || undefined,
      comments: formVal.comments || undefined
    };

    this.caseService.create(request).subscribe({
      next: (newCase) => {
        this.snackBar.open('Case created successfully', 'Close', { duration: 3000 });
        this.router.navigate(['/cases', newCase.id]);
      },
      error: () => {
        this.snackBar.open('Failed to create case', 'Close', { duration: 3000 });
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/cases']);
  }
}
