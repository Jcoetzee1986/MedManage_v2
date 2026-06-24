import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReferenceDataDropdownComponent } from '../../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { CaseService } from '../../services/case.service';
import { CaseExclusionDto, CreateCaseExclusionRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-exclusions-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './case-exclusions-tab.component.html',
  styleUrls: ['./case-exclusions-tab.component.scss']
})
export class CaseExclusionsTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseExclusionDto[] = [];
  displayedColumns = ['exclusionName', 'comment', 'actions'];
  showAddForm = false;

  addForm = this.fb.group({
    exclusionId: [null as number | null],
    comment: ['']
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getExclusions(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load exclusions', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    const val = this.addForm.value;
    const request: CreateCaseExclusionRequest = {
      exclusionId: val.exclusionId || undefined,
      comment: val.comment || undefined
    };

    this.caseService.createExclusion(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.showAddForm = false;
        this.snackBar.open('Exclusion added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add exclusion', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseExclusionDto): void {
    if (confirm('Delete this exclusion?')) {
      this.caseService.deleteExclusion(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Exclusion deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
