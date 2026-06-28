import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  ReportType,
  ReportFormat,
  CaseLetterParams,
  CasesBetweenDatesParams,
  WipExtractParams,
  BillingSummaryParams
} from '../models/report.models';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/report`;

  // ─── Report Generation ──────────────────────────────────────

  generateCaseLetter(params: CaseLetterParams): Observable<Blob> {
    return this.http.post(`${this.baseUrl}/case-letter`, params, {
      responseType: 'blob'
    });
  }

  generateCasesBetweenDates(params: CasesBetweenDatesParams, format: ReportFormat = 'pdf'): Observable<Blob> {
    return this.http.post(`${this.baseUrl}/cases-between-dates`, { ...params, format }, {
      responseType: 'blob'
    });
  }

  generateWipExtract(params: WipExtractParams, format: ReportFormat = 'excel'): Observable<Blob> {
    return this.http.post(`${this.baseUrl}/wip-extract`, { ...params, format }, {
      responseType: 'blob'
    });
  }

  generateBillingSummary(params: BillingSummaryParams, format: ReportFormat = 'pdf'): Observable<Blob> {
    return this.http.post(`${this.baseUrl}/billing-summary`, { ...params, format }, {
      responseType: 'blob'
    });
  }

  // ─── Download Helper ────────────────────────────────────────

  /**
   * Triggers a file download in the browser from a Blob response.
   */
  downloadBlob(blob: Blob, filename: string): void {
    const url = window.URL.createObjectURL(blob);
    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = filename;
    anchor.click();
    window.URL.revokeObjectURL(url);
  }

  /**
   * Determines the file extension based on the report format.
   */
  getFileExtension(format: ReportFormat): string {
    return format === 'excel' ? 'xlsx' : 'pdf';
  }

  /**
   * Builds a default filename for the report.
   */
  buildFilename(reportType: ReportType, format: ReportFormat): string {
    const timestamp = new Date().toISOString().slice(0, 10);
    const extension = this.getFileExtension(format);
    return `${reportType}_${timestamp}.${extension}`;
  }

  /**
   * Creates an object URL for inline PDF preview from a Blob.
   */
  createPreviewUrl(blob: Blob): string {
    return window.URL.createObjectURL(blob);
  }

  /**
   * Revokes a previously created object URL.
   */
  revokePreviewUrl(url: string): void {
    window.URL.revokeObjectURL(url);
  }
}
