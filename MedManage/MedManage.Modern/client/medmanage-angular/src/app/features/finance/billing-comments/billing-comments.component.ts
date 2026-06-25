import { Component, inject, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { BillingService } from '../services/billing.service';
import { BillingCommentDto } from '../models/billing.models';

@Component({
  selector: 'app-billing-comments',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <mat-card class="billing-comments">
      <mat-card-header>
        <mat-card-title>Billing Comments</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        @if (loading) {
          <div class="loading-spinner">
            <mat-spinner diameter="30"></mat-spinner>
          </div>
        } @else {
          <div class="add-comment">
            <mat-form-field appearance="outline" class="comment-input">
              <mat-label>Add a comment</mat-label>
              <input matInput [(ngModel)]="newComment" placeholder="Type your comment..."
                     (keyup.enter)="addComment()">
            </mat-form-field>
            <button mat-mini-fab color="primary" (click)="addComment()" [disabled]="!newComment.trim()">
              <mat-icon>send</mat-icon>
            </button>
          </div>

          @if (comments.length === 0) {
            <p class="no-comments">No comments yet.</p>
          } @else {
            <mat-list>
              @for (comment of comments; track comment.caseBillingCommentId) {
                <mat-list-item class="comment-item">
                  <div class="comment-content">
                    <span class="comment-text">{{ comment.comment }}</span>
                    <span class="comment-meta">
                      {{ comment.userID }} &mdash; {{ comment.dateInserted | date:'short' }}
                    </span>
                  </div>
                  <button mat-icon-button color="warn" (click)="deleteComment(comment.caseBillingCommentId)">
                    <mat-icon>delete</mat-icon>
                  </button>
                </mat-list-item>
              }
            </mat-list>
          }
        }
      </mat-card-content>
    </mat-card>
  `,
  styles: [`
    .billing-comments {
      margin-top: 16px;
    }
    .add-comment {
      display: flex;
      align-items: center;
      gap: 8px;
      margin-bottom: 8px;
    }
    .comment-input {
      flex: 1;
    }
    .comment-item {
      display: flex;
      align-items: center;
      justify-content: space-between;
    }
    .comment-content {
      display: flex;
      flex-direction: column;
      flex: 1;
    }
    .comment-text {
      font-size: 0.9rem;
    }
    .comment-meta {
      font-size: 0.75rem;
      color: #666;
    }
    .no-comments {
      color: #888;
      font-style: italic;
    }
    ::ng-deep .comment-input .mat-mdc-form-field-subscript-wrapper {
      display: none;
    }
  `]
})
export class BillingCommentsComponent implements OnInit, OnChanges {
  private readonly billingService = inject(BillingService);
  private readonly snackBar = inject(MatSnackBar);

  @Input() billingId: number | null = null;

  comments: BillingCommentDto[] = [];
  newComment = '';
  loading = false;

  ngOnInit(): void {
    this.loadComments();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['billingId'] && !changes['billingId'].firstChange) {
      this.loadComments();
    }
  }

  private loadComments(): void {
    if (!this.billingId) return;

    this.loading = true;
    this.billingService.getComments(this.billingId).subscribe({
      next: (comments) => {
        this.comments = comments;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  addComment(): void {
    if (!this.billingId || !this.newComment.trim()) return;

    this.billingService.addComment(this.billingId, { comment: this.newComment.trim() }).subscribe({
      next: (comment) => {
        this.comments.unshift(comment);
        this.newComment = '';
        this.snackBar.open('Comment added', 'Close', { duration: 2000 });
      },
      error: () => {
        this.snackBar.open('Failed to add comment', 'Close', { duration: 3000 });
      }
    });
  }

  deleteComment(commentId: number): void {
    if (!this.billingId) return;

    this.billingService.deleteComment(this.billingId, commentId).subscribe({
      next: () => {
        this.comments = this.comments.filter(c => c.caseBillingCommentId !== commentId);
        this.snackBar.open('Comment deleted', 'Close', { duration: 2000 });
      },
      error: () => {
        this.snackBar.open('Failed to delete comment', 'Close', { duration: 3000 });
      }
    });
  }
}
