import { Routes } from '@angular/router';
import { roleGuard } from '../../core/guards/role.guard';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    redirectTo: 'reference-data',
    pathMatch: 'full'
  },
  {
    path: 'reference-data',
    loadComponent: () =>
      import('./reference-data/reference-data-admin.component').then(
        m => m.ReferenceDataAdminComponent
      ),
    canActivate: [roleGuard('Admin')],
    title: 'Reference Data Administration'
  },
  {
    path: 'system-config',
    loadComponent: () =>
      import('./system-config/system-config.component').then(
        m => m.SystemConfigComponent
      ),
    canActivate: [roleGuard('Admin')],
    title: 'System Configuration'
  },
  {
    path: 'users',
    loadComponent: () =>
      import('./user-management/user-management.component').then(
        m => m.UserManagementComponent
      ),
    canActivate: [roleGuard('Admin')],
    title: 'User Management'
  },
  {
    path: 'imports',
    loadComponent: () =>
      import('./imports/imports.component').then(
        m => m.ImportsComponent
      ),
    canActivate: [roleGuard('Admin')],
    title: 'Data Import'
  }
];
