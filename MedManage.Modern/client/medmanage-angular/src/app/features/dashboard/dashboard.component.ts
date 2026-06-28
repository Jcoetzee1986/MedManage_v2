import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Subject, takeUntil } from 'rxjs';
import { DashboardService, DashboardStats } from '../../core/services/dashboard.service';
import { AuthService } from '../../core/services/auth.service';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [
      CommonModule,
      MatButtonModule,
      MatIconModule,
      MatCardModule,
      MatProgressSpinnerModule
    ],
    templateUrl: './dashboard.component.html',
    styles: [`
      .dashboard-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 24px;
      }
      .actions {
        display: flex;
        gap: 12px;
        flex-wrap: wrap;
      }
    `]
})
export class DashboardComponent implements OnInit, OnDestroy {
  private readonly router = inject(Router);
  private readonly dashboardService = inject(DashboardService);
  private readonly authService = inject(AuthService);
  private readonly destroy$ = new Subject<void>();

  stats: DashboardStats | null = null;
  loading = true;
  error: string | null = null;

  ngOnInit(): void {
    // Load stats whenever active client changes (BehaviorSubject fires immediately with current value)
    this.authService.activeClient$
      .pipe(takeUntil(this.destroy$))
      .subscribe(() => {
        this.loadStats();
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  goToMyCases(): void {
    this.router.navigate(['/cases/my-cases']);
  }

  private loadStats(): void {
    this.loading = true;
    this.error = null;

    const clientId = this.authService.activeClientId;
    this.dashboardService.getStats(clientId).subscribe({
      next: (stats) => {
        this.stats = stats;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load dashboard statistics. Please try again later.';
        this.loading = false;
        console.error('Dashboard stats error:', err);
      }
    });
  }
}
