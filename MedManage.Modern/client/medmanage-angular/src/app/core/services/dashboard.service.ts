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

  getStats(mainClientId?: number | null): Observable<DashboardStats> {
    let url = `${this.API_URL}/stats`;
    if (mainClientId) {
      url += `?mainClientId=${mainClientId}`;
    }
    return this.http.get<ApiResponse<DashboardStats>>(url).pipe(
      map(response => response.data)
    );
  }
}
