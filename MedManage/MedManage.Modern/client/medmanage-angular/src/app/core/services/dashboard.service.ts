import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface DashboardStats {
  totalCases: number;
  totalMembers: number;
  pendingBillingCount: number;
  activeCases: number;
  recentCases: number;
}

interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
  errors: string[];
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private readonly http = inject(HttpClient);
  private readonly API_URL = `${environment.apiUrl}/dashboard`;

  getStats(): Observable<DashboardStats> {
    return this.http.get<ApiResponse<DashboardStats>>(`${this.API_URL}/stats`).pipe(
      map(response => response.data)
    );
  }
}
