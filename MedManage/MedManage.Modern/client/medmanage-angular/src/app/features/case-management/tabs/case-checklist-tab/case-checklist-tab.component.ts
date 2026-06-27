import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseChecklistDto, CreateCaseChecklistRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-checklist-tab',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule
  ],
  templateUrl: './case-checklist-tab.component.html',
  styleUrls: ['./case-checklist-tab.component.scss']
})
export class CaseChecklistTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseChecklistDto[] = [];
  displayedColumns = ['templateName', 'checked', 'notApplicable', 'comments', 'date'];
  hasPendingChanges = false;

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getChecklist(this.caseId).subscribe({
      next: (items) => {
        this.items = items;
        this.hasPendingChanges = false;
      },
      error: () => this.snackBar.open('Failed to load checklist', 'Close', { duration: 3000 })
    });
  }

  onCheckChanged(item: CaseChecklistDto): void {
    item.checked = !item.checked;
    this.saveRow(item);
  }

  onNaChanged(item: CaseChecklistDto): void {
    item.notApplicable = !item.notApplicable;
    this.saveRow(item);
  }

  onCommentBlur(item: CaseChecklistDto): void {
    this.saveRow(item);
  }

  private saveRow(item: CaseChecklistDto): void {
    const request: CreateCaseChecklistRequest = {
      checked: item.checked ?? false,
      notApplicable: item.notApplicable ?? false,
      comments: item.comments || ''
    };

    this.caseService.updateChecklistItem(this.caseId, item.id, request).subscribe({
      next: () => {},
      error: () => this.snackBar.open('Failed to save checklist item', 'Close', { duration: 3000 })
    });
  }

  saveAll(): void {
    let pending = this.items.length;
    let errors = 0;

    for (const item of this.items) {
      const request: CreateCaseChecklistRequest = {
        checked: item.checked ?? false,
        notApplicable: item.notApplicable ?? false,
        comments: item.comments || ''
      };

      this.caseService.updateChecklistItem(this.caseId, item.id, request).subscribe({
        next: () => {
          pending--;
          if (pending === 0) {
            this.snackBar.open(errors ? `Saved with ${errors} error(s)` : 'All items saved', 'Close', { duration: 3000 });
          }
        },
        error: () => {
          errors++;
          pending--;
          if (pending === 0) {
            this.snackBar.open(`Saved with ${errors} error(s)`, 'Close', { duration: 3000 });
          }
        }
      });
    }
  }
}
