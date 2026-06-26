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
  templateUrl: './client-switcher.component.html',
  styleUrls: ['./client-switcher.component.scss']
})
export class ClientSwitcherComponent implements OnInit {
  private readonly authService = inject(AuthService);
  private readonly snackBar = inject(MatSnackBar);

  clients: AvailableClientDto[] = [];
  selectedClientId: number | null = null;
  loading = false;

  ngOnInit(): void {
    this.selectedClientId = this.authService.activeClientId;
    this.loadClients();
  }

  private loadClients(): void {
    this.loading = true;
    this.authService.getAvailableClients().subscribe({
      next: (clients) => {
        this.clients = clients;
        if (clients.length > 0) {
          const storedId = this.selectedClientId;
          if (storedId && clients.some(c => c.mainClientId === storedId)) {
            this.selectedClientId = storedId;
          } else {
            this.selectedClientId = clients[0].mainClientId;
            this.authService.setActiveClient(this.selectedClientId);
          }
        }
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onClientChange(mainClientId: number): void {
    // Always update the local state immediately
    this.selectedClientId = mainClientId;
    this.authService.setActiveClient(mainClientId);

    // Try to get a new token with client context (optional — may fail without auth)
    this.authService.switchClient({ mainClientId }).subscribe({
      next: (response) => {
        if (response.success) {
          this.snackBar.open(response.message || 'Client switched successfully', 'Close', { duration: 3000 });
        }
      },
      error: () => {
        // Still switched locally even if API call fails (dev mode without auth)
      }
    });
  }
}
