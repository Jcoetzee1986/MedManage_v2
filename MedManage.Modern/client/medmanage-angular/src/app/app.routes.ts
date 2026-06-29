import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  },
  // Authentication Routes
  {
    path: 'login',
    title: 'Login - MedManage',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    title: 'Register - MedManage',
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'forgot-password',
    title: 'Forgot Password - MedManage',
    loadComponent: () => import('./features/auth/forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent)
  },
  {
    path: 'reset-password',
    title: 'Reset Password - MedManage',
    loadComponent: () => import('./features/auth/reset-password/reset-password.component').then(m => m.ResetPasswordComponent)
  },
  // Protected Routes
  {
    path: 'dashboard',
    title: 'Dashboard - MedManage',
    loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
    canActivate: [authGuard]
  },
  {
    path: 'cases',
    title: 'Cases - MedManage',
    loadChildren: () => import('./features/case-management/case-management.routes').then(m => m.CASE_ROUTES),
    canActivate: [authGuard]
  },
  {
    path: 'finance',
    title: 'Finance - MedManage',
    loadChildren: () => import('./features/finance/finance.routes').then(m => m.FINANCE_ROUTES),
    canActivate: [roleGuard('System Administrator', 'Billing Auditing', 'Case Manager')]
  },
  {
    path: 'members',
    title: 'Members - MedManage',
    loadChildren: () => import('./features/members/members.routes').then(m => m.MEMBER_ROUTES),
    canActivate: [authGuard]
  },
  {
    path: 'providers',
    title: 'Providers - MedManage',
    loadChildren: () => import('./features/providers/providers.routes').then(m => m.PROVIDER_ROUTES),
    canActivate: [authGuard]
  },
  {
    path: 'tariffs',
    title: 'Tariffs - MedManage',
    loadChildren: () => import('./features/tariffs/tariffs.routes').then(m => m.TARIFF_ROUTES),
    canActivate: [authGuard]
  },
  {
    path: 'bookings',
    title: 'Bookings - MedManage',
    loadChildren: () => import('./features/bookings/bookings.routes').then(m => m.BOOKING_ROUTES),
    canActivate: [authGuard]
  },
  {
    path: 'admin',
    title: 'Admin - MedManage',
    loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES),
    canActivate: [roleGuard('System Administrator')]
  },
  {
    path: 'medical-aids',
    title: 'Medical Aids - MedManage',
    loadChildren: () => import('./features/medical-aids/medical-aids.routes').then(m => m.MEDICAL_AID_ROUTES),
    canActivate: [authGuard]
  },
  {
    path: 'episodes',
    title: 'Episodes - MedManage',
    loadChildren: () => import('./features/episodes/episodes.routes').then(m => m.EPISODE_ROUTES),
    canActivate: [authGuard]
  },
  {
    path: 'reports',
    title: 'Reports - MedManage',
    loadChildren: () => import('./features/reports/reports.routes').then(m => m.REPORT_ROUTES),
    canActivate: [authGuard]
  },
  // Wildcard Route
  {
    path: '**',
    redirectTo: '/dashboard'
  }
];
