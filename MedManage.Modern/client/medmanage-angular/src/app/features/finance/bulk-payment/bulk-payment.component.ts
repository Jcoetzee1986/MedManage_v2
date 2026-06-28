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
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { BillingService } from '../services/billing.service';
import { CaseBillingDto, BillingSearchRequest, BulkPaymentRequest, RemittanceUpdateRequest } from '../models/billing.models';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { BulkPaymentHelpDialogComponent } from './bulk-payment-help-dialog.component';

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
    MatDialogModule,
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
  private readonly dialog = inject(MatDialog);

  selectedBillings: CaseBillingDto[] = [];
  processing = false;

  /** Columns for the mat-table */
  displayedColumns = ['select', 'accountNumber', 'invoiceNumber', 'caseNumber', 'memberName', 'providerName', 'remittanceNumber', 'amount', 'discount', 'penalty', 'rejectedAmount', 'finalInvoiceAmount', 'dateReceived', 'datePaid'];

  /** Search form for finding billings to pay */
  searchForm = this.fb.group({
    providerName: [''],
    accountNumber: [''],
    remittanceNumber: [''],
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
      remittance: formValue.remittanceNumber || undefined,
      billingStatusId: formValue.billingStatusId || undefined,
      dateFrom: formValue.dateFrom ? formValue.dateFrom.toISOString() : undefined,
      dateTo: formValue.dateTo ? formValue.dateTo.toISOString() : undefined,
      isPaid: false,
      pageNumber: 1,
      pageSize: 200
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
        // Auto-download CSV for the remittance batch
        this.exportRemittanceCsv(remittanceNumber);
        this.selectedBillings = [];
        this.loadBillings();
      },
      error: () => {
        this.snackBar.open('Failed to create remittance', 'Close', { duration: 3000 });
      }
    });
  }

  /** Export a CSV specifically for the remittance batch that was just created */
  private exportRemittanceCsv(remittanceNumber: string): void {
    const data = this.selectedBillings;
    const headers = ['BillingId', 'AccountNumber', 'InvoiceNumber', 'CaseNumber', 'MemberName', 'ProviderName', 'RemittanceNumber', 'AmountDue', 'Discount', 'Penalty', 'RejectedAmount', 'FinalInvoiceAmount', 'DateReceived', 'Paid', 'DatePaid'];
    const rows = data.map((r: any) => [
      r.id, r.accountNumber, r.invoiceNumber, r.caseNumber, r.memberName, r.providerName,
      remittanceNumber, r.amount || 0, r.discount || 0, r.penalty || 0,
      r.rejectedAmount || 0, r.finalInvoiceAmount || 0, r.dateReceived || '',
      false, ''
    ]);

    const csv = ['sep=,', headers.join(','), ...rows.map(row => row.map((v: any) => `"${(v ?? '').toString().replace(/"/g, '""')}"`).join(','))].join('\r\n');
    const bom = '\uFEFF';
    const blob = new Blob([bom + csv], { type: 'text/csv;charset=utf-8' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `remittance-${remittanceNumber}-${new Date().toISOString().split('T')[0]}.csv`;
    a.click();
    window.URL.revokeObjectURL(url);
  }

  /** Export current filtered results as CSV for editing */
  onExportCsv(): void {
    const data = this.rowData.length > 0 ? this.rowData : [];
    if (data.length === 0) {
      this.snackBar.open('No data to export. Run a search first.', 'Close', { duration: 3000 });
      return;
    }

    const headers = ['BillingId', 'AccountNumber', 'InvoiceNumber', 'CaseNumber', 'MemberName', 'ProviderName', 'RemittanceNumber', 'AmountDue', 'Discount', 'Penalty', 'RejectedAmount', 'FinalInvoiceAmount', 'DateReceived', 'Paid', 'DatePaid'];
    const rows = data.map((r: any) => [
      r.id, r.accountNumber, r.invoiceNumber, r.caseNumber, r.memberName, r.providerName,
      r.remittanceNumber || '', r.amount || 0, r.discount || 0, r.penalty || 0,
      r.rejectedAmount || 0, r.finalInvoiceAmount || 0, r.dateReceived || '',
      r.paid || false, r.datePaid || ''
    ]);

    const csv = ['sep=,', headers.join(','), ...rows.map(row => row.map((v: any) => `"${(v ?? '').toString().replace(/"/g, '""')}"`).join(','))].join('\r\n');
    const bom = '\uFEFF';
    const blob = new Blob([bom + csv], { type: 'text/csv;charset=utf-8' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `billing-export-${new Date().toISOString().split('T')[0]}.csv`;
    a.click();
    window.URL.revokeObjectURL(url);
  }

  /** Import status updates from CSV file */
  onImportStatus(): void {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = '.csv';
    input.onchange = (event: any) => {
      const file = event.target.files[0];
      if (!file) return;

      const reader = new FileReader();
      reader.onload = (e: any) => {
        const text = e.target.result as string;
        this.processImportCsv(text);
      };
      reader.readAsText(file);
    };
    input.click();
  }

  private processImportCsv(csvText: string): void {
    // Strip BOM if present
    let text = csvText.replace(/^\uFEFF/, '');
    let lines = text.split(/\r?\n/).filter(l => l.trim());
    if (lines.length < 2) {
      this.snackBar.open('CSV file is empty or has no data rows', 'Close', { duration: 3000 });
      return;
    }

    // Skip the sep= directive if present
    if (lines[0].toLowerCase().startsWith('sep=')) {
      lines = lines.slice(1);
    }

    // Detect delimiter: try comma first, fall back to semicolon, then tab
    let delimiter = ',';
    const firstDataLine = lines[0];
    if (firstDataLine.split(';').length > firstDataLine.split(',').length) {
      delimiter = ';';
    } else if (firstDataLine.split('\t').length > firstDataLine.split(',').length) {
      delimiter = '\t';
    }

    const headers = lines[0].split(delimiter).map(h => h.replace(/"/g, '').trim().toLowerCase());
    const billingIdIdx = headers.indexOf('billingid');
    const paidIdx = headers.indexOf('paid');
    const datePaidIdx = headers.indexOf('datepaid');
    const remittanceIdx = headers.indexOf('remittancenumber');
    const discountIdx = headers.indexOf('discount');
    const penaltyIdx = headers.indexOf('penalty');
    const rejectedIdx = headers.indexOf('rejectedamount');
    const finalInvoiceIdx = headers.indexOf('finalinvoiceamount');

    if (billingIdIdx === -1) {
      this.snackBar.open('CSV must have a "BillingId" column', 'Close', { duration: 3000 });
      return;
    }

    const updates: { id: number; paid: boolean; datePaid?: string; remittanceNumber?: string; discount?: number; penalty?: number; rejectedAmount?: number; finalInvoiceAmount?: number }[] = [];

    for (let i = 1; i < lines.length; i++) {
      const cols = lines[i].split(delimiter).map(c => c.replace(/"/g, '').trim());
      const id = parseInt(cols[billingIdIdx], 10);
      if (isNaN(id)) continue;

      updates.push({
        id,
        paid: paidIdx >= 0 ? cols[paidIdx]?.toLowerCase() === 'true' : false,
        datePaid: datePaidIdx >= 0 ? cols[datePaidIdx] || undefined : undefined,
        remittanceNumber: remittanceIdx >= 0 ? cols[remittanceIdx] || undefined : undefined,
        discount: discountIdx >= 0 && cols[discountIdx] ? parseFloat(cols[discountIdx]) || undefined : undefined,
        penalty: penaltyIdx >= 0 && cols[penaltyIdx] ? parseFloat(cols[penaltyIdx]) || undefined : undefined,
        rejectedAmount: rejectedIdx >= 0 && cols[rejectedIdx] ? parseFloat(cols[rejectedIdx]) || undefined : undefined,
        finalInvoiceAmount: finalInvoiceIdx >= 0 && cols[finalInvoiceIdx] ? parseFloat(cols[finalInvoiceIdx]) || undefined : undefined
      });
    }

    if (updates.length === 0) {
      this.snackBar.open('No valid rows found in CSV', 'Close', { duration: 3000 });
      return;
    }

    if (!confirm(`Import ${updates.length} billing status update(s)?`)) return;

    this.billingService.importStatusUpdates(updates).subscribe({
      next: (result) => {
        this.snackBar.open(result.message || `Imported ${result.successCount} update(s)`, 'Close', { duration: 5000 });
        this.loadBillings();
      },
      error: () => this.snackBar.open('Failed to import status updates', 'Close', { duration: 3000 })
    });
  }

  onShowHelp(): void {
    this.dialog.open(BulkPaymentHelpDialogComponent, { width: '650px' });
  }
}
