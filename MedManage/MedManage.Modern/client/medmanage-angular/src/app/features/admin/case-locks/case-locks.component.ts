import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { environment } from '../../../../environments/environment';

interface CaseLock {
  caseId: number;
  userId: string;
  userName: string;
  lockedAt: string;
  lastActivity: string;
}

interface ApiResponse<T> { success: boolean; data: T; message?: string; }

@Component({
  selector: 'app-case-locks',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatCardModule
  ],
  templateUrl: './case-locks.component.html',
  styleUrls: ['./case-locks.component.scss']
})
export class CaseLocksComponent implements OnInit {
  private readonly http = inject(HttpClient);
  private readonly snackBar = inject(MatSnackBar);

  locks: CaseLock[] = [];
  displayedColumns = ['caseId', 'userName', 'lockedAt', 'lastActivity', 'actions'];
  loading = false;

  ngOnInit(): void {
    this.loadLocks();
  }

  loadLocks(): void {
    this.loading = true;
    this.http.get<ApiResponse<CaseLock[]>>(`${environment.apiUrl}/admin/locks`).subscribe({
      next: (r) => {
        this.locks = r.data || [];
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to load locks', 'Close', { duration: 3000 });
      }
    });
  }

  releaseLock(lock: CaseLock): void {
    if (!confirm(`Release lock on case ${lock.caseId} (held by ${lock.userName})?`)) return;
    this.http.delete<ApiResponse<boolean>>(`${environment.apiUrl}/admin/locks/${lock.caseId}`).subscribe({
      next: () => {
        this.loadLocks();
        this.snackBar.open('Lock released', 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to release lock', 'Close', { duration: 3000 })
    });
  }

  releaseAll(): void {
    if (!confirm(`Release ALL ${this.locks.length} active locks?`)) return;
    this.http.delete<ApiResponse<number>>(`${environment.apiUrl}/admin/locks`).subscribe({
      next: (r) => {
        this.loadLocks();
        this.snackBar.open(`Released ${r.data} lock(s)`, 'Close', { duration: 3000 });
      },
      error: () => this.snackBar.open('Failed to release locks', 'Close', { duration: 3000 })
    });
  }
}
