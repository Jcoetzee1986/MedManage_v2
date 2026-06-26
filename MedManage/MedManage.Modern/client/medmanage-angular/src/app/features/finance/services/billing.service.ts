import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  CaseBillingDto,
  CreateBillingRequest,
  UpdateBillingRequest,
  BillingSearchRequest,
  PagedResult,
  BulkPaymentRequest,
  BulkPaymentResult,
  RemittanceUpdateRequest,
  BillingSummary,
  BillingCommentDto,
  CreateBillingCommentRequest
} from '../models/billing.models';

interface ApiResponse<T> { success: boolean; data: T; message?: string; }

@Injectable({
  providedIn: 'root'
})
export class BillingService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/casebilling`;

  // ─── CRUD ───────────────────────────────────────────────────

  search(request: BillingSearchRequest): Observable<PagedResult<CaseBillingDto>> {
    return this.http.post<ApiResponse<PagedResult<CaseBillingDto>>>(`${this.baseUrl}/search`, request)
      .pipe(map(r => r.data));
  }

  getById(id: number): Observable<CaseBillingDto> {
    return this.http.get<ApiResponse<CaseBillingDto>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  create(request: CreateBillingRequest): Observable<CaseBillingDto> {
    return this.http.post<ApiResponse<CaseBillingDto>>(this.baseUrl, request)
      .pipe(map(r => r.data));
  }

  update(id: number, request: UpdateBillingRequest): Observable<CaseBillingDto> {
    return this.http.put<ApiResponse<CaseBillingDto>>(`${this.baseUrl}/${id}`, request)
      .pipe(map(r => r.data));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // ─── Bulk Payment ───────────────────────────────────────────

  bulkPayment(request: BulkPaymentRequest): Observable<BulkPaymentResult> {
    return this.http.post<ApiResponse<BulkPaymentResult>>(`${this.baseUrl}/bulk-payment`, request)
      .pipe(map(r => r.data));
  }

  // ─── Remittance ─────────────────────────────────────────────

  updateRemittance(request: RemittanceUpdateRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/remittance`, request);
  }

  getByRemittance(remittanceNumber: string): Observable<CaseBillingDto[]> {
    return this.http.get<ApiResponse<CaseBillingDto[]>>(`${this.baseUrl}/remittance/${encodeURIComponent(remittanceNumber)}`)
      .pipe(map(r => r.data));
  }

  // ─── Summary ────────────────────────────────────────────────

  getSummary(caseId: number): Observable<BillingSummary> {
    return this.http.get<ApiResponse<BillingSummary>>(`${this.baseUrl}/summary/${caseId}`)
      .pipe(map(r => r.data));
  }

  // ─── Billing Comments ───────────────────────────────────────

  getComments(billingId: number): Observable<BillingCommentDto[]> {
    return this.http.get<ApiResponse<BillingCommentDto[]>>(`${this.baseUrl}/${billingId}/comments`)
      .pipe(map(r => r.data));
  }

  addComment(billingId: number, request: CreateBillingCommentRequest): Observable<BillingCommentDto> {
    return this.http.post<ApiResponse<BillingCommentDto>>(`${this.baseUrl}/${billingId}/comments`, request)
      .pipe(map(r => r.data));
  }

  deleteComment(billingId: number, commentId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${billingId}/comments/${commentId}`);
  }
}
