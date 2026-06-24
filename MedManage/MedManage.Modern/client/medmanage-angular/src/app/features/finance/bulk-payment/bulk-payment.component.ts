import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatStepperModule } from '@angular/material/stepper';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AgGridModule } from 'ag-grid-angular';
import {
  ColDef,
  GridReadyEvent,
  GridApi,
  IServerSideDatasource,
  IServerSideGetRowsParams,
  SelectionChangedEvent
} from 'ag-grid-community';

import { BillingService } from '../services/billing.service';
import { CaseBillingDto, BillingSearchRequest, BulkPaymentRequest } from '../models/billing.models';

@Component({
  selector: 'app-bulk-payment',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatCardModule,
    MatCheckboxModule,
    MatSnackBarModule,
    MatStepperModule,
    MatProgressSpinnerModule,
    AgGridModule
  ],
  templateUrl: './bulk-payment.component.html',
  styleUrls: ['./bulk-payment.component.scss']
})
export class BulkPaymentComponent {
  private readonly billingService = inject(BillingService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  private gridApi!: GridApi;
  selectedBillings: CaseBillingDto[] = [];
  processing = false;

  /** Search form for finding billings to pay */
  searchForm = this.fb.group({
    providerName: [''],
    accountNumber: [''],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  /** Payment details form */
  paymentForm = this.fb.group({
    amountPaid: [null as number | null, [Validators.required, Validators.min(0.01)]],
    datePaid: [new Date(), Validators.required],
    comments: ['']
  });

  /** ag-Grid columns for selection */
  columnDefs: ColDef[] = [
    { headerCheckboxSelection: true, checkboxSelection: true, width: 50 },
    { field: 'accountNumber', headerName: 'Account #', width: 130 },
    { field: 'invoiceNumber', headerName: 'Invoice #', width: 130 },
    { field: 'caseNumber', headerName: 'Case #', width: 120 },
    { field: 'memberName', headerName: 'Member', flex: 1 },
    { field: 'providerName', headerName: 'Provider', flex: 1 },
    {
      field: 'amount',
      headerName: 'Amount',
      width: 120,
      type: 'numericColumn',
      valueFormatter: p => p.value != null ? `R ${p.value.toFixed(2)}` : ''
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

  rowModelType: 'serverSide' = 'serverSide';

  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
    this.gridApi.setGridOption('serverSideDatasource', this.createDatasource());
  }

  private createDatasource(): IServerSideDatasource {
    return {
      getRows: (params: IServerSideGetRowsParams) => {
        const startRow = params.request.startRow ?? 0;
        const endRow = params.request.endRow ?? 50;
        const pageSize = endRow - startRow;
        const pageNumber = Math.floor(startRow / pageSize) + 1;

        const formValue = this.searchForm.value;
        const searchRequest: BillingSearchRequest = {
          providerName: formValue.providerName || undefined,
          accountNumber: formValue.accountNumber || undefined,
          dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : undefined,
          dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : undefined,
          isPaid: false, // Only show unpaid billings
          pageNumber,
          pageSize
        };

        this.billingService.search(searchRequest).subscribe({
          next: (result) => {
            params.success({
              rowData: result.items,
              rowCount: result.totalCount
            });
          },
          error: () => {
            params.fail();
          }
        });
      }
    };
  }

  onSearch(): void {
    this.gridApi?.refreshServerSide({ purge: true });
  }

  onSelectionChanged(event: SelectionChangedEvent): void {
    this.selectedBillings = this.gridApi.getSelectedRows();
  }

  get totalSelectedAmount(): number {
    return this.selectedBillings.reduce((sum, b) => sum + (b.amount || 0), 0);
  }

  /** Process bulk payment */
  onProcessPayment(): void {
    if (this.paymentForm.invalid || this.selectedBillings.length === 0) return;

    const formVal = this.paymentForm.value;
    const request: BulkPaymentRequest = {
      billingIds: this.selectedBillings.map(b => b.id),
      amountPaid: formVal.amountPaid!,
      datePaid: formVal.datePaid!.toISOString(),
      comments: formVal.comments || undefined
    };

    this.processing = true;
    this.billingService.bulkPayment(request).subscribe({
      next: (result) => {
        this.processing = false;
        this.snackBar.open(
          `Payment processed: ${result.updatedCount} billing(s) updated`,
          'Close',
          { duration: 5000 }
        );
        this.selectedBillings = [];
        this.gridApi?.refreshServerSide({ purge: true });
        this.paymentForm.reset({ datePaid: new Date() });
      },
      error: () => {
        this.processing = false;
        this.snackBar.open('Failed to process bulk payment', 'Close', { duration: 3000 });
      }
    });
  }

  onBack(): void {
    this.router.navigate(['/finance']);
  }
}
