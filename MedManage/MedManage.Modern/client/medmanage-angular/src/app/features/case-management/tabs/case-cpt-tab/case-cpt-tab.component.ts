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
import { CaseCptDto, CreateCaseCptRequest } from '../../models/case.models';

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
    MatSnackBarModule
  ],
  templateUrl: './case-cpt-tab.component.html',
  styleUrls: ['./case-cpt-tab.component.scss']
})
export class CaseCptTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseCptDto[] = [];
  displayedColumns = ['cptCode', 'cptDescription', 'dateOfProcedure', 'primaryCode', 'secondaryCode', 'actions'];
  showAddForm = false;

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

  onAdd(): void {
    const val = this.addForm.value;
    const request: CreateCaseCptRequest = {
      cptId: val.cptId || undefined,
      dateOfProcedure: val.dateOfProcedure?.toISOString(),
      primaryCode: val.primaryCode || false,
      secondaryCode: val.secondaryCode || false
    };

    this.caseService.createCpt(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
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
