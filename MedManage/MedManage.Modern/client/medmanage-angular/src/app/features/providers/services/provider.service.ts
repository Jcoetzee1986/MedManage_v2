import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  ProviderDto, CreateProviderRequest, UpdateProviderRequest, ProviderSearchRequest, PagedResult,
  ProviderAutocompleteResult,
  ProviderTariffDto, CreateProviderTariffRequest,
  ProviderCustomTariffDto, CreateProviderCustomTariffRequest,
  ProviderDiscountDto, CreateProviderDiscountRequest
} from '../models/provider.models';

@Injectable({
  providedIn: 'root'
})
export class ProviderService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/service-providers`;

  // ─── Provider CRUD ───────────────────────────────────────────

  search(request: ProviderSearchRequest): Observable<PagedResult<ProviderDto>> {
    return this.http.post<PagedResult<ProviderDto>>(`${this.baseUrl}/search`, request);
  }

  getAll(): Observable<ProviderDto[]> {
    return this.http.get<ProviderDto[]>(this.baseUrl);
  }

  getById(id: number): Observable<ProviderDto> {
    return this.http.get<ProviderDto>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateProviderRequest): Observable<ProviderDto> {
    return this.http.post<ProviderDto>(this.baseUrl, request);
  }

  update(id: number, request: UpdateProviderRequest): Observable<ProviderDto> {
    return this.http.put<ProviderDto>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  autocomplete(query: string): Observable<ProviderAutocompleteResult[]> {
    const params = new HttpParams().set('query', query);
    return this.http.get<ProviderAutocompleteResult[]>(`${this.baseUrl}/autocomplete`, { params });
  }

  // ─── Tariff Assignments ──────────────────────────────────────

  getTariffs(providerId: number): Observable<ProviderTariffDto[]> {
    return this.http.get<ProviderTariffDto[]>(`${this.baseUrl}/${providerId}/tariffs`);
  }

  createTariff(providerId: number, request: CreateProviderTariffRequest): Observable<ProviderTariffDto> {
    return this.http.post<ProviderTariffDto>(`${this.baseUrl}/${providerId}/tariffs`, request);
  }

  updateTariff(providerId: number, id: number, request: CreateProviderTariffRequest): Observable<ProviderTariffDto> {
    return this.http.put<ProviderTariffDto>(`${this.baseUrl}/${providerId}/tariffs/${id}`, request);
  }

  deleteTariff(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${providerId}/tariffs/${id}`);
  }

  // ─── Custom Tariffs ──────────────────────────────────────────

  getCustomTariffs(providerId: number): Observable<ProviderCustomTariffDto[]> {
    return this.http.get<ProviderCustomTariffDto[]>(`${this.baseUrl}/${providerId}/custom-tariffs`);
  }

  createCustomTariff(providerId: number, request: CreateProviderCustomTariffRequest): Observable<ProviderCustomTariffDto> {
    return this.http.post<ProviderCustomTariffDto>(`${this.baseUrl}/${providerId}/custom-tariffs`, request);
  }

  updateCustomTariff(providerId: number, id: number, request: CreateProviderCustomTariffRequest): Observable<ProviderCustomTariffDto> {
    return this.http.put<ProviderCustomTariffDto>(`${this.baseUrl}/${providerId}/custom-tariffs/${id}`, request);
  }

  deleteCustomTariff(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${providerId}/custom-tariffs/${id}`);
  }

  // ─── Discounts ───────────────────────────────────────────────

  getDiscounts(providerId: number): Observable<ProviderDiscountDto[]> {
    return this.http.get<ProviderDiscountDto[]>(`${this.baseUrl}/${providerId}/discounts`);
  }

  createDiscount(providerId: number, request: CreateProviderDiscountRequest): Observable<ProviderDiscountDto> {
    return this.http.post<ProviderDiscountDto>(`${this.baseUrl}/${providerId}/discounts`, request);
  }

  updateDiscount(providerId: number, id: number, request: CreateProviderDiscountRequest): Observable<ProviderDiscountDto> {
    return this.http.put<ProviderDiscountDto>(`${this.baseUrl}/${providerId}/discounts/${id}`, request);
  }

  deleteDiscount(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${providerId}/discounts/${id}`);
  }
}
