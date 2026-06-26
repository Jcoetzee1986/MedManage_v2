import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  ProviderDto, CreateProviderRequest, UpdateProviderRequest, ProviderSearchRequest, PagedResult,
  ProviderAutocompleteResult,
  ProviderTariffDto, CreateProviderTariffRequest,
  ProviderCustomTariffDto, CreateProviderCustomTariffRequest,
  ProviderDiscountDto, CreateProviderDiscountRequest
} from '../models/provider.models';

interface ApiResponse<T> { success: boolean; data: T; message?: string; }

@Injectable({
  providedIn: 'root'
})
export class ProviderService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/service-providers`;

  // ─── Provider CRUD ───────────────────────────────────────────

  search(request: ProviderSearchRequest): Observable<PagedResult<ProviderDto>> {
    return this.http.post<ApiResponse<PagedResult<ProviderDto>>>(`${this.baseUrl}/search`, request)
      .pipe(map(r => r.data));
  }

  getAll(): Observable<ProviderDto[]> {
    return this.http.get<ApiResponse<ProviderDto[]>>(this.baseUrl)
      .pipe(map(r => r.data));
  }

  getById(id: number): Observable<ProviderDto> {
    return this.http.get<ApiResponse<ProviderDto>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  create(request: CreateProviderRequest): Observable<ProviderDto> {
    return this.http.post<ApiResponse<ProviderDto>>(this.baseUrl, request)
      .pipe(map(r => r.data));
  }

  update(id: number, request: UpdateProviderRequest): Observable<ProviderDto> {
    return this.http.put<ApiResponse<ProviderDto>>(`${this.baseUrl}/${id}`, request)
      .pipe(map(r => r.data));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  autocomplete(query: string): Observable<ProviderAutocompleteResult[]> {
    const params = new HttpParams().set('query', query);
    return this.http.get<ApiResponse<ProviderAutocompleteResult[]>>(`${this.baseUrl}/autocomplete`, { params })
      .pipe(map(r => r.data));
  }

  // ─── Tariff Assignments ──────────────────────────────────────

  getTariffs(providerId: number): Observable<ProviderTariffDto[]> {
    return this.http.get<ApiResponse<ProviderTariffDto[]>>(`${this.baseUrl}/${providerId}/tariffs`)
      .pipe(map(r => r.data));
  }

  createTariff(providerId: number, request: CreateProviderTariffRequest): Observable<ProviderTariffDto> {
    return this.http.post<ApiResponse<ProviderTariffDto>>(`${this.baseUrl}/${providerId}/tariffs`, request)
      .pipe(map(r => r.data));
  }

  updateTariff(providerId: number, id: number, request: CreateProviderTariffRequest): Observable<ProviderTariffDto> {
    return this.http.put<ApiResponse<ProviderTariffDto>>(`${this.baseUrl}/${providerId}/tariffs/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteTariff(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${providerId}/tariffs/${id}`);
  }

  // ─── Custom Tariffs ──────────────────────────────────────────

  getCustomTariffs(providerId: number): Observable<ProviderCustomTariffDto[]> {
    return this.http.get<ApiResponse<ProviderCustomTariffDto[]>>(`${this.baseUrl}/${providerId}/custom-tariffs`)
      .pipe(map(r => r.data));
  }

  createCustomTariff(providerId: number, request: CreateProviderCustomTariffRequest): Observable<ProviderCustomTariffDto> {
    return this.http.post<ApiResponse<ProviderCustomTariffDto>>(`${this.baseUrl}/${providerId}/custom-tariffs`, request)
      .pipe(map(r => r.data));
  }

  updateCustomTariff(providerId: number, id: number, request: CreateProviderCustomTariffRequest): Observable<ProviderCustomTariffDto> {
    return this.http.put<ApiResponse<ProviderCustomTariffDto>>(`${this.baseUrl}/${providerId}/custom-tariffs/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteCustomTariff(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${providerId}/custom-tariffs/${id}`);
  }

  // ─── Discounts ───────────────────────────────────────────────

  getDiscounts(providerId: number): Observable<ProviderDiscountDto[]> {
    return this.http.get<ApiResponse<ProviderDiscountDto[]>>(`${this.baseUrl}/${providerId}/discounts`)
      .pipe(map(r => r.data));
  }

  createDiscount(providerId: number, request: CreateProviderDiscountRequest): Observable<ProviderDiscountDto> {
    return this.http.post<ApiResponse<ProviderDiscountDto>>(`${this.baseUrl}/${providerId}/discounts`, request)
      .pipe(map(r => r.data));
  }

  updateDiscount(providerId: number, id: number, request: CreateProviderDiscountRequest): Observable<ProviderDiscountDto> {
    return this.http.put<ApiResponse<ProviderDiscountDto>>(`${this.baseUrl}/${providerId}/discounts/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteDiscount(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${providerId}/discounts/${id}`);
  }
}
