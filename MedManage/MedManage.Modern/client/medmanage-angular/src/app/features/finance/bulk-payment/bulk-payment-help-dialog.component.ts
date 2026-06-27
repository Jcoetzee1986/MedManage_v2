import { Component, inject } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-bulk-payment-help-dialog',
  standalone: true,
  imports: [MatDialogModule, MatButtonModule, MatIconModule],
  template: `
    <h2 mat-dialog-title>How Bulk Payments & Import/Export Works</h2>
    <mat-dialog-content>
      <h3>Workflow Overview</h3>
      <ol>
        <li><strong>Search</strong> — Filter unpaid billings by provider, account, remittance, date, or status.</li>
        <li><strong>Create Remittance</strong> — Select billings, click "Create Remittance", enter a number. This:
          <ul>
            <li>Assigns the remittance number to all selected billings</li>
            <li>Sets their status to <strong>Submitted</strong></li>
            <li>Auto-downloads a CSV file with the selected billings</li>
          </ul>
        </li>
        <li><strong>Export CSV</strong> — Downloads the current filtered results as a CSV file you can open in Excel.</li>
        <li><strong>Edit in Excel</strong> — Update the following columns:
          <ul>
            <li><strong>Paid</strong> — set to "true" to mark as paid</li>
            <li><strong>DatePaid</strong> — the payment date (e.g., 2026-06-27)</li>
            <li><strong>RemittanceNumber</strong> — assign or update remittance reference</li>
            <li><strong>Discount</strong> — discount amount</li>
            <li><strong>Penalty</strong> — penalty amount</li>
            <li><strong>RejectedAmount</strong> — rejected amount</li>
            <li><strong>FinalInvoiceAmount</strong> — final invoice amount after adjustments</li>
          </ul>
        </li>
        <li><strong>Import Status</strong> — Upload the edited CSV. The system:
          <ul>
            <li>Updates all editable fields per billing ID</li>
            <li>Does <strong>NOT</strong> change the Amount Due (original invoice amount is preserved)</li>
            <li>If <strong>Paid = true</strong> → status changes to <strong>Paid</strong></li>
            <li>If a <strong>Remittance Number is added</strong> where there wasn't one → status changes to <strong>Submitted</strong></li>
          </ul>
        </li>
      </ol>

      <h3>Status Transitions</h3>
      <table style="width: 100%; border-collapse: collapse; margin-top: 8px;">
        <tr style="background: #f5f5f5;"><th style="padding: 6px; text-align: left;">Action</th><th style="padding: 6px; text-align: left;">Status</th></tr>
        <tr><td style="padding: 6px;">New billing created</td><td style="padding: 6px;"><strong>New</strong> (1)</td></tr>
        <tr><td style="padding: 6px;">Remittance assigned (Create Remittance or Import)</td><td style="padding: 6px;"><strong>Submitted</strong> (2)</td></tr>
        <tr><td style="padding: 6px;">Paid = true (Bulk Payment or Import)</td><td style="padding: 6px;"><strong>Paid</strong> (3)</td></tr>
      </table>

      <h3>CSV Column Reference</h3>
      <p style="font-size: 13px; color: #666;">
        BillingId, AccountNumber, InvoiceNumber, CaseNumber, MemberName, ProviderName,
        RemittanceNumber, AmountDue, Discount, Penalty, RejectedAmount, FinalInvoiceAmount,
        DateReceived, Paid, DatePaid
      </p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-raised-button color="primary" (click)="dialogRef.close()">Got it</button>
    </mat-dialog-actions>
  `
})
export class BulkPaymentHelpDialogComponent {
  readonly dialogRef = inject(MatDialogRef<BulkPaymentHelpDialogComponent>);
}
