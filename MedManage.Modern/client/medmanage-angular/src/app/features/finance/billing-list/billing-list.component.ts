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
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { DataTableComponent, DataTableColumn } from '../../../shared/components/data-table/data-table.component';
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
    DataTableComponent,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './billing-list.component.html',
  styleUrls: ['./billing-list.component.scss']
})
export class BillingListComponent implements OnInit {
  private readonly billingService = inject(BillingService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  selectedRow: any = null;

  /** Pagination */
  totalCount = 0;
  pageSize = 30;
  pageIndex = 0;

  /** Sort state */
  currentSortBy?: string;
  currentSortDescending?: boolean;

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
    { field: 'remittanceNumber', header: 'Remittance #', width: '140px' },
    { field: 'dateReceived', header: 'Received', width: '110px', pipe: 'date' },
    { field: 'datePaid', header: 'Date Paid', width: '110px', pipe: 'date' }
  ];

  /** Row data */
  rowData: any[] = [];

  ngOnInit(): void {
    this.loadBillings();
  }

  /** Load billings from the API */
  private loadBillings(): void {
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
      pageNumber: this.pageIndex + 1,
      pageSize: this.pageSize,
      sortField: this.currentSortBy,
      sortDirection: this.currentSortDescending === true ? 'desc' : this.currentSortDescending === false ? 'asc' : undefined
    };

    this.billingService.search(searchRequest).subscribe({
      next: (result) => {
        this.rowData = result.items;
        this.totalCount = result.totalCount;
      },
      error: () => {
        this.rowData = [];
        this.totalCount = 0;
      }
    });
  }

  /** Execute search with current filters */
  onSearch(): void {
    this.pageIndex = 0;
    this.loadBillings();
  }

  /** Handle page changes */
  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadBillings();
  }

  /** Handle sort change */
  onSortChange(sort: Sort): void {
    if (sort.direction) {
      this.currentSortBy = sort.active;
      this.currentSortDescending = sort.direction === 'desc';
    } else {
      this.currentSortBy = undefined;
      this.currentSortDescending = undefined;
    }
    this.pageIndex = 0;
    this.loadBillings();
  }

  /** Reset search form */
  onReset(): void {
    this.searchForm.reset();
    this.pageIndex = 0;
    this.currentSortBy = undefined;
    this.currentSortDescending = undefined;
    this.loadBillings();
  }

  /** Navigate to billing detail */
  onRowDoubleClicked(row: any): void {
    if (row) {
      this.router.navigate(['/finance/billing', row.id]);
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
