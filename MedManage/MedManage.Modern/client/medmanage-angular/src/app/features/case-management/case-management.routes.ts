import { Routes } from '@angular/router';

export const CASE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./case-list/case-list.component').then(m => m.CaseListComponent)
  },
  {
    path: 'my-cases',
    redirectTo: '',
    pathMatch: 'full'
  },
  {
    path: 'new',
    loadComponent: () => import('./case-wizard/case-wizard.component').then(m => m.CaseWizardComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./case-detail/case-detail.component').then(m => m.CaseDetailComponent)
  }
];
