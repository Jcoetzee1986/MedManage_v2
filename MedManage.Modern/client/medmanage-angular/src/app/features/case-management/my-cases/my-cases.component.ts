import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CaseService } from '../services/case.service';
import { CaseDto } from '../models/case.models';

@Component({
  selector: 'app-my-cases',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  template: `
    <div class="my-cases-container">
      <mat-card>
        <mat-card-header>
          <mat-card-title>
            <mat-icon>assignment_ind</mat-icon> My Cases
          </mat-card-title>
        </mat-card-header>
        <mat-card-content>
          @if (loading) {
            <div class="loading-spinner">
              <mat-spinner diameter="40"></mat-spinner>
            </div>
          } @else if (cases.length === 0) {
            <p class="no-data">You have no cases assigned.</p>
          } @else {
            <table mat-table [dataSource]="cases" class="mat-elevation-z1 full-width">
              <ng-container matColumnDef="caseNumber">
                <th mat-header-cell *matHeaderCellDef>Case #</th>
                <td mat-cell *matCellDef="let row">{{ row.caseNumber || row.authNumber || row.id }}</td>
              </ng-container>

              <ng-container matColumnDef="memberName">
                <th mat-header-cell *matHeaderCellDef>Member</th>
                <td mat-cell *matCellDef="let row">{{ row.memberName || '—' }}</td>
              </ng-container>

              <ng-container matColumnDef="status">
                <th mat-header-cell *matHeaderCellDef>Status</th>
                <td mat-cell *matCellDef="let row">{{ row.caseStatusName || '—' }}</td>
              </ng-container>

              <ng-container matColumnDef="dateCreated">
                <th mat-header-cell *matHeaderCellDef>Created</th>
                <td mat-cell *matCellDef="let row">{{ row.dateCreated | date:'shortDate' }}</td>
              </ng-container>

              <ng-container matColumnDef="actions">
                <th mat-header-cell *matHeaderCellDef></th>
                <td mat-cell *matCellDef="let row">
                  <button mat-icon-button color="primary" (click)="openCase(row.id)">
                    <mat-icon>open_in_new</mat-icon>
                  </button>
                </td>
              </ng-container>

              <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
              <tr mat-row *matRowDef="let row; columns: displayedColumns;" (click)="openCase(row.id)" class="clickable-row"></tr>
            </table>
          }
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .my-cases-container {
      padding: 24px;
    }
    mat-card-title {
      display: flex;
      align-items: center;
      gap: 8px;
    }
  `]
})
export class MyCasesComponent implements OnInit {
  private readonly caseService = inject(CaseService);
  private readonly router = inject(Router);

  cases: CaseDto[] = [];
  loading = false;
  displayedColumns = ['caseNumber', 'memberName', 'status', 'dateCreated', 'actions'];

  ngOnInit(): void {
    this.loadMyCases();
  }

  private loadMyCases(): void {
    this.loading = true;
    this.caseService.getMyCases().subscribe({
      next: (cases) => {
        this.cases = cases;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  openCase(id: number): void {
    this.router.navigate(['/cases', id]);
  }
}
