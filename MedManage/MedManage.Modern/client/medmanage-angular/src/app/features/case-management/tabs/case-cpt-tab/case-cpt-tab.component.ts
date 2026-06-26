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
import { CaseCptDto, CreateCaseCptRequest } from '../../models/case.models';
import { CodeLookupDialogComponent } from '../../../../shared/components/code-lookup-dialog/code-lookup-dialog.component';
import { CodeLookupResult } from '../../../../core/services/code-lookup.service';

@Component({
  selector: 'app-case-cpt-tab',
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
  templateUrl: './case-cpt-tab.component.html',
  styleUrls: ['./case-cpt-tab.component.scss']
})
export class CaseCptTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

  items: CaseCptDto[] = [];
  displayedColumns = ['cptCode', 'cptDescription', 'dateOfProcedure', 'primaryCode', 'secondaryCode', 'actions'];
  showAddForm = false;
  selectedCpt: CodeLookupResult | null = null;

  addForm = this.fb.group({
    cptId: [null as number | null],
    dateOfProcedure: [null as Date | null],
    primaryCode: [false],
    secondaryCode: [false]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getCptCodes(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load CPT codes', 'Close', { duration: 3000 })
    });
  }

  onOpenLookup(): void {
    const dialogRef = this.dialog.open(CodeLookupDialogComponent, {
      width: '700px',
      data: { codeType: 'cpt', title: 'CPT Lookup' }
    });

    dialogRef.afterClosed().subscribe((result: CodeLookupResult | null) => {
      if (result) {
        this.selectedCpt = result;
        this.addForm.patchValue({ cptId: result.id });
      }
    });
  }

  clearSelectedCpt(): void {
    this.selectedCpt = null;
    this.addForm.patchValue({ cptId: null });
  }

  onShowAddForm(): void {
    this.showAddForm = true;
    this.selectedCpt = null;
    this.addForm.reset();
  }

  onAdd(): void {
    if (!this.addForm.value.cptId) {
      this.snackBar.open('Please select a CPT code first', 'Close', { duration: 3000 });
      return;
    }

    const val = this.addForm.value;
    // Backend expects: Cptid (not cptId), DateOfProcedure as date-only string (YYYY-MM-DD)
    const request: any = {
      cptid: val.cptId,
      dateOfProcedure: val.dateOfProcedure ? val.dateOfProcedure.toISOString().split('T')[0] : null,
      primaryCode: val.primaryCode || false,
      secondaryCode: val.secondaryCode || false
    };

    this.caseService.createCpt(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.selectedCpt = null;
        this.showAddForm = false;
        this.snackBar.open('CPT code added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add CPT code', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseCptDto): void {
    if (confirm('Delete this CPT code?')) {
      this.caseService.deleteCpt(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('CPT code deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete CPT code', 'Close', { duration: 3000 })
      });
    }
  }
}
