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
import { MemberChronicIllnessDto, CreateMemberChronicIllnessRequest } from '../../models/member.models';

@Component({
  selector: 'app-member-chronic-illness-tab',
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
  templateUrl: './member-chronic-illness-tab.component.html',
  styleUrls: ['./member-chronic-illness-tab.component.scss']
})
export class MemberChronicIllnessTabComponent implements OnInit {
  @Input() memberId!: number;

  private readonly memberService = inject(MemberService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: MemberChronicIllnessDto[] = [];
  displayedColumns = ['chronicIllnessName', 'dateFrom', 'dateTo', 'comments', 'actions'];
  showForm = false;
  editingId: number | null = null;

  form = this.fb.group({
    chronicIllnessId: [null as number | null, Validators.required],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null],
    comments: ['']
  });

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.memberService.getChronicIllnesses(this.memberId).subscribe({
      next: (data) => this.items = data,
      error: () => this.snackBar.open('Failed to load chronic illnesses', 'Close', { duration: 3000 })
    });
  }

  onAdd(): void {
    this.showForm = true;
    this.editingId = null;
    this.form.reset();
  }

  onEdit(item: MemberChronicIllnessDto): void {
    this.showForm = true;
    this.editingId = item.id;
    this.form.patchValue({
      chronicIllnessId: item.chronicIllnessId,
      dateFrom: item.dateFrom ? new Date(item.dateFrom) : null,
      dateTo: item.dateTo ? new Date(item.dateTo) : null,
      comments: item.comments || ''
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
    const request: CreateMemberChronicIllnessRequest = {
      chronicIllnessId: formValue.chronicIllnessId!,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : null,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : null,
      comments: formValue.comments || null
    };

    const obs = this.editingId
      ? this.memberService.updateChronicIllness(this.memberId, this.editingId, request)
      : this.memberService.createChronicIllness(this.memberId, request);

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

  onDelete(item: MemberChronicIllnessDto): void {
    if (confirm('Are you sure you want to remove this chronic illness?')) {
      this.memberService.deleteChronicIllness(this.memberId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Deleted successfully', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
