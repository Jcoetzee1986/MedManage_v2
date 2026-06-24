import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
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

@Injectable({
  providedIn: 'root'
})
export class BillingService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/casebilling`;

  // ─── CRUD ───────────────────────────────────────────────────

  search(request: BillingSearchRequest): Observable<PagedResult<CaseBillingDto>> {
    return this.http.post<PagedResult<CaseBillingDto>>(`${this.baseUrl}/search`, request);
  }

  getById(id: number): Observable<CaseBillingDto> {
    return this.http.get<CaseBillingDto>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateBillingRequest): Observable<CaseBillingDto> {
    return this.http.post<CaseBillingDto>(this.baseUrl, request);
  }

  update(id: number, request: UpdateBillingRequest): Observable<CaseBillingDto> {
    return this.http.put<CaseBillingDto>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // ─── Bulk Payment ───────────────────────────────────────────

  bulkPayment(request: BulkPaymentRequest): Observable<BulkPaymentResult> {
    return this.http.post<BulkPaymentResult>(`${this.baseUrl}/bulk-payment`, request);
  }

  // ─── Remittance ─────────────────────────────────────────────

  updateRemittance(request: RemittanceUpdateRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/remittance`, request);
  }

  getByRemittance(remittanceNumber: string): Observable<CaseBillingDto[]> {
    return this.http.get<CaseBillingDto[]>(`${this.baseUrl}/remittance/${encodeURIComponent(remittanceNumber)}`);
  }

  // ─── Summary ────────────────────────────────────────────────

  getSummary(caseId: number): Observable<BillingSummary> {
    return this.http.get<BillingSummary>(`${this.baseUrl}/summary/${caseId}`);
  }

  // ─── Billing Comments ───────────────────────────────────────

  getComments(billingId: number): Observable<BillingCommentDto[]> {
    return this.http.get<BillingCommentDto[]>(`${this.baseUrl}/${billingId}/comments`);
  }

  addComment(billingId: number, request: CreateBillingCommentRequest): Observable<BillingCommentDto> {
    return this.http.post<BillingCommentDto>(`${this.baseUrl}/${billingId}/comments`, request);
  }

  deleteComment(billingId: number, commentId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${billingId}/comments/${commentId}`);
  }
}
