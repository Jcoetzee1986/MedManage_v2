import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  EpisodeDto,
  CreateEpisodeDto,
  UpdateEpisodeDto,
  EpisodeSearchFilters,
  EpisodeCaseDto,
  LinkCaseToEpisodeDto,
  PagedResult,
  ApiResponse
} from '../models/episode.models';

@Injectable({
  providedIn: 'root'
})
export class EpisodeService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/episodes`;

  // ─── Episode CRUD ───────────────────────────────────────────

  getAll(includeDeleted = false): Observable<EpisodeDto[]> {
    return this.http
      .get<ApiResponse<EpisodeDto[]>>(`${this.baseUrl}?includeDeleted=${includeDeleted}`)
      .pipe(map(r => r.data));
  }

  getById(id: number): Observable<EpisodeDto> {
    return this.http
      .get<ApiResponse<EpisodeDto>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  getByIdWithCases(id: number): Observable<EpisodeDto> {
    return this.http
      .get<ApiResponse<EpisodeDto>>(`${this.baseUrl}/${id}/with-cases`)
      .pipe(map(r => r.data));
  }

  search(filters: EpisodeSearchFilters): Observable<PagedResult<EpisodeDto>> {
    return this.http
      .post<ApiResponse<PagedResult<EpisodeDto>>>(`${this.baseUrl}/search`, filters)
      .pipe(map(r => r.data));
  }

  create(dto: CreateEpisodeDto): Observable<EpisodeDto> {
    return this.http
      .post<ApiResponse<EpisodeDto>>(this.baseUrl, dto)
      .pipe(map(r => r.data));
  }

  update(id: number, dto: UpdateEpisodeDto): Observable<EpisodeDto> {
    return this.http
      .put<ApiResponse<EpisodeDto>>(`${this.baseUrl}/${id}`, dto)
      .pipe(map(r => r.data));
  }

  delete(id: number): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  // ─── Episode-Case Linking ───────────────────────────────────

  getCases(episodeId: number): Observable<EpisodeCaseDto[]> {
    return this.http
      .get<ApiResponse<EpisodeCaseDto[]>>(`${this.baseUrl}/${episodeId}/cases`)
      .pipe(map(r => r.data));
  }

  linkCase(episodeId: number, dto: LinkCaseToEpisodeDto): Observable<EpisodeCaseDto> {
    return this.http
      .post<ApiResponse<EpisodeCaseDto>>(`${this.baseUrl}/${episodeId}/cases`, dto)
      .pipe(map(r => r.data));
  }

  unlinkCase(episodeId: number, caseId: number): Observable<boolean> {
    return this.http
      .delete<ApiResponse<boolean>>(`${this.baseUrl}/${episodeId}/cases/${caseId}`)
      .pipe(map(r => r.data));
  }
}
