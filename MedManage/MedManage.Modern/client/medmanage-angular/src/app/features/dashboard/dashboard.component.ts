import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { ClientSwitcherComponent } from '../../shared/components/client-switcher/client-switcher.component';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [
      CommonModule,
      MatButtonModule,
      MatIconModule,
      MatCardModule,
      ClientSwitcherComponent
    ],
    templateUrl: './dashboard.component.html',
    styles: [`
      .dashboard {
        padding: 24px;
      }
      .dashboard-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        margin-bottom: 24px;
      }
      .stats {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 16px;
        margin-bottom: 24px;
      }
      .stat-card {
        padding: 16px;
      }
      .stat-value {
        font-size: 1.5rem;
        font-weight: bold;
      }
      .actions {
        display: flex;
        gap: 12px;
        flex-wrap: wrap;
      }
    `]
})
export class DashboardComponent {
  private readonly router = inject(Router);

  goToMyCases(): void {
    this.router.navigate(['/cases/my-cases']);
  }
}
