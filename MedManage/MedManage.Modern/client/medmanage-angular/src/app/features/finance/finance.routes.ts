import { Routes } from '@angular/router';
import { roleGuard } from '../../core/guards/role.guard';

export const FINANCE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./billing-list/billing-list.component').then(m => m.BillingListComponent),
    canActivate: [roleGuard('Admin', 'BillingOfficer', 'CaseManager')]
  },
  {
    path: 'billing/new',
    loadComponent: () => import('./billing-form/billing-form.component').then(m => m.BillingFormComponent),
    canActivate: [roleGuard('Admin', 'BillingOfficer')]
  },
  {
    path: 'billing/:id',
    loadComponent: () => import('./billing-form/billing-form.component').then(m => m.BillingFormComponent),
    canActivate: [roleGuard('Admin', 'BillingOfficer')]
  },
  {
    path: 'payments',
    loadComponent: () => import('./bulk-payment/bulk-payment.component').then(m => m.BulkPaymentComponent),
    canActivate: [roleGuard('Admin', 'BillingOfficer')]
  },
  {
    path: 'remittance',
    loadComponent: () => import('./remittance/remittance.component').then(m => m.RemittanceComponent),
    canActivate: [roleGuard('Admin', 'BillingOfficer')]
  }
];
