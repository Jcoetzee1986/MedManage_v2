import { Routes } from '@angular/router';

export const BOOKING_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./booking-list/booking-list.component').then(m => m.BookingListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./booking-form/booking-form.component').then(m => m.BookingFormComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./booking-form/booking-form.component').then(m => m.BookingFormComponent)
  }
];
