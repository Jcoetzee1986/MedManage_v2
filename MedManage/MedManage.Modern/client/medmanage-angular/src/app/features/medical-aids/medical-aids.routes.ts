import { Routes } from '@angular/router';

export const MEDICAL_AID_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./medical-aid-admin/medical-aid-admin.component').then(m => m.MedicalAidAdminComponent)
  }
];
