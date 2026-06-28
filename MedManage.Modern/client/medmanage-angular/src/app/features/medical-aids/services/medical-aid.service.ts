import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  MedicalAidDto, CreateMedicalAidRequest, UpdateMedicalAidRequest,
  MedicalAidProductDto, CreateMedicalAidProductRequest, UpdateMedicalAidProductRequest,
  MedicalAidExclusionDto, CreateMedicalAidExclusionRequest,
  MedicalAidTariffDto, CreateMedicalAidTariffRequest,
  MedicalAidSearchFilters
} from '../models/medical-aid.models';

interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
}

@Injectable({
  providedIn: 'root'
})
export class MedicalAidService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/medical-aids`;

  // ─── Medical Aid CRUD ────────────────────────────────────────

  getAll(includeDeleted = false): Observable<MedicalAidDto[]> {
    return this.http.get<ApiResponse<MedicalAidDto[]>>(`${this.baseUrl}?includeDeleted=${includeDeleted}`)
      .pipe(map(r => r.data));
  }

  getActive(mainClientId?: number): Observable<MedicalAidDto[]> {
    const params = mainClientId ? `?mainClientId=${mainClientId}` : '';
    return this.http.get<ApiResponse<MedicalAidDto[]>>(`${this.baseUrl}/active${params}`)
      .pipe(map(r => r.data));
  }

  getById(id: number): Observable<MedicalAidDto> {
    return this.http.get<ApiResponse<MedicalAidDto>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  search(filters: MedicalAidSearchFilters): Observable<MedicalAidDto[]> {
    return this.http.post<ApiResponse<MedicalAidDto[]>>(`${this.baseUrl}/search`, filters)
      .pipe(map(r => r.data));
  }

  create(request: CreateMedicalAidRequest): Observable<MedicalAidDto> {
    return this.http.post<ApiResponse<MedicalAidDto>>(this.baseUrl, request)
      .pipe(map(r => r.data));
  }

  update(id: number, request: UpdateMedicalAidRequest): Observable<MedicalAidDto> {
    return this.http.put<ApiResponse<MedicalAidDto>>(`${this.baseUrl}/${id}`, request)
      .pipe(map(r => r.data));
  }

  delete(id: number): Observable<boolean> {
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  // ─── Product CRUD ────────────────────────────────────────────

  getProducts(medicalAidId: number): Observable<MedicalAidProductDto[]> {
    return this.http.get<ApiResponse<MedicalAidProductDto[]>>(`${this.baseUrl}/${medicalAidId}/products`)
      .pipe(map(r => r.data));
  }

  createProduct(medicalAidId: number, request: CreateMedicalAidProductRequest): Observable<MedicalAidProductDto> {
    return this.http.post<ApiResponse<MedicalAidProductDto>>(`${this.baseUrl}/${medicalAidId}/products`, request)
      .pipe(map(r => r.data));
  }

  updateProduct(medicalAidId: number, productId: number, request: UpdateMedicalAidProductRequest): Observable<MedicalAidProductDto> {
    return this.http.put<ApiResponse<MedicalAidProductDto>>(`${this.baseUrl}/${medicalAidId}/products/${productId}`, request)
      .pipe(map(r => r.data));
  }

  deleteProduct(medicalAidId: number, productId: number): Observable<boolean> {
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/${medicalAidId}/products/${productId}`)
      .pipe(map(r => r.data));
  }

  // ─── Exclusion CRUD ──────────────────────────────────────────

  getExclusions(medicalAidId: number): Observable<MedicalAidExclusionDto[]> {
    return this.http.get<ApiResponse<MedicalAidExclusionDto[]>>(`${this.baseUrl}/${medicalAidId}/exclusions`)
      .pipe(map(r => r.data));
  }

  addExclusion(medicalAidId: number, request: CreateMedicalAidExclusionRequest): Observable<MedicalAidExclusionDto> {
    return this.http.post<ApiResponse<MedicalAidExclusionDto>>(`${this.baseUrl}/${medicalAidId}/exclusions`, request)
      .pipe(map(r => r.data));
  }

  removeExclusion(medicalAidId: number, baseTariffId: string): Observable<boolean> {
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/${medicalAidId}/exclusions/${encodeURIComponent(baseTariffId)}`)
      .pipe(map(r => r.data));
  }

  // ─── Tariff Association ──────────────────────────────────────

  getTariffs(medicalAidId: number): Observable<MedicalAidTariffDto[]> {
    return this.http.get<ApiResponse<MedicalAidTariffDto[]>>(`${this.baseUrl}/${medicalAidId}/tariffs`)
      .pipe(map(r => r.data));
  }

  addTariff(medicalAidId: number, request: CreateMedicalAidTariffRequest): Observable<MedicalAidTariffDto> {
    return this.http.post<ApiResponse<MedicalAidTariffDto>>(`${this.baseUrl}/${medicalAidId}/tariffs`, request)
      .pipe(map(r => r.data));
  }

  removeTariff(medicalAidId: number, tariffNameId: number): Observable<boolean> {
    return this.http.delete<ApiResponse<boolean>>(`${this.baseUrl}/${medicalAidId}/tariffs/${tariffNameId}`)
      .pipe(map(r => r.data));
  }
}
