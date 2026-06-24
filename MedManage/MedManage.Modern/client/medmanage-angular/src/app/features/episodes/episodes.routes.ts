import { Routes } from '@angular/router';

export const EPISODE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./episode-list/episode-list.component').then(m => m.EpisodeListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./episode-detail/episode-detail.component').then(m => m.EpisodeDetailComponent)
  },
  {
    path: ':id',
    loadComponent: () => import('./episode-detail/episode-detail.component').then(m => m.EpisodeDetailComponent)
  }
];
