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
  displayedColumns = ['note', 'interimAmount', 'interimHospital', 'interimRadiology', 'interimDialysis', 'interimSpecialist', 'interimPhysio', 'interimTransport', 'interimAccomodation', 'interimScript', 'dateCreated', 'actions'];
  showAddForm = false;
  editingItem: CaseNoteDto | null = null;

  addForm = this.fb.group({
    note: [''],
    interimHospital: [0],
    interimRadiology: [0],
    interimDialysis: [0],
    interimSpecialist: [0],
    interimPhysio: [0],
    interimTransport: [0],
    interimAccomodation: [0],
    interimScript: [0]
  });

  get interimTotal(): number {
    const v = this.addForm.value;
    return (v.interimHospital || 0) + (v.interimRadiology || 0) + (v.interimDialysis || 0)
      + (v.interimSpecialist || 0) + (v.interimPhysio || 0) + (v.interimTransport || 0)
      + (v.interimAccomodation || 0) + (v.interimScript || 0);
  }

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getNotes(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load notes', 'Close', { duration: 3000 })
    });
  }

  onShowAddForm(): void {
    this.showAddForm = true;
    this.editingItem = null;
    this.addForm.reset({
      interimHospital: 0, interimRadiology: 0, interimDialysis: 0,
      interimSpecialist: 0, interimPhysio: 0, interimTransport: 0,
      interimAccomodation: 0, interimScript: 0
    });
  }

  onEdit(item: CaseNoteDto): void {
    this.showAddForm = true;
    this.editingItem = item;
    this.addForm.patchValue({
      note: item.note || '',
      interimHospital: item.interimHospital || 0,
      interimRadiology: item.interimRadiology || 0,
      interimDialysis: item.interimDialysis || 0,
      interimSpecialist: item.interimSpecialist || 0,
      interimPhysio: item.interimPhysio || 0,
      interimTransport: item.interimTransport || 0,
      interimAccomodation: item.interimAccomodation || 0,
      interimScript: item.interimScript || 0
    });
  }

  onCancel(): void {
    this.showAddForm = false;
    this.editingItem = null;
    this.addForm.reset();
  }

  onSave(): void {
    const val = this.addForm.value;
    const request: CreateCaseNoteRequest = {
      note: val.note || undefined,
      interimAmount: this.interimTotal,
      interimHospital: val.interimHospital || 0,
      interimRadiology: val.interimRadiology || 0,
      interimDialysis: val.interimDialysis || 0,
      interimSpecialist: val.interimSpecialist || 0,
      interimPhysio: val.interimPhysio || 0,
      interimTransport: val.interimTransport || 0,
      interimAccomodation: val.interimAccomodation || 0,
      interimScript: val.interimScript || 0
    };

    const obs = this.editingItem
      ? this.caseService.updateNote(this.caseId, this.editingItem.id, request)
      : this.caseService.createNote(this.caseId, request);

    obs.subscribe({
      next: () => {
        this.loadItems();
        this.addForm.reset();
        this.showAddForm = false;
        this.editingItem = null;
        this.snackBar.open(this.editingItem ? 'Note updated' : 'Note added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to save note', 'Close', { duration: 3000 })
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
