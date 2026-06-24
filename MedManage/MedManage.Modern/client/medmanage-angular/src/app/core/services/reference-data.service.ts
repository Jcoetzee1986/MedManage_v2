import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay, tap, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ReferenceDataItem,
  ReferenceDataRequest,
  ReferenceDataResource
} from '../models/reference-data.models';

/**
 * Generic reference data service with in-memory caching via shareReplay.
 * Provides CRUD operations for all 17 lookup tables.
 * Cache is automatically invalidated on create/update/delete operations.
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
   * Subsequent calls return the cached observable until invalidated.
   */
  getAll(resource: ReferenceDataResource): Observable<ReferenceDataItem[]> {
    if (!this.cache.has(resource)) {
      const request$ = this.http.get<ReferenceDataItem[]>(
        `${this.baseUrl}/${resource}`
      ).pipe(
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
    return this.http.get<ReferenceDataItem>(`${this.baseUrl}/${resource}/${id}`);
  }

  /**
   * Create a new reference data item. Invalidates cache for the resource.
   */
  create(resource: ReferenceDataResource, item: ReferenceDataRequest): Observable<ReferenceDataItem> {
    return this.http.post<ReferenceDataItem>(
      `${this.baseUrl}/${resource}`,
      item
    ).pipe(
      tap(() => this.invalidateCache(resource))
    );
  }

  /**
   * Update an existing reference data item. Invalidates cache for the resource.
   */
  update(resource: ReferenceDataResource, id: number, item: ReferenceDataRequest): Observable<ReferenceDataItem> {
    return this.http.put<ReferenceDataItem>(
      `${this.baseUrl}/${resource}/${id}`,
      item
    ).pipe(
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
   * Next call to getAll() will fetch fresh data from the API.
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
}
