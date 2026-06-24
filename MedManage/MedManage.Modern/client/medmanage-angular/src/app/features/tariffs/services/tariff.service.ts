import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  BaseTariffDto, CreateBaseTariffRequest, UpdateBaseTariffRequest,
  TariffRateDto, CreateTariffRateRequest, UpdateTariffRateRequest,
  TariffNameDto, CreateTariffNameRequest, UpdateTariffNameRequest,
  TariffLookupResult, TariffCalculationResult,
  ProviderTariffAssignmentDto, CreateProviderTariffAssignmentRequest,
  ProviderCustomTariffDto, CreateProviderCustomTariffRequest
} from '../models/tariff.models';

@Injectable({
  providedIn: 'root'
})
export class TariffService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/tariff`;
  private readonly providerUrl = `${environment.apiUrl}/service-providers`;

  // ─── Base Tariff CRUD ────────────────────────────────────────

  getBaseTariffs(): Observable<BaseTariffDto[]> {
    return this.http.get<BaseTariffDto[]>(`${this.baseUrl}/base`);
  }

  getBaseTariffById(id: number): Observable<BaseTariffDto> {
    return this.http.get<BaseTariffDto>(`${this.baseUrl}/base/${id}`);
  }

  createBaseTariff(request: CreateBaseTariffRequest): Observable<BaseTariffDto> {
    return this.http.post<BaseTariffDto>(`${this.baseUrl}/base`, request);
  }

  updateBaseTariff(id: number, request: UpdateBaseTariffRequest): Observable<BaseTariffDto> {
    return this.http.put<BaseTariffDto>(`${this.baseUrl}/base/${id}`, request);
  }

  deleteBaseTariff(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/base/${id}`);
  }

  // ─── Tariff Rates CRUD ───────────────────────────────────────

  getRates(): Observable<TariffRateDto[]> {
    return this.http.get<TariffRateDto[]>(`${this.baseUrl}/rates`);
  }

  getRatesByTariffId(tariffId: number): Observable<TariffRateDto[]> {
    const params = new HttpParams().set('tariffId', tariffId.toString());
    return this.http.get<TariffRateDto[]>(`${this.baseUrl}/rates`, { params });
  }

  createRate(request: CreateTariffRateRequest): Observable<TariffRateDto> {
    return this.http.post<TariffRateDto>(`${this.baseUrl}/rates`, request);
  }

  updateRate(id: number, request: UpdateTariffRateRequest): Observable<TariffRateDto> {
    return this.http.put<TariffRateDto>(`${this.baseUrl}/rates/${id}`, request);
  }

  deleteRate(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/rates/${id}`);
  }

  // ─── Tariff Names CRUD ───────────────────────────────────────

  getNames(): Observable<TariffNameDto[]> {
    return this.http.get<TariffNameDto[]>(`${this.baseUrl}/names`);
  }

  getNamesByTariffId(tariffId: number): Observable<TariffNameDto[]> {
    const params = new HttpParams().set('tariffId', tariffId.toString());
    return this.http.get<TariffNameDto[]>(`${this.baseUrl}/names`, { params });
  }

  createName(request: CreateTariffNameRequest): Observable<TariffNameDto> {
    return this.http.post<TariffNameDto>(`${this.baseUrl}/names`, request);
  }

  updateName(id: number, request: UpdateTariffNameRequest): Observable<TariffNameDto> {
    return this.http.put<TariffNameDto>(`${this.baseUrl}/names/${id}`, request);
  }

  deleteName(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/names/${id}`);
  }

  // ─── Tariff Lookup ───────────────────────────────────────────

  lookup(query: string): Observable<TariffLookupResult[]> {
    const params = new HttpParams().set('query', query);
    return this.http.get<TariffLookupResult[]>(`${this.baseUrl}/lookup`, { params });
  }

  // ─── Tariff Calculation ──────────────────────────────────────

  calculate(caseId: number): Observable<TariffCalculationResult> {
    return this.http.get<TariffCalculationResult>(`${this.baseUrl}/calculate/${caseId}`);
  }

  // ─── Provider Tariff Assignments ─────────────────────────────

  getProviderTariffs(providerId: number): Observable<ProviderTariffAssignmentDto[]> {
    return this.http.get<ProviderTariffAssignmentDto[]>(`${this.providerUrl}/${providerId}/tariffs`);
  }

  createProviderTariff(providerId: number, request: CreateProviderTariffAssignmentRequest): Observable<ProviderTariffAssignmentDto> {
    return this.http.post<ProviderTariffAssignmentDto>(`${this.providerUrl}/${providerId}/tariffs`, request);
  }

  updateProviderTariff(providerId: number, id: number, request: CreateProviderTariffAssignmentRequest): Observable<ProviderTariffAssignmentDto> {
    return this.http.put<ProviderTariffAssignmentDto>(`${this.providerUrl}/${providerId}/tariffs/${id}`, request);
  }

  deleteProviderTariff(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.providerUrl}/${providerId}/tariffs/${id}`);
  }

  // ─── Provider Custom Tariffs ─────────────────────────────────

  getProviderCustomTariffs(providerId: number): Observable<ProviderCustomTariffDto[]> {
    return this.http.get<ProviderCustomTariffDto[]>(`${this.providerUrl}/${providerId}/custom-tariffs`);
  }

  createProviderCustomTariff(providerId: number, request: CreateProviderCustomTariffRequest): Observable<ProviderCustomTariffDto> {
    return this.http.post<ProviderCustomTariffDto>(`${this.providerUrl}/${providerId}/custom-tariffs`, request);
  }

  updateProviderCustomTariff(providerId: number, id: number, request: CreateProviderCustomTariffRequest): Observable<ProviderCustomTariffDto> {
    return this.http.put<ProviderCustomTariffDto>(`${this.providerUrl}/${providerId}/custom-tariffs/${id}`, request);
  }

  deleteProviderCustomTariff(providerId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.providerUrl}/${providerId}/custom-tariffs/${id}`);
  }
}
