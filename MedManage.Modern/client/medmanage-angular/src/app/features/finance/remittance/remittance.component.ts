import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDividerModule } from '@angular/material/divider';
import { BillingService } from '../services/billing.service';
import { CaseBillingDto } from '../models/billing.models';
import { DataTableComponent, DataTableColumn } from '../../../shared/components/data-table/data-table.component';

@Component({
  selector: 'app-remittance',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    MatDividerModule,
    DataTableComponent
  ],
  templateUrl: './remittance.component.html',
  styleUrls: ['./remittance.component.scss']
})
export class RemittanceComponent {
  private readonly billingService = inject(BillingService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  loading = false;
  billings: CaseBillingDto[] = [];
  searchedRemittance = '';

  /** Search form */
  searchForm = this.fb.group({
    remittanceNumber: ['', Validators.required]
  });

  /** Column definitions for DataTableComponent */
  tableColumnDefs: DataTableColumn[] = [
    { field: 'accountNumber', header: 'Account #', width: '130px' },
    { field: 'invoiceNumber', header: 'Invoice #', width: '130px' },
    { field: 'caseNumber', header: 'Case #', width: '120px' },
    { field: 'memberName', header: 'Member' },
    { field: 'providerName', header: 'Provider' },
    { field: 'billingStatusName', header: 'Status', width: '120px' },
    { field: 'amount', header: 'Amount', width: '120px', pipe: 'currency', align: 'right' },
    { field: 'amountPaid', header: 'Paid', width: '120px', pipe: 'currency', align: 'right' },
    { field: 'datePaid', header: 'Date Paid', width: '110px', pipe: 'date' },
    { field: 'dateReceived', header: 'Received', width: '110px', pipe: 'date' }
  ];

  get totalAmount(): number {
    return this.billings.reduce((sum, b) => sum + (b.amount || 0), 0);
  }

  get totalPaid(): number {
    return this.billings.reduce((sum, b) => sum + (b.amountPaid || 0), 0);
  }

  /** Search for billings by remittance number */
  onSearch(): void {
    if (this.searchForm.invalid) return;

    const remittanceNumber = this.searchForm.value.remittanceNumber!;
    this.loading = true;
    this.searchedRemittance = remittanceNumber;

    this.billingService.getByRemittance(remittanceNumber).subscribe({
      next: (results) => {
        this.billings = results;
        this.loading = false;
        if (results.length === 0) {
          this.snackBar.open('No billings found for this remittance number', 'Close', { duration: 3000 });
        }
      },
      error: () => {
        this.loading = false;
        this.snackBar.open('Failed to search remittance', 'Close', { duration: 3000 });
      }
    });
  }

  /** Navigate to specific billing record */
  onRowDoubleClicked(row: any): void {
    if (row) {
      this.router.navigate(['/finance/billing', row.id]);
    }
  }

  onBack(): void {
    this.router.navigate(['/finance']);
  }
}
