import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { DashboardService, DashboardStats } from '../../core/services/dashboard.service';

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
export class DashboardComponent implements OnInit {
  private readonly router = inject(Router);
  private readonly dashboardService = inject(DashboardService);

  stats: DashboardStats | null = null;
  loading = true;
  error: string | null = null;

  ngOnInit(): void {
    this.loadStats();
  }

  goToMyCases(): void {
    this.router.navigate(['/cases/my-cases']);
  }

  private loadStats(): void {
    this.loading = true;
    this.error = null;

    this.dashboardService.getStats().subscribe({
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
