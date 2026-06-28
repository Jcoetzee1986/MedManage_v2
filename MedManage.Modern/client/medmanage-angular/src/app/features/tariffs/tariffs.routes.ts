import { Routes } from '@angular/router';

export const TARIFF_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./tariff-admin/tariff-admin.component').then(m => m.TariffAdminComponent)
  },
  {
    path: 'provider/:providerId',
    loadComponent: () => import('./provider-tariff-customization/provider-tariff-customization.component').then(m => m.ProviderTariffCustomizationComponent)
  }
];
