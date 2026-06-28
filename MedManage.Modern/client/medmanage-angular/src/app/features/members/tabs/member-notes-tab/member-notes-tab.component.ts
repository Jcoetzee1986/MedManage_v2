import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MemberService } from '../../services/member.service';
import { MemberNoteDto, CreateMemberNoteRequest } from '../../models/member.models';

@Component({
  selector: 'app-member-notes-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSnackBarModule
  ],
  templateUrl: './member-notes-tab.component.html',
  styleUrls: ['./member-notes-tab.component.scss']
})
export class MemberNotesTabComponent implements OnInit {
  @Input() memberId!: number;

  private readonly memberService = inject(MemberService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: MemberNoteDto[] = [];
  displayedColumns = ['note', 'noteDate', 'createdBy', 'dateCreated', 'actions'];
  showForm = false;
  editingId: number | null = null;

  form = this.fb.group({
    note: ['', Validators.required],
    noteDate: [new Date() as Date | null]
  });

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.memberService.getNotes(this.memberId).subscribe({
      next: (data) => this.items = data,
      error: () => this.snackBar.open('Failed to load notes', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    this.showForm = true;
    this.editingId = null;
    this.form.reset({ note: '', noteDate: new Date() });
  }

  onEdit(item: MemberNoteDto): void {
    this.showForm = true;
    this.editingId = item.id;
    this.form.patchValue({
      note: item.note,
      noteDate: item.noteDate ? new Date(item.noteDate) : new Date()
    });
  }

  onCancel(): void {
    this.showForm = false;
    this.editingId = null;
    this.form.reset();
  }

  onSave(): void {
    if (this.form.invalid) return;

    const formValue = this.form.value;
    const request: CreateMemberNoteRequest = {
      note: formValue.note!,
      noteDate: formValue.noteDate ? formValue.noteDate.toISOString() : null
    };

    const obs = this.editingId
      ? this.memberService.updateNote(this.memberId, this.editingId, request)
      : this.memberService.createNote(this.memberId, request);

    obs.subscribe({
      next: () => {
        this.showForm = false;
        this.editingId = null;
        this.form.reset();
        this.loadItems();
        this.snackBar.open('Note saved', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save note', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: MemberNoteDto): void {
    if (confirm('Are you sure you want to delete this note?')) {
      this.memberService.deleteNote(this.memberId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Note deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete note', 'Close', { duration: 3000 })
      });
    }
  }
}
