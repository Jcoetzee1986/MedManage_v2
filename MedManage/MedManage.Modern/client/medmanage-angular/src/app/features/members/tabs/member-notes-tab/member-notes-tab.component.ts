import { Component, Input, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
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
  displayedColumns = ['note', 'createdBy', 'dateCreated', 'actions'];
  showForm = false;
  editingId: number | null = null;

  form = this.fb.group({
    note: ['', Validators.required]
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
    this.form.reset();
  }

  onEdit(item: MemberNoteDto): void {
    this.showForm = true;
    this.editingId = item.id;
    this.form.patchValue({ note: item.note });
  }

  onCancel(): void {
    this.showForm = false;
    this.editingId = null;
    this.form.reset();
  }

  onSave(): void {
    if (this.form.invalid) return;

    const request: CreateMemberNoteRequest = {
      note: this.form.value.note!
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
        this.snackBar.open('Saved successfully', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: MemberNoteDto): void {
    if (confirm('Are you sure you want to delete this note?')) {
      this.memberService.deleteNote(this.memberId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
