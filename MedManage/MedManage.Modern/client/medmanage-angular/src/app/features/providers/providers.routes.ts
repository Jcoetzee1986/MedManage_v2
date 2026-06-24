import { Routes } from '@angular/router';

export const PROVIDER_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./provider-list/provider-list.component').then(m => m.ProviderListComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./provider-detail/provider-detail.component').then(m => m.ProviderDetailComponent)
  }
];
