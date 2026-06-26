import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  CaseDto, CreateCaseRequest, UpdateCaseRequest, CaseSearchRequest, PagedResult, ApiResponse,
  CaseCptDto, CreateCaseCptRequest,
  CaseIcdDto, CreateCaseIcdRequest,
  CaseTariffDto, CreateCaseTariffRequest,
  CaseFacilityTypeDto, CreateCaseFacilityTypeRequest,
  CaseNoteDto, CreateCaseNoteRequest,
  CaseCommentDto, CreateCaseCommentRequest,
  CaseExclusionDto, CreateCaseExclusionRequest,
  CaseChecklistDto, CreateCaseChecklistRequest,
  CaseNappiDto, CreateCaseNappiRequest,
  CaseFileDto,
  CaseLinkDto, CreateCaseLinkRequest,
  CaseCopyRequest,
  DuplicateCheckResult
} from '../models/case.models';

@Injectable({
  providedIn: 'root'
})
export class CaseService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/cases`;

  // ─── Case CRUD ───────────────────────────────────────────────

  search(request: CaseSearchRequest): Observable<PagedResult<CaseDto>> {
    return this.http.post<ApiResponse<PagedResult<CaseDto>>>(`${this.baseUrl}/search`, request)
      .pipe(map(r => r.data));
  }

  getMyCases(): Observable<CaseDto[]> {
    return this.http.get<ApiResponse<CaseDto[]>>(`${this.baseUrl}/my-cases`)
      .pipe(map(r => r.data));
  }

  getById(id: number): Observable<CaseDto> {
    return this.http.get<ApiResponse<CaseDto>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  create(request: CreateCaseRequest): Observable<CaseDto> {
    return this.http.post<ApiResponse<CaseDto>>(this.baseUrl, request)
      .pipe(map(r => r.data));
  }

  update(id: number, request: UpdateCaseRequest): Observable<CaseDto> {
    return this.http.put<ApiResponse<CaseDto>>(`${this.baseUrl}/${id}`, request)
      .pipe(map(r => r.data));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  /** Generate a report for cases between dates */
  exportReport(request: CaseSearchRequest): Observable<Blob> {
    return this.http.post(`${environment.apiUrl}/reports/cases-between-dates`, request, {
      responseType: 'blob'
    });
  }

  // ─── CPT Codes ──────────────────────────────────────────────

  getCptCodes(caseId: number): Observable<CaseCptDto[]> {
    return this.http.get<ApiResponse<CaseCptDto[]>>(`${this.baseUrl}/${caseId}/cpt`)
      .pipe(map(r => r.data));
  }

  createCpt(caseId: number, request: CreateCaseCptRequest): Observable<CaseCptDto> {
    return this.http.post<ApiResponse<CaseCptDto>>(`${this.baseUrl}/${caseId}/cpt`, request)
      .pipe(map(r => r.data));
  }

  updateCpt(caseId: number, id: number, request: CreateCaseCptRequest): Observable<CaseCptDto> {
    return this.http.put<ApiResponse<CaseCptDto>>(`${this.baseUrl}/${caseId}/cpt/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteCpt(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/cpt/${id}`);
  }

  // ─── ICD Codes ──────────────────────────────────────────────

  getIcdCodes(caseId: number): Observable<CaseIcdDto[]> {
    return this.http.get<ApiResponse<CaseIcdDto[]>>(`${this.baseUrl}/${caseId}/icd`)
      .pipe(map(r => r.data));
  }

  createIcd(caseId: number, request: CreateCaseIcdRequest): Observable<CaseIcdDto> {
    return this.http.post<ApiResponse<CaseIcdDto>>(`${this.baseUrl}/${caseId}/icd`, request)
      .pipe(map(r => r.data));
  }

  updateIcd(caseId: number, id: number, request: CreateCaseIcdRequest): Observable<CaseIcdDto> {
    return this.http.put<ApiResponse<CaseIcdDto>>(`${this.baseUrl}/${caseId}/icd/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteIcd(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/icd/${id}`);
  }

  // ─── Tariffs ────────────────────────────────────────────────

  getTariffs(caseId: number): Observable<CaseTariffDto[]> {
    return this.http.get<ApiResponse<CaseTariffDto[]>>(`${this.baseUrl}/${caseId}/tariffs`)
      .pipe(map(r => r.data));
  }

  createTariff(caseId: number, request: CreateCaseTariffRequest): Observable<CaseTariffDto> {
    return this.http.post<ApiResponse<CaseTariffDto>>(`${this.baseUrl}/${caseId}/tariffs`, request)
      .pipe(map(r => r.data));
  }

  updateTariff(caseId: number, id: number, request: CreateCaseTariffRequest): Observable<CaseTariffDto> {
    return this.http.put<ApiResponse<CaseTariffDto>>(`${this.baseUrl}/${caseId}/tariffs/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteTariff(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/tariffs/${id}`);
  }

  // ─── Facility Types ─────────────────────────────────────────

  getFacilityTypes(caseId: number): Observable<CaseFacilityTypeDto[]> {
    return this.http.get<ApiResponse<CaseFacilityTypeDto[]>>(`${this.baseUrl}/${caseId}/facility-types`)
      .pipe(map(r => r.data));
  }

  createFacilityType(caseId: number, request: CreateCaseFacilityTypeRequest): Observable<CaseFacilityTypeDto> {
    return this.http.post<ApiResponse<CaseFacilityTypeDto>>(`${this.baseUrl}/${caseId}/facility-types`, request)
      .pipe(map(r => r.data));
  }

  updateFacilityType(caseId: number, id: number, request: CreateCaseFacilityTypeRequest): Observable<CaseFacilityTypeDto> {
    return this.http.put<ApiResponse<CaseFacilityTypeDto>>(`${this.baseUrl}/${caseId}/facility-types/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteFacilityType(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/facility-types/${id}`);
  }

  // ─── Notes ──────────────────────────────────────────────────

  getNotes(caseId: number): Observable<CaseNoteDto[]> {
    return this.http.get<ApiResponse<CaseNoteDto[]>>(`${this.baseUrl}/${caseId}/notes`)
      .pipe(map(r => r.data));
  }

  createNote(caseId: number, request: CreateCaseNoteRequest): Observable<CaseNoteDto> {
    return this.http.post<ApiResponse<CaseNoteDto>>(`${this.baseUrl}/${caseId}/notes`, request)
      .pipe(map(r => r.data));
  }

  updateNote(caseId: number, id: number, request: CreateCaseNoteRequest): Observable<CaseNoteDto> {
    return this.http.put<ApiResponse<CaseNoteDto>>(`${this.baseUrl}/${caseId}/notes/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteNote(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/notes/${id}`);
  }

  // ─── Comments ───────────────────────────────────────────────

  getComments(caseId: number): Observable<CaseCommentDto[]> {
    return this.http.get<ApiResponse<CaseCommentDto[]>>(`${this.baseUrl}/${caseId}/comments`)
      .pipe(map(r => r.data));
  }

  createComment(caseId: number, request: CreateCaseCommentRequest): Observable<CaseCommentDto> {
    return this.http.post<ApiResponse<CaseCommentDto>>(`${this.baseUrl}/${caseId}/comments`, request)
      .pipe(map(r => r.data));
  }

  updateComment(caseId: number, id: number, request: CreateCaseCommentRequest): Observable<CaseCommentDto> {
    return this.http.put<ApiResponse<CaseCommentDto>>(`${this.baseUrl}/${caseId}/comments/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteComment(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/comments/${id}`);
  }

  // ─── Exclusions ─────────────────────────────────────────────

  getExclusions(caseId: number): Observable<CaseExclusionDto[]> {
    return this.http.get<ApiResponse<CaseExclusionDto[]>>(`${this.baseUrl}/${caseId}/exclusions`)
      .pipe(map(r => r.data));
  }

  createExclusion(caseId: number, request: CreateCaseExclusionRequest): Observable<CaseExclusionDto> {
    return this.http.post<ApiResponse<CaseExclusionDto>>(`${this.baseUrl}/${caseId}/exclusions`, request)
      .pipe(map(r => r.data));
  }

  updateExclusion(caseId: number, id: number, request: CreateCaseExclusionRequest): Observable<CaseExclusionDto> {
    return this.http.put<ApiResponse<CaseExclusionDto>>(`${this.baseUrl}/${caseId}/exclusions/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteExclusion(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/exclusions/${id}`);
  }

  // ─── Checklist ──────────────────────────────────────────────

  getChecklist(caseId: number): Observable<CaseChecklistDto[]> {
    return this.http.get<ApiResponse<CaseChecklistDto[]>>(`${this.baseUrl}/${caseId}/checklist`)
      .pipe(map(r => r.data));
  }

  createChecklistItem(caseId: number, request: CreateCaseChecklistRequest): Observable<CaseChecklistDto> {
    return this.http.post<ApiResponse<CaseChecklistDto>>(`${this.baseUrl}/${caseId}/checklist`, request)
      .pipe(map(r => r.data));
  }

  updateChecklistItem(caseId: number, id: number, request: CreateCaseChecklistRequest): Observable<CaseChecklistDto> {
    return this.http.put<ApiResponse<CaseChecklistDto>>(`${this.baseUrl}/${caseId}/checklist/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteChecklistItem(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/checklist/${id}`);
  }

  // ─── NAPPI ──────────────────────────────────────────────────

  getNappi(caseId: number): Observable<CaseNappiDto[]> {
    return this.http.get<ApiResponse<CaseNappiDto[]>>(`${this.baseUrl}/${caseId}/nappi`)
      .pipe(map(r => r.data));
  }

  createNappi(caseId: number, request: CreateCaseNappiRequest): Observable<CaseNappiDto> {
    return this.http.post<ApiResponse<CaseNappiDto>>(`${this.baseUrl}/${caseId}/nappi`, request)
      .pipe(map(r => r.data));
  }

  updateNappi(caseId: number, id: number, request: CreateCaseNappiRequest): Observable<CaseNappiDto> {
    return this.http.put<ApiResponse<CaseNappiDto>>(`${this.baseUrl}/${caseId}/nappi/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteNappi(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/nappi/${id}`);
  }

  // ─── Linked Files ───────────────────────────────────────────

  getFiles(caseId: number): Observable<CaseFileDto[]> {
    return this.http.get<ApiResponse<CaseFileDto[]>>(`${this.baseUrl}/${caseId}/files`)
      .pipe(map(r => r.data));
  }

  uploadFile(caseId: number, file: File): Observable<CaseFileDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<ApiResponse<CaseFileDto>>(`${this.baseUrl}/${caseId}/files`, formData)
      .pipe(map(r => r.data));
  }

  deleteFile(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/files/${id}`);
  }

  downloadFile(caseId: number, id: number): Observable<Blob> {
    return this.http.get(`${this.baseUrl}/${caseId}/files/${id}/download`, {
      responseType: 'blob'
    });
  }

  // ─── Linked Cases ──────────────────────────────────────────

  getLinks(caseId: number): Observable<CaseLinkDto[]> {
    return this.http.get<ApiResponse<CaseLinkDto[]>>(`${this.baseUrl}/${caseId}/links`)
      .pipe(map(r => r.data));
  }

  createLink(caseId: number, request: CreateCaseLinkRequest): Observable<CaseLinkDto> {
    return this.http.post<ApiResponse<CaseLinkDto>>(`${this.baseUrl}/${caseId}/links`, request)
      .pipe(map(r => r.data));
  }

  deleteLink(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/links/${id}`);
  }

  // ─── Case Copy ─────────────────────────────────────────────

  copyCase(caseId: number, request: CaseCopyRequest): Observable<CaseDto> {
    return this.http.post<ApiResponse<CaseDto>>(`${this.baseUrl}/${caseId}/copy`, request)
      .pipe(map(r => r.data));
  }

  // ─── Case Lock ─────────────────────────────────────────────

  lockCase(caseId: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${caseId}/lock`, {});
  }

  unlockCase(caseId: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/lock`);
  }

  /** Refresh the lock heartbeat (call periodically to prevent expiry) */
  refreshLock(caseId: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${caseId}/lock`, {});
  }

  /** Release ALL locks held by the current user (call on logout) */
  releaseAllMyLocks(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/locks/mine`);
  }

  // ─── Case Duplicate Check ────────────────────────────────────

  checkDuplicate(request: CreateCaseRequest): Observable<DuplicateCheckResult> {
    return this.http.post<ApiResponse<DuplicateCheckResult>>(`${this.baseUrl}/check-duplicate`, request)
      .pipe(map(r => r.data));
  }

  // ─── Case Status Transition ─────────────────────────────────

  transitionCase(caseId: number, newStatusId: number): Observable<CaseDto> {
    return this.http.post<ApiResponse<CaseDto>>(`${this.baseUrl}/${caseId}/transition`, { newStatusId })
      .pipe(map(r => r.data));
  }
}
