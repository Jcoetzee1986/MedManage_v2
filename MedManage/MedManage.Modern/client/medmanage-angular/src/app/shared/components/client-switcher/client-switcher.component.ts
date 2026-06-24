import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';
import { AvailableClientDto } from '../../../core/models/auth.models';

@Component({
  selector: 'app-client-switcher',
  standalone: true,
  imports: [
    CommonModule,
    MatSelectModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    MatSnackBarModule
  ],
  template: `
    <div class="client-switcher">
      @if (loading) {
        <mat-spinner diameter="20"></mat-spinner>
      } @else if (clients.length > 0) {
        <mat-form-field appearance="outline" class="client-select">
          <mat-label>Active Client</mat-label>
          <mat-select [value]="selectedClientId" (selectionChange)="onClientChange($event.value)">
            @for (client of clients; track client.mainClientId) {
              <mat-option [value]="client.mainClientId">
                {{ client.mainClientName }}
              </mat-option>
            }
          </mat-select>
        </mat-form-field>
      }
    </div>
  `,
  styles: [`
    .client-switcher {
      display: inline-flex;
      align-items: center;
    }
    .client-select {
      min-width: 180px;
      font-size: 0.85rem;
    }
    ::ng-deep .client-select .mat-mdc-form-field-subscript-wrapper {
      display: none;
    }
  `]
})
export class ClientSwitcherComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly snackBar = inject(MatSnackBar);

  clients: AvailableClientDto[] = [];
  selectedClientId: number | null = null;
  loading = false;

  ngOnInit(): void {
    this.loadClients();
  }

  private loadClients(): void {
    this.loading = true;
    this.authService.getAvailableClients().subscribe({
      next: (clients) => {
        this.clients = clients;
        if (clients.length > 0) {
          this.selectedClientId = clients[0].mainClientId;
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onClientChange(mainClientId: number): void {
    this.authService.switchClient({ mainClientId }).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open(response.message || 'Client switched successfully', 'Close', { duration: 3000 });
          this.selectedClientId = mainClientId;
        } else {
          this.snackBar.open(response.message || 'Failed to switch client', 'Close', { duration: 3000 });
        }
      },
      error: () => {
        this.snackBar.open('Failed to switch client', 'Close', { duration: 3000 });
      }
    });
  }
}
