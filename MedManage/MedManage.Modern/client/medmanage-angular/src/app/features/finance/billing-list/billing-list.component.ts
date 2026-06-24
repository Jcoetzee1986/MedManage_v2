import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { AgGridModule } from 'ag-grid-angular';
import {
  ColDef,
  GridReadyEvent,
  GridApi,
  IServerSideDatasource,
  IServerSideGetRowsParams
} from 'ag-grid-community';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { BillingService } from '../services/billing.service';
import { BillingSearchRequest } from '../models/billing.models';

@Component({
  selector: 'app-billing-list',
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
    AgGridModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './billing-list.component.html',
  styleUrls: ['./billing-list.component.scss']
})
export class BillingListComponent implements OnInit {
  private readonly billingService = inject(BillingService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  private gridApi!: GridApi;

  /** Search filter form */
  searchForm = this.fb.group({
    providerName: [''],
    accountNumber: [''],
    memberName: [''],
    memberNumber: [''],
    billingStatusId: [null as number | null],
    isPaid: [null as boolean | null],
    remittanceNumber: [''],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  /** ag-Grid column definitions */
  columnDefs: ColDef[] = [
    { field: 'accountNumber', headerName: 'Account #', width: 130, sortable: true },
    { field: 'invoiceNumber', headerName: 'Invoice #', width: 130, sortable: true },
    { field: 'caseNumber', headerName: 'Case #', width: 120, sortable: true },
    { field: 'memberName', headerName: 'Member', flex: 1, sortable: true },
    { field: 'providerName', headerName: 'Provider', flex: 1, sortable: true },
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
    { field: 'remittanceNumber', headerName: 'Remittance #', width: 140 },
    {
      field: 'dateReceived',
      headerName: 'Received',
      width: 120,
      valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : ''
    },
    {
      field: 'datePaid',
      headerName: 'Date Paid',
      width: 120,
      valueFormatter: p => p.value ? new Date(p.value).toLocaleDateString() : ''
    }
  ];

  /** ag-Grid default column settings */
  defaultColDef: ColDef = {
    resizable: true,
    filter: false
  };

  /** Server-side row model type */
  rowModelType: 'serverSide' = 'serverSide';

  ngOnInit(): void {}

  onGridReady(params: GridReadyEvent): void {
    this.gridApi = params.api;
    this.gridApi.setGridOption('serverSideDatasource', this.createDatasource());
  }

  /** Create server-side datasource for ag-Grid */
  private createDatasource(): IServerSideDatasource {
    return {
      getRows: (params: IServerSideGetRowsParams) => {
        const startRow = params.request.startRow ?? 0;
        const endRow = params.request.endRow ?? 50;
        const pageSize = endRow - startRow;
        const pageNumber = Math.floor(startRow / pageSize) + 1;

        const sortModel = params.request.sortModel;
        const sortField = sortModel?.length ? sortModel[0].colId : undefined;
        const sortDirection = sortModel?.length ? sortModel[0].sort as 'asc' | 'desc' : undefined;

        const formValue = this.searchForm.value;
        const searchRequest: BillingSearchRequest = {
          providerName: formValue.providerName || undefined,
          accountNumber: formValue.accountNumber || undefined,
          memberName: formValue.memberName || undefined,
          memberNumber: formValue.memberNumber || undefined,
          billingStatusId: formValue.billingStatusId || undefined,
          isPaid: formValue.isPaid ?? undefined,
          remittanceNumber: formValue.remittanceNumber || undefined,
          dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : undefined,
          dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : undefined,
          pageNumber,
          pageSize,
          sortField,
          sortDirection
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

  /** Execute search with current filters */
  onSearch(): void {
    this.gridApi?.refreshServerSide({ purge: true });
  }

  /** Reset search form */
  onReset(): void {
    this.searchForm.reset();
    this.gridApi?.refreshServerSide({ purge: true });
  }

  /** Navigate to billing detail */
  onRowDoubleClicked(event: any): void {
    if (event.data) {
      this.router.navigate(['/finance/billing', event.data.id]);
    }
  }

  /** Navigate to create new billing */
  onNewBilling(): void {
    this.router.navigate(['/finance/billing/new']);
  }

  /** Navigate to bulk payments */
  onBulkPayments(): void {
    this.router.navigate(['/finance/payments']);
  }

  /** Navigate to remittance tracking */
  onRemittance(): void {
    this.router.navigate(['/finance/remittance']);
  }
}
