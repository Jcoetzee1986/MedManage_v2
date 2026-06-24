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
import { AgGridModule } from 'ag-grid-angular';
import { ColDef, GridReadyEvent, GridApi } from 'ag-grid-community';
import { BillingService } from '../services/billing.service';
import { CaseBillingDto } from '../models/billing.models';

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
    AgGridModule
  ],
  templateUrl: './remittance.component.html',
  styleUrls: ['./remittance.component.scss']
})
export class RemittanceComponent {
  private readonly billingService = inject(BillingService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  private gridApi!: GridApi;
  loading = false;
  billings: CaseBillingDto[] = [];
  searchedRemittance = '';

  /** Search form */
  searchForm = this.fb.group({
    remittanceNumber: ['', Validators.required]
  });

  /** ag-Grid column definitions */
  columnDefs: ColDef[] = [
    { field: 'accountNumber', headerName: 'Account #', width: 130 },
    { field: 'invoiceNumber', headerName: 'Invoice #', width: 130 },
    { field: 'caseNumber', headerName: 'Case #', width: 120 },
    { field: 'memberName', headerName: 'Member', flex: 1 },
    { field: 'providerName', headerName: 'Provider', flex: 1 },
    { field: 'billingStatusName', headerName: 'Status', width: 120 },
    {
      field: 'amount',
      headerName: 'Amount',
      width: 120,
      type: 'numericColumn',
      valueFormatter: p => p.value != null ? `R ${p.value.toFixed(2)}` : ''
    },
    {
      field: 'amountPaid',
      headerName: 'Paid',
      width: 120,
      type: 'numericColumn',
      valueFormatter: p => p.value != null ? `R ${p.value.toFixed(2)}` : ''
    },
    {
      field: 'datePaid',
      headerName: 'Date Paid',
      width: 120,
      valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : ''
    },
    {
      field: 'dateReceived',
      headerName: 'Received',
      width: 120,
      valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : ''
    }
  ];

  defaultColDef: ColDef = {
    resizable: true,
    filter: false,
    sortable: true
  };

  get totalAmount(): number {
    return this.billings.reduce((sum, b) => sum + (b.amount || 0), 0);
  }

  get totalPaid(): number {
    return this.billings.reduce((sum, b) => sum + (b.amountPaid || 0), 0);
  }

  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
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
  onRowDoubleClicked(event: any): void {
    if (event.data) {
      this.router.navigate(['/finance/billing', event.data.id]);
    }
  }

  onBack(): void {
    this.router.navigate(['/finance']);
  }
}
