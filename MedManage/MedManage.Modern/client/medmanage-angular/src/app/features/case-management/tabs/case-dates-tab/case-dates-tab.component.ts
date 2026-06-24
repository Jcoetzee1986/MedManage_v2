import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseDto, UpdateCaseRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-dates-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  templateUrl: './case-dates-tab.component.html',
  styleUrls: ['./case-dates-tab.component.scss']
})
export class CaseDatesTabComponent implements OnInit {
  @Input({ required: true }) caseData!: CaseDto;
  @Input({ required: true }) caseId!: number;

  private readonly fb = inject(FormBuilder);
  private readonly caseService = inject(CaseService);
  private readonly snackBar = inject(MatSnackBar);

  form = this.fb.group({
    dateOfService: [null as Date | null],
    dateAdmitted: [null as Date | null],
    dateDischarged: [null as Date | null]
  });

  ngOnInit(): void {
    if (this.caseData) {
      this.form.patchValue({
        dateOfService: this.caseData.dateOfService ? new Date(this.caseData.dateOfService) : null,
        dateAdmitted: this.caseData.dateAdmitted ? new Date(this.caseData.dateAdmitted) : null,
        dateDischarged: this.caseData.dateDischarged ? new Date(this.caseData.dateDischarged) : null
      });
    }
  }

  onSave(): void {
    const formVal = this.form.value;
    const request: UpdateCaseRequest = {
      id: this.caseId,
      dateOfService: formVal.dateOfService?.toISOString(),
      dateAdmitted: formVal.dateAdmitted?.toISOString(),
      dateDischarged: formVal.dateDischarged?.toISOString()
    };

    this.caseService.update(this.caseId, request).subscribe({
      next: () => this.snackBar.open('Dates updated', 'Close', { duration: 3000 }),
      error: () => this.snackBar.open('Failed to update dates', 'Close', { duration: 3000 })
    });
  }
}
