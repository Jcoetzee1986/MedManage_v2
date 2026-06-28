import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import { DocumentDto, ApiResponse } from '../models/document.models';

/**
 * Service for document management operations.
 * Supports upload with progress, download, thumbnail retrieval, listing, and deletion.
 */
@Injectable({
  providedIn: 'root'
})
export class DocumentService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/documents`;

  /** List documents for a given entity */
  getByEntity(entityType: string, entityId: number): Observable<DocumentDto[]> {
    return this.http
      .get<ApiResponse<DocumentDto[]>>(`${this.baseUrl}?entityType=${encodeURIComponent(entityType)}&entityId=${entityId}`)
      .pipe(map(r => r.data));
  }

  /** Upload a file linked to an entity with progress tracking */
  upload(file: File, entityType: string, entityId: number): Observable<{ progress: number; result?: DocumentDto }> {
    const formData = new FormData();
    formData.append('file', file, file.name);
    formData.append('entityType', entityType);
    formData.append('entityId', entityId.toString());

    const req = new HttpRequest('POST', this.baseUrl, formData, {
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
            const body = event.body as ApiResponse<DocumentDto>;
            observer.next({ progress: 100, result: body?.data });
            observer.complete();
          }
        },
        error: (err) => observer.error(err)
      });
    });
  }

  /** Get the download URL for a document */
  getDownloadUrl(documentId: number): string {
    return `${this.baseUrl}/${documentId}`;
  }

  /** Get the thumbnail URL for an image document */
  getThumbnailUrl(documentId: number): string {
    return `${this.baseUrl}/${documentId}/thumbnail`;
  }

  /** Delete a document */
  delete(documentId: number): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.baseUrl}/${documentId}`)
      .pipe(map(r => r.data));
  }
}
