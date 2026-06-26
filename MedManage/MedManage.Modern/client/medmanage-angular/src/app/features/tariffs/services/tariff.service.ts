import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  BaseTariffDto, CreateBaseTariffRequest, UpdateBaseTariffRequest,
  TariffRateDto, CreateTariffRateRequest, UpdateTariffRateRequest,
  TariffNameDto, CreateTariffNameRequest, UpdateTariffNameRequest,
  TariffLookupResult, TariffCalculationResult,
  ProviderTariffAssignmentDto, CreateProviderTariffAssignmentRequest,
  ProviderCustomTariffDto, CreateProviderCustomTariffRequest
} from '../models/tariff.models';

interface ApiResponse<T> { success: boolean; data: T; message?: string; }

@Injectable({
  providedIn: 'root'
})
export class TariffService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/tariff`;
  private readonly providerUrl = `${environment.apiUrl}/service-providers`;

  // ─── Base Tariff CRUD ────────────────────────────────────────

  getBaseTariffs(): Observable<BaseTariffDto[]> {
    return this.http.get<ApiResponse<BaseTariffDto[]>>(`${this.baseUrl}/base`)
      .pipe(map(r => r.data));
  }

  getBaseTariffById(id: number): Observable<BaseTariffDto> {
    return this.http.get<ApiResponse<BaseTariffDto>>(`${this.baseUrl}/base/${id}`)
      .pipe(map(r => r.data));
  }

  createBaseTariff(request: CreateBaseTariffRequest): Observable<BaseTariffDto> {
    return this.http.post<ApiResponse<BaseTariffDto>>(`${this.baseUrl}/base`, request)
      .pipe(map(r => r.data));
  }

  updateBaseTariff(id: number, request: UpdateBaseTariffRequest): Observable<BaseTariffDto> {
    return this.http.put<ApiResponse<BaseTariffDto>>(`${this.baseUrl}/base/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteBaseTariff(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/base/${id}`);
  }

  // ─── Tariff Rates CRUD ───────────────────────────────────────

  getRates(): Observable<TariffRateDto[]> {
    return this.http.get<ApiResponse<TariffRateDto[]>>(`${this.baseUrl}/rates`)
      .pipe(map(r => r.data));
  }

  getRatesByTariffId(tariffId: number): Observable<TariffRateDto[]> {
    const params = new HttpParams().set('tariffId', tariffId.toString());
    return this.http.get<ApiResponse<TariffRateDto[]>>(`${this.baseUrl}/rates`, { params })
      .pipe(map(r => r.data));
  }

  createRate(request: CreateTariffRateRequest): Observable<TariffRateDto> {
    return this.http.post<ApiResponse<TariffRateDto>>(`${this.baseUrl}/rates`, request)
      .pipe(map(r => r.data));
  }

  updateRate(id: number, request: UpdateTariffRateRequest): Observable<TariffRateDto> {
    return this.http.put<ApiResponse<TariffRateDto>>(`${this.baseUrl}/rates/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteRate(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/rates/${id}`);
  }

  // ─── Tariff Names CRUD ───────────────────────────────────────

  getNames(): Observable<TariffNameDto[]> {
    return this.http.get<ApiResponse<TariffNameDto[]>>(`${this.baseUrl}/names`)
      .pipe(map(r => r.data));
  }

  getNamesByTariffId(tariffId: number): Observable<TariffNameDto[]> {
    const params = new HttpParams().set('tariffId', tariffId.toString());
    return this.http.get<ApiResponse<TariffNameDto[]>>(`${this.baseUrl}/names`, { params })
      .pipe(map(r => r.data));
  }

  createName(request: CreateTariffNameRequest): Observable<TariffNameDto> {
    return this.http.post<ApiResponse<TariffNameDto>>(`${this.baseUrl}/names`, request)
      .pipe(map(r => r.data));
  }

  updateName(id: number, request: UpdateTariffNameRequest): Observable<TariffNameDto> {
    return this.http.put<ApiResponse<TariffNameDto>>(`${this.baseUrl}/names/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteName(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/names/${id}`);
  }

  // ─── Tariff Lookup ───────────────────────────────────────────

  lookup(query: string): Observable<TariffLookupResult[]> {
    const params = new HttpParams().set('q', query);
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/search`, { params })
      .pipe(map(r => (r.data || []).map((item: any) => {
        // BaseTariffId format: "14_0109" — extract code after underscore
        const rawCode = item.baseTariffId || item.code || '';
        const code = rawCode.includes('_') ? rawCode.substring(rawCode.indexOf('_') + 1) : rawCode;
        return {
          id: item.tariffId || item.id || 0,
          code: code,
          description: item.tariffDescription || item.description || '',
          category: item.category || null,
          currentRate: item.currentRate || null,
          baseTariffId: rawCode
        };
      })));
  }

  // ─── Tariff Calculation ──────────────────────────────────────

  calculate(caseId: number): Observable<TariffCalculationResult> {
    return this.http.get<ApiResponse<TariffCalculationResult>>(`${this.baseUrl}/calculate/${caseId}`)
      .pipe(map(r => r.data));
  }

  // ─── Provider Tariff Assignments ─────────────────────────────

  getProviderTariffs(providerId: number): Observable<ProviderTariffAssignmentDto[]> {
    return this.http.get<ApiResponse<ProviderTariffAssignmentDto[]>>(`${this.providerUrl}/${providerId}/tariffs`)
      .pipe(map(r => r.data));
  }

  createProviderTariff(providerId: number, request: CreateProviderTariffAssignmentRequest): Observable<ProviderTariffAssignmentDto> {
    return this.http.post<ApiResponse<ProviderTariffAssignmentDto>>(`${this.providerUrl}/${providerId}/tariffs`, request)
      .pipe(map(r => r.data));
  }

  updateProviderTariff(providerId: number, id: number, request: CreateProviderTariffAssignmentRequest): Observable<ProviderTariffAssignmentDto> {
    return this.http.put<ApiResponse<ProviderTariffAssignmentDto>>(`${this.providerUrl}/${providerId}/tariffs/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteProviderTariff(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.providerUrl}/${providerId}/tariffs/${id}`);
  }

  // ─── Provider Custom Tariffs ─────────────────────────────────

  getProviderCustomTariffs(providerId: number): Observable<ProviderCustomTariffDto[]> {
    return this.http.get<ApiResponse<ProviderCustomTariffDto[]>>(`${this.providerUrl}/${providerId}/custom-tariffs`)
      .pipe(map(r => r.data));
  }

  createProviderCustomTariff(providerId: number, request: CreateProviderCustomTariffRequest): Observable<ProviderCustomTariffDto> {
    return this.http.post<ApiResponse<ProviderCustomTariffDto>>(`${this.providerUrl}/${providerId}/custom-tariffs`, request)
      .pipe(map(r => r.data));
  }

  updateProviderCustomTariff(providerId: number, id: number, request: CreateProviderCustomTariffRequest): Observable<ProviderCustomTariffDto> {
    return this.http.put<ApiResponse<ProviderCustomTariffDto>>(`${this.providerUrl}/${providerId}/custom-tariffs/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteProviderCustomTariff(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.providerUrl}/${providerId}/custom-tariffs/${id}`);
  }
}
