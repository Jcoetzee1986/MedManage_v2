import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ReferenceDataDropdownComponent } from '../../../shared/components/reference-data-dropdown/reference-data-dropdown.component';
import { BillingCommentsComponent } from '../billing-comments/billing-comments.component';
import { BillingService } from '../services/billing.service';
import { CaseBillingDto, CreateBillingRequest, UpdateBillingRequest } from '../models/billing.models';

@Component({
  selector: 'app-billing-form',
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
    MatSnackBarModule,
    MatProgressSpinnerModule,
    ReferenceDataDropdownComponent,
    BillingCommentsComponent
  ],
  templateUrl: './billing-form.component.html',
  styleUrls: ['./billing-form.component.scss']
})
export class BillingFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly billingService = inject(BillingService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly snackBar = inject(MatSnackBar);

  isEditMode = false;
  billingId: number | null = null;
  loading = false;

  form = this.fb.group({
    caseId: [null as number | null, Validators.required],
    accountNumber: [''],
    invoiceNumber: [''],
    billingStatusId: [null as number | null],
    dateReceived: [null as Date | null],
    dateSubmitted: [null as Date | null],
    amount: [null as number | null],
    finalInvoiceAmount: [null as number | null],
    comments: ['']
  });

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam && idParam !== 'new') {
      this.isEditMode = true;
      this.billingId = +idParam;
      this.loadBilling(this.billingId);
    }
  }

  private loadBilling(id: number): void {
    this.loading = true;
    this.billingService.getById(id).subscribe({
      next: (billing) => {
        this.form.patchValue({
          caseId: billing.caseId,
          accountNumber: billing.accountNumber || '',
          invoiceNumber: billing.invoiceNumber || '',
          billingStatusId: billing.billingStatusId || null,
          dateReceived: billing.dateReceived ? new Date(billing.dateReceived) : null,
          dateSubmitted: billing.dateSubmitted ? new Date(billing.dateSubmitted) : null,
          amount: billing.amount || null,
          finalInvoiceAmount: billing.finalInvoiceAmount || null,
          comments: billing.comments || ''
        });
        this.loading = false;
      },
      error: () => {
        this.snackBar.open('Failed to load billing record', 'Close', { duration: 3000 });
        this.loading = false;
        this.router.navigate(['/finance']);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    const formVal = this.form.value;

    if (this.isEditMode && this.billingId) {
      const request: UpdateBillingRequest = {
        id: this.billingId,
        caseId: formVal.caseId!,
        accountNumber: formVal.accountNumber || undefined,
        invoiceNumber: formVal.invoiceNumber || undefined,
        billingStatusId: formVal.billingStatusId || undefined,
        dateReceived: formVal.dateReceived?.toISOString(),
        dateSubmitted: formVal.dateSubmitted?.toISOString(),
        amount: formVal.amount || undefined,
        finalInvoiceAmount: formVal.finalInvoiceAmount || undefined,
        comments: formVal.comments || undefined
      };

      this.billingService.update(this.billingId, request).subscribe({
        next: () => {
          this.snackBar.open('Billing record updated successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/finance']);
        },
        error: () => {
          this.snackBar.open('Failed to update billing record', 'Close', { duration: 3000 });
        }
      });
    } else {
      const request: CreateBillingRequest = {
        caseId: formVal.caseId!,
        accountNumber: formVal.accountNumber || undefined,
        invoiceNumber: formVal.invoiceNumber || undefined,
        billingStatusId: formVal.billingStatusId || undefined,
        dateReceived: formVal.dateReceived?.toISOString(),
        dateSubmitted: formVal.dateSubmitted?.toISOString(),
        amount: formVal.amount || undefined,
        finalInvoiceAmount: formVal.finalInvoiceAmount || undefined,
        comments: formVal.comments || undefined
      };

      this.billingService.create(request).subscribe({
        next: (newBilling) => {
          this.snackBar.open('Billing record created successfully', 'Close', { duration: 3000 });
          this.router.navigate(['/finance/billing', newBilling.id]);
        },
        error: () => {
          this.snackBar.open('Failed to create billing record', 'Close', { duration: 3000 });
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/finance']);
  }

  onDelete(): void {
    if (!this.billingId) return;

    if (confirm('Are you sure you want to delete this billing record?')) {
      this.billingService.delete(this.billingId).subscribe({
        next: () => {
          this.snackBar.open('Billing record deleted', 'Close', { duration: 3000 });
          this.router.navigate(['/finance']);
        },
        error: () => {
          this.snackBar.open('Failed to delete billing record', 'Close', { duration: 3000 });
        }
      });
    }
  }
}
