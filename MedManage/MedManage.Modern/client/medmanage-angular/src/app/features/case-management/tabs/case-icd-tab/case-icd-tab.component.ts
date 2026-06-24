import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseIcdDto, CreateCaseIcdRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-icd-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],
  templateUrl: './case-icd-tab.component.html',
  styleUrls: ['./case-icd-tab.component.scss']
})
export class CaseIcdTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseIcdDto[] = [];
  displayedColumns = ['icdCode', 'icdDescription', 'dateOfProcedure', 'primaryCode', 'secondaryCode', 'coMorbidityCode', 'actions'];
  showAddForm = false;

  addForm = this.fb.group({
    icdId: [null as number | null],
    dateOfProcedure: [null as Date | null],
    primaryCode: [false],
    secondaryCode: [false],
    coMorbidityCode: [false]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getIcdCodes(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load ICD codes', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    const val = this.addForm.value;
    const request: CreateCaseIcdRequest = {
      icdId: val.icdId || undefined,
      dateOfProcedure: val.dateOfProcedure?.toISOString(),
      primaryCode: val.primaryCode || false,
      secondaryCode: val.secondaryCode || false,
      coMorbidityCode: val.coMorbidityCode || false
    };

    this.caseService.createIcd(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.showAddForm = false;
        this.snackBar.open('ICD code added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add ICD code', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseIcdDto): void {
    if (confirm('Delete this ICD code?')) {
      this.caseService.deleteIcd(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('ICD code deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete ICD code', 'Close', { duration: 3000 })
      });
    }
  }
}
