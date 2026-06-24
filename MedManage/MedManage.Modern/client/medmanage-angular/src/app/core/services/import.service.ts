import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ImportResultDto, ImportHistoryDto, ApiResponse } from '../models/import.models';

/**
 * Service for data import operations.
 * Provides file upload with progress tracking for DRD members, NAPPI codes, and billing.
 */
@Injectable({
  providedIn: 'root'
})
export class ImportService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/imports`;

  /** Upload and import DRD member file */
  importMembers(file: File): Observable<{ progress: number; result?: ImportResultDto }> {
    return this.uploadFile(`${this.baseUrl}/members`, file);
  }

  /** Upload and import NAPPI codes file */
  importNappiCodes(file: File): Observable<{ progress: number; result?: ImportResultDto }> {
    return this.uploadFile(`${this.baseUrl}/nappi`, file);
  }

  /** Upload and import billing file */
  importBilling(file: File): Observable<{ progress: number; result?: ImportResultDto }> {
    return this.uploadFile(`${this.baseUrl}/billing`, file);
  }

  /** Get import history */
  getHistory(importType?: string, limit?: number): Observable<ImportHistoryDto[]> {
    let url = `${this.baseUrl}/history`;
    const params: string[] = [];
    if (importType) params.push(`importType=${encodeURIComponent(importType)}`);
    if (limit) params.push(`limit=${limit}`);
    if (params.length) url += '?' + params.join('&');

    return this.http.get<ApiResponse<ImportHistoryDto[]>>(url)
      .pipe(map(r => r.data));
  }

  private uploadFile(url: string, file: File): Observable<{ progress: number; result?: ImportResultDto }> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    const req = new HttpRequest('POST', url, formData, {
      reportProgress: true
    });

    return new Observable(observer => {
      this.http.request(req).subscribe({
        next: (event) => {
          if (event.type === HttpEventType.UploadProgress) {
            const progress = event.total
              ? Math.round(100 * event.loaded / event.total)
              : 0;
            observer.next({ progress });
          } else if (event.type === HttpEventType.Response) {
            const body = event.body as ApiResponse<ImportResultDto>;
            observer.next({ progress: 100, result: body?.data });
            observer.complete();
          }
        },
        error: (err) => observer.error(err)
      });
    });
  }
}
