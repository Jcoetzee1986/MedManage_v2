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
import { MatTableModule } from '@angular/material/table';

import { BillingService } from '../services/billing.service';
import { CaseBillingDto, BillingSearchRequest, BulkPaymentRequest, RemittanceUpdateRequest } from '../models/billing.models';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';

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
    MatTableModule,
    ReferenceDataDropdownComponent
  ],
  templateUrl: './bulk-payment.component.html',
  styleUrls: ['./bulk-payment.component.scss']
})
export class BulkPaymentComponent {
  private readonly billingService = inject(BillingService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);

  selectedBillings: CaseBillingDto[] = [];
  processing = false;

  /** Columns for the mat-table */
  displayedColumns = ['select', 'accountNumber', 'invoiceNumber', 'caseNumber', 'memberName', 'providerName', 'amount', 'dateReceived'];

  /** Search form for finding billings to pay */
  searchForm = this.fb.group({
    providerName: [''],
    accountNumber: [''],
    billingStatusId: [null as number | null],
    caseStatusId: [null as number | null],
    dateFrom: [null as Date | null],
    dateTo: [null as Date | null]
  });

  /** Payment details form */
  paymentForm = this.fb.group({
    amountPaid: [null as number | null, [Validators.required, Validators.min(0.01)]],
    datePaid: [new Date(), Validators.required],
    comments: ['']
  });

  /** Row data for the table */
  rowData: any[] = [];

  ngOnInit(): void {
    this.loadBillings();
  }

  /** Load unpaid billings from the API */
  private loadBillings(): void {
    const formValue = this.searchForm.value;
    const searchRequest: BillingSearchRequest = {
      providerName: formValue.providerName || undefined,
      accountNumber: formValue.accountNumber || undefined,
      billingStatusId: formValue.billingStatusId || undefined,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : undefined,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : undefined,
      isPaid: false,
      pageNumber: 1,
      pageSize: 30
    };

    this.billingService.search(searchRequest).subscribe({
      next: (result) => {
        this.rowData = result.items;
      },
      error: () => {
        this.rowData = [];
      }
    });
  }

  onSearch(): void {
    this.loadBillings();
  }

  /** Multi-select: toggle all rows */
  get allSelected(): boolean {
    return this.rowData.length > 0 && this.selectedBillings.length === this.rowData.length;
  }

  get someSelected(): boolean {
    return this.selectedBillings.length > 0;
  }

  onToggleAll(checked: boolean): void {
    this.selectedBillings = checked ? [...this.rowData] : [];
  }

  onToggleRow(row: any, checked: boolean): void {
    if (checked) {
      this.selectedBillings = [...this.selectedBillings, row];
    } else {
      this.selectedBillings = this.selectedBillings.filter(b => b !== row);
    }
  }

  isRowSelected(row: any): boolean {
    return this.selectedBillings.includes(row);
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
        this.loadBillings();
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

  /** Create a remittance batch from the selected billings */
  onCreateRemittance(): void {
    if (this.selectedBillings.length === 0) return;

    const remittanceNumber = prompt('Enter remittance number:');
    if (!remittanceNumber) return;

    const request: RemittanceUpdateRequest = {
      billingIds: this.selectedBillings.map(b => b.id),
      remittanceNumber
    };

    this.billingService.updateRemittance(request).subscribe({
      next: () => {
        this.snackBar.open(
          `Remittance "${remittanceNumber}" assigned to ${this.selectedBillings.length} billing(s)`,
          'Close',
          { duration: 5000 }
        );
        this.selectedBillings = [];
        this.loadBillings();
      },
      error: () => {
        this.snackBar.open('Failed to create remittance', 'Close', { duration: 3000 });
      }
    });
  }

  /** Navigate to import status functionality */
  onImportStatus(): void {
    this.router.navigate(['/admin/imports']);
  }
}
