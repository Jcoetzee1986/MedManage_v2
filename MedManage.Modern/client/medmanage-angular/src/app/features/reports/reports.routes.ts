import { Routes } from '@angular/router';

export const REPORT_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./reports-page/reports-page.component').then(m => m.ReportsPageComponent)
  }
];
