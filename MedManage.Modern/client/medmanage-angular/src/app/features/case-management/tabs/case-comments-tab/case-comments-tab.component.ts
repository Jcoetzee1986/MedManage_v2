import { Component, Input, inject, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CaseService } from '../../services/case.service';
import { CaseCommentDto, CreateCaseCommentRequest } from '../../models/case.models';

@Component({
  selector: 'app-case-comments-tab',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatSnackBarModule,
    MatTooltipModule
  ],
  templateUrl: './case-comments-tab.component.html',
  styleUrls: ['./case-comments-tab.component.scss']
})
export class CaseCommentsTabComponent implements OnInit {
  @Input({ required: true }) caseId!: number;
  @ViewChild('editorEl') editorEl!: ElementRef<HTMLDivElement>;

  private readonly caseService = inject(CaseService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  items: CaseCommentDto[] = [];
  editorContent = '';

  addForm = this.fb.group({
    text: ['']
  });

  ngOnInit(): void {
    this.loadItems();
  }

  private loadItems(): void {
    this.caseService.getComments(this.caseId).subscribe({
      next: (items) => this.items = items,
      error: () => this.snackBar.open('Failed to load comments', 'Close', { duration: 3000 })
    });
  }

  onEditorInput(): void {
    this.editorContent = this.editorEl?.nativeElement?.innerHTML || '';
  }

  execCmd(command: string): void {
    document.execCommand(command, false);
    this.editorEl?.nativeElement?.focus();
  }

  onAdd(): void {
    const html = this.editorEl?.nativeElement?.innerHTML?.trim();
    if (!html || html === '<br>') return;

    const request: CreateCaseCommentRequest = { text: html };

    this.caseService.createComment(this.caseId, request).subscribe({
      next: () => {
        this.loadItems();
        this.editorEl.nativeElement.innerHTML = '';
        this.editorContent = '';
        this.snackBar.open('Comment added', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to add comment', 'Close', { duration: 3000 })
    });
  }

  onDelete(item: CaseCommentDto): void {
    if (confirm('Delete this comment?')) {
      this.caseService.deleteComment(this.caseId, item.id).subscribe({
        next: () => {
          this.loadItems();
          this.snackBar.open('Comment deleted', 'Close', { duration: 3000 });
        },
        error: () => this.snackBar.open('Failed to delete', 'Close', { duration: 3000 })
      });
    }
  }
}
