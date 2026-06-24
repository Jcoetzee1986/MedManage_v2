import { Routes } from '@angular/router';

export const CASE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./case-list/case-list.component').then(m => m.CaseListComponent)
  },
  {
    path: 'my-cases',
    loadComponent: () => import('./my-cases/my-cases.component').then(m => m.MyCasesComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./case-form/case-form.component').then(m => m.CaseFormComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./case-detail/case-detail.component').then(m => m.CaseDetailComponent)
  }
];
