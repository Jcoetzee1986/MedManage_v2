import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  BookingDto,
  CreateBookingDto,
  UpdateBookingDto,
  BookingSearchFilters,
  PagedResult,
  ApiResponse
} from '../models/booking.models';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/booking`;

  // ─── CRUD ───────────────────────────────────────────────────

  getAll(includeDeleted = false): Observable<BookingDto[]> {
    return this.http
      .get<ApiResponse<BookingDto[]>>(`${this.baseUrl}?includeDeleted=${includeDeleted}`)
      .pipe(map(r => r.data));
  }

  getById(id: number): Observable<BookingDto> {
    return this.http
      .get<ApiResponse<BookingDto>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  getByMemberNumber(memberNumber: string): Observable<BookingDto[]> {
    return this.http
      .get<ApiResponse<BookingDto[]>>(`${this.baseUrl}/member/${encodeURIComponent(memberNumber)}`)
      .pipe(map(r => r.data));
  }

  search(filters: BookingSearchFilters): Observable<PagedResult<BookingDto>> {
    return this.http
      .post<ApiResponse<PagedResult<BookingDto>>>(`${this.baseUrl}/search`, filters)
      .pipe(map(r => r.data));
  }

  create(dto: CreateBookingDto): Observable<BookingDto> {
    return this.http
      .post<ApiResponse<BookingDto>>(this.baseUrl, dto)
      .pipe(map(r => r.data));
  }

  update(id: number, dto: UpdateBookingDto): Observable<BookingDto> {
    return this.http
      .put<ApiResponse<BookingDto>>(`${this.baseUrl}/${id}`, dto)
      .pipe(map(r => r.data));
  }

  delete(id: number): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  // ─── Booking-to-Case Conversion ────────────────────────────

  convertToCase(bookingId: number): Observable<any> {
    return this.http
      .post<ApiResponse<any>>(`${this.baseUrl}/${bookingId}/convert-to-case`, {})
      .pipe(map(r => r.data));
  }
}
