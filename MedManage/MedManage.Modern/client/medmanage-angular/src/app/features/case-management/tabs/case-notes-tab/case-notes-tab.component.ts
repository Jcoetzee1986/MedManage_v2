import { Component, Input, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { CaseService } from '../../services/case.service';
import { CaseNoteDto, CreateCaseNoteRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-notes-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule
  ],
  templateUrl: './case-notes-tab.component.html',
  styleUrls: ['./case-notes-tab.component.scss']
})
export class CaseNotesTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseNoteDto[] = [];
  displayedColumns = ['note', 'interimAmount1', 'interimAmount2', 'dateCreated', 'createdBy', 'actions'];
  showAddForm = false;

  addForm = this.fb.group({
    note: [''],
    interimAmount1: [null as number | null],
    interimAmount2: [null as number | null],
    interimAmount3: [null as number | null],
    interimAmount4: [null as number | null],
    interimAmount5: [null as number | null],
    interimAmount6: [null as number | null],
    interimAmount7: [null as number | null],
    interimAmount8: [null as number | null]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getNotes(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load notes', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    const val = this.addForm.value;
    const request: CreateCaseNoteRequest = {
      note: val.note || undefined,
      interimAmount1: val.interimAmount1 || undefined,
      interimAmount2: val.interimAmount2 || undefined,
      interimAmount3: val.interimAmount3 || undefined,
      interimAmount4: val.interimAmount4 || undefined,
      interimAmount5: val.interimAmount5 || undefined,
      interimAmount6: val.interimAmount6 || undefined,
      interimAmount7: val.interimAmount7 || undefined,
      interimAmount8: val.interimAmount8 || undefined
    };

    this.caseService.createNote(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.showAddForm = false;
        this.snackBar.open('Note added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add note', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseNoteDto): void {
    if (confirm('Delete this note?')) {
      this.caseService.deleteNote(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Note deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
