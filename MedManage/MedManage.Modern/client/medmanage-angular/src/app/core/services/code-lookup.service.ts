import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';

/** Code type enum for the reusable lookup dialog */
export type CodeType = 'cpt' | 'icd' | 'nappi';

/** Lightweight result from code lookup endpoints */
export interface CodeLookupResult {
  id: number;
  code: string;
  description: string;
}

/** Extended NAPPI result including date range */
export interface NappiCodeLookupResult extends CodeLookupResult {
  startDate?: string | null;
  endDate?: string | null;
}

/** Standard API response wrapper matching the backend ApiResponse<T> */
interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
  errors: string[];
}

/**
 * Service for searching CPT, ICD, and NAPPI codes via the backend typeahead API.
 * Used by the CodeLookupDialogComponent.
 */
@Injectable({
  providedIn: 'root'
})
export class CodeLookupService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/codes`;

  /**
   * Search CPT codes by code or description.
   */
  searchCpt(query: string): Observable<CodeLookupResult[]> {
    const params = new HttpParams().set('q', query);
    return this.http.get<ApiResponse<CodeLookupResult[]>>(`${this.baseUrl}/cpt`, { params })
      .pipe(map(r => r.data));
  }

  /**
   * Search ICD codes by code or description.
   */
  searchIcd(query: string): Observable<CodeLookupResult[]> {
    const params = new HttpParams().set('q', query);
    return this.http.get<ApiResponse<CodeLookupResult[]>>(`${this.baseUrl}/icd`, { params })
      .pipe(map(r => r.data));
  }

  /**
   * Search NAPPI codes by code or description, optionally filtered by effective date.
   */
  searchNappi(query: string, effectiveDate?: Date): Observable<NappiCodeLookupResult[]> {
    let params = new HttpParams().set('q', query);
    if (effectiveDate) {
      params = params.set('effectiveDate', effectiveDate.toISOString());
    }
    return this.http.get<ApiResponse<NappiCodeLookupResult[]>>(`${this.baseUrl}/nappi`, { params })
      .pipe(map(r => r.data));
  }

  /**
   * Generic search method that dispatches to the appropriate code type endpoint.
   */
  search(codeType: CodeType, query: string, effectiveDate?: Date): Observable<CodeLookupResult[]> {
    switch (codeType) {
      case 'cpt':
        return this.searchCpt(query);
      case 'icd':
        return this.searchIcd(query);
      case 'nappi':
        return this.searchNappi(query, effectiveDate);
    }
  }
}
