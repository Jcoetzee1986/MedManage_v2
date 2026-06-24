import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ReferenceDataDropdownComponent } from '../../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { CaseService } from '../../services/case.service';
import { CaseChecklistDto, CreateCaseChecklistRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-checklist-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './case-checklist-tab.component.html',
  styleUrls: ['./case-checklist-tab.component.scss']
})
export class CaseChecklistTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseChecklistDto[] = [];
  displayedColumns = ['templateName', 'checked', 'notApplicable', 'comments', 'date', 'actions'];
  showAddForm = false;

  addForm = this.fb.group({
    templateId: [null as number | null],
    checked: [false],
    notApplicable: [false],
    comments: ['']
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getChecklist(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load checklist', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    const val = this.addForm.value;
    const request: CreateCaseChecklistRequest = {
      templateId: val.templateId || undefined,
      checked: val.checked || false,
      notApplicable: val.notApplicable || false,
      comments: val.comments || undefined
    };

    this.caseService.createChecklistItem(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.showAddForm = false;
        this.snackBar.open('Checklist item added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add checklist item', 'Close', { duration: 3000 })
    });
  }

  onToggleChecked(item: CaseChecklistDto): void {
    const request: CreateCaseChecklistRequest = {
      templateId: item.templateId,
      checked: !item.checked,
      notApplicable: item.notApplicable,
      comments: item.comments
    };

    this.caseService.updateChecklistItem(this.caseId, item.id, request).subscribe({
      next: () => this.loadItems(),
      error: () => this.snackBar.open('Failed to update', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseChecklistDto): void {
    if (confirm('Delete this checklist item?')) {
      this.caseService.deleteChecklistItem(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Item deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
