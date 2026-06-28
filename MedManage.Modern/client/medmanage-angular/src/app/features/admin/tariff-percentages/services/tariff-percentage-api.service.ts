import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../../environments/environment';
import {
  TariffPercentage,
  CreateTariffPercentageRequest,
  UpdateTariffPercentageRequest,
  TariffUpdateJobStatus
} from '../models/tariff-percentage.models';

/** Generic API response wrapper */
export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
  errors?: string[];
}

/**
 * Service for tariff percentage management API operations.
 * Communicates with the /api/admin/tariff-percentages endpoints.
 */
@Injectable({
  providedIn: 'root'
})
export class TariffPercentageApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/admin/tariff-percentages`;

  /** Get all tariff percentages */
  getAll(): Observable<ApiResponse<TariffPercentage[]>> {
    return this.http.get<ApiResponse<TariffPercentage[]>>(this.baseUrl);
  }

  /** Get a tariff percentage by ID */
  getById(id: number): Observable<ApiResponse<TariffPercentage>> {
    return this.http.get<ApiResponse<TariffPercentage>>(`${this.baseUrl}/${id}`);
  }

  /** Create a new tariff percentage */
  create(dto: CreateTariffPercentageRequest): Observable<ApiResponse<TariffPercentage>> {
    return this.http.post<ApiResponse<TariffPercentage>>(this.baseUrl, dto);
  }

  /** Update an existing tariff percentage */
  update(id: number, dto: UpdateTariffPercentageRequest): Observable<ApiResponse<TariffPercentage>> {
    return this.http.put<ApiResponse<TariffPercentage>>(`${this.baseUrl}/${id}`, dto);
  }

  /** Soft-delete a tariff percentage */
  delete(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/${id}`);
  }

  /** Trigger propagation for a tariff percentage (returns 202 with job status) */
  apply(id: number): Observable<ApiResponse<TariffUpdateJobStatus>> {
    return this.http.post<ApiResponse<TariffUpdateJobStatus>>(
      `${this.baseUrl}/${id}/apply`, {});
  }

  /** Poll the status of a propagation job */
  getJobStatus(jobId: string): Observable<ApiResponse<TariffUpdateJobStatus>> {
    return this.http.get<ApiResponse<TariffUpdateJobStatus>>(
      `${this.baseUrl}/jobs/${jobId}`);
  }
}
