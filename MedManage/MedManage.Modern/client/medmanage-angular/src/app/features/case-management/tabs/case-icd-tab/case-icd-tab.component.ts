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
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { CaseService } from '../../services/case.service';
import { CaseIcdDto, CreateCaseIcdRequest } from '../../models/case.models';
import { CodeLookupDialogComponent } from '../../../../shared/components/code-lookup-dialog/code-lookup-dialog.component';
import { CodeLookupResult } from '../../../../core/services/code-lookup.service';

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
    MatSnackBarModule,
    MatDialogModule
  ],
  templateUrl: './case-icd-tab.component.html',
  styleUrls: ['./case-icd-tab.component.scss']
})
export class CaseIcdTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

  items: CaseIcdDto[] = [];
  displayedColumns = ['diagnosisCode', 'diagnosisDesc', 'dateOfProcedure', 'primaryCode', 'secondaryCode', 'coMorbidityCode', 'actions'];
  showAddForm = false;
  selectedIcd: CodeLookupResult | null = null;

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

  onOpenLookup(): void {
    const dialogRef = this.dialog.open(CodeLookupDialogComponent, {
      width: '700px',
      data: { codeType: 'icd', title: 'ICD Lookup' }
    });

    dialogRef.afterClosed().subscribe((result: CodeLookupResult | null) => {
      if (result) {
        this.selectedIcd = result;
        this.addForm.patchValue({ icdId: result.id });
      }
    });
  }

  clearSelectedIcd(): void {
    this.selectedIcd = null;
    this.addForm.patchValue({ icdId: null });
  }

  onShowAddForm(): void {
    this.showAddForm = true;
    this.selectedIcd = null;
    this.addForm.reset();
  }

  onAdd(): void {
    if (!this.addForm.value.icdId) {
      this.snackBar.open('Please select an ICD code first', 'Close', { duration: 3000 });
      return;
    }

    const val = this.addForm.value;
    // Backend expects: icdid (not icdId), DateOfProcedure as date-only string
    const request: any = {
      icdid: val.icdId,
      dateOfProcedure: val.dateOfProcedure ? val.dateOfProcedure.toISOString().split('T')[0] : null,
      primaryCode: val.primaryCode || false,
      secondaryCode: val.secondaryCode || false,
      coMorbidityCode: val.coMorbidityCode || false
    };

    this.caseService.createIcd(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.selectedIcd = null;
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
