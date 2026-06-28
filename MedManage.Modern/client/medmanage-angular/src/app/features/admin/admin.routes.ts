import { Routes } from '@angular/router';
import { roleGuard } from '../../core/guards/role.guard';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./admin-layout.component').then(m => m.AdminLayoutComponent),
    children: [
      {
        path: '',
        redirectTo: 'users',
        pathMatch: 'full'
      },
      {
        path: 'users',
        loadComponent: () =>
          import('./user-management/user-management.component').then(
            m => m.UserManagementComponent
          ),
        title: 'User Management'
      },
      {
        path: 'locks',
        loadComponent: () =>
          import('./case-locks/case-locks.component').then(
            m => m.CaseLocksComponent
          ),
        title: 'Case Locks'
      },
      {
        path: 'reference-data',
        loadComponent: () =>
          import('./reference-data/reference-data-admin.component').then(
            m => m.ReferenceDataAdminComponent
          ),
        title: 'Reference Data Administration'
      },
      {
        path: 'system-config',
        loadComponent: () =>
          import('./system-config/system-config.component').then(
            m => m.SystemConfigComponent
          ),
        title: 'System Configuration'
      },
      {
        path: 'imports',
        loadComponent: () =>
          import('./imports/imports.component').then(
            m => m.ImportsComponent
          ),
        title: 'Data Import'
      },
      {
        path: 'tariff-percentages',
        loadComponent: () =>
          import('./tariff-percentages/tariff-percentage-management.component').then(
            m => m.TariffPercentageManagementComponent
          ),
        canActivate: [roleGuard('System Administrator')],
        title: 'Tariff Percentages'
      }
    ]
  }
];
