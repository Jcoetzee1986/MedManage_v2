import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap, of, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ReferenceDataItem,
  ReferenceDataRequest,
  ReferenceDataResource
} from '../models/reference-data.models';

/** API response envelope used by the .NET backend */
interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
}

/**
 * Generic reference data service with in-memory caching via shareReplay.
 * Provides CRUD operations for all 17 lookup tables.
 * Cache is automatically invalidated on create/update/delete operations.
 * 
 * The API returns entity-specific property names (e.g., caseStatusId, caseStatus)
 * which are normalized to the generic ReferenceDataItem shape (id, name).
 */
@Injectable({
  providedIn: 'root'
})
export class ReferenceDataService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  /** Cache map: resource name -> cached Observable */
  private cache = new Map<string, Observable<ReferenceDataItem[]>>();

  /**
   * Get all items for a given resource. Results are cached with shareReplay.
   * Normalizes entity-specific property names to generic { id, name } shape.
   */
  getAll(resource: ReferenceDataResource): Observable<ReferenceDataItem[]> {
    if (!this.cache.has(resource)) {
      const request$ = this.http.get<ApiResponse<any[]>>(
        `${this.baseUrl}/${resource}`
      ).pipe(
        map(response => this.normalizeItems(response.data || [])),
        shareReplay({ bufferSize: 1, refCount: true })
      );
      this.cache.set(resource, request$);
    }
    return this.cache.get(resource)!;
  }

  /**
   * Get a single item by ID.
   */
  getById(resource: ReferenceDataResource, id: number): Observable<ReferenceDataItem> {
    return this.http.get<ApiResponse<any>>(`${this.baseUrl}/${resource}/${id}`)
      .pipe(map(response => this.normalizeItem(response.data)));
  }

  /**
   * Create a new reference data item. Invalidates cache for the resource.
   */
  create(resource: ReferenceDataResource, item: ReferenceDataRequest): Observable<ReferenceDataItem> {
    return this.http.post<ApiResponse<any>>(
      `${this.baseUrl}/${resource}`,
      item
    ).pipe(
      map(response => this.normalizeItem(response.data)),
      tap(() => this.invalidateCache(resource))
    );
  }

  /**
   * Update an existing reference data item. Invalidates cache for the resource.
   */
  update(resource: ReferenceDataResource, id: number, item: ReferenceDataRequest): Observable<ReferenceDataItem> {
    return this.http.put<ApiResponse<any>>(
      `${this.baseUrl}/${resource}/${id}`,
      item
    ).pipe(
      map(response => this.normalizeItem(response.data)),
      tap(() => this.invalidateCache(resource))
    );
  }

  /**
   * Delete a reference data item. Invalidates cache for the resource.
   */
  delete(resource: ReferenceDataResource, id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.baseUrl}/${resource}/${id}`
    ).pipe(
      tap(() => this.invalidateCache(resource))
    );
  }

  /**
   * Invalidate cache for a specific resource.
   */
  invalidateCache(resource: ReferenceDataResource): void {
    this.cache.delete(resource);
  }

  /**
   * Invalidate all cached reference data.
   */
  invalidateAll(): void {
    this.cache.clear();
  }

  /**
   * Normalize an array of entity-specific items to generic ReferenceDataItem shape.
   * The API returns different property names per entity:
   *   { caseStatusId: 2, caseStatus: "Overdue" }
   *   { caseTypeId: 1, caseType: "In-Patient" }
   *   { genderId: 1, genderDescription: "Male" }
   *   { languageId: 1, language: "English" }
   * 
   * This method extracts the ID (first property ending in "Id") and name (first string property).
   */
  private normalizeItems(items: any[]): ReferenceDataItem[] {
    if (!items || !Array.isArray(items)) return [];
    return items.map(item => this.normalizeItem(item));
  }

  private normalizeItem(item: any): ReferenceDataItem {
    if (!item) return { id: 0, name: '' };

    // If already in normalized shape
    if ('id' in item && 'name' in item) {
      return { id: item.id, name: item.name, description: item.description, isActive: item.isActive };
    }

    const keys = Object.keys(item);

    // Find the ID field (property ending in "Id" or "ID" that is a number)
    const idKey = keys.find(k =>
      (k.toLowerCase().endsWith('id') && k.toLowerCase() !== 'datedeleted') &&
      typeof item[k] === 'number'
    );

    // Find the name field — first string property that isn't a date or ID field
    const nameKey = keys.find(k => {
      if (k.toLowerCase().endsWith('id')) return false;
      if (k.toLowerCase().startsWith('date')) return false;
      if (k === 'isActive' || k === 'dateCreated' || k === 'dateModified' || k === 'dateDeleted') return false;
      return typeof item[k] === 'string' && item[k] !== null;
    });

    return {
      id: idKey ? item[idKey] : 0,
      name: nameKey ? item[nameKey] : '',
      description: undefined,
      isActive: item.isActive ?? item.visible ?? true
    };
  }
}
