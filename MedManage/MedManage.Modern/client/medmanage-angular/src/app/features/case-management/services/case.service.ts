import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  CaseDto, CreateCaseRequest, UpdateCaseRequest, CaseSearchRequest, PagedResult,
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
  CaseCopyRequest
} from '../models/case.models';

@Injectable({
  providedIn: 'root'
})
export class CaseService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/cases`;

  // ─── Case CRUD ───────────────────────────────────────────────

  search(request: CaseSearchRequest): Observable<PagedResult<CaseDto>> {
    return this.http.post<PagedResult<CaseDto>>(`${this.baseUrl}/search`, request);
  }

  getMyCases(): Observable<CaseDto[]> {
    return this.http.get<CaseDto[]>(`${this.baseUrl}/my-cases`);
  }

  getById(id: number): Observable<CaseDto> {
    return this.http.get<CaseDto>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateCaseRequest): Observable<CaseDto> {
    return this.http.post<CaseDto>(this.baseUrl, request);
  }

  update(id: number, request: UpdateCaseRequest): Observable<CaseDto> {
    return this.http.put<CaseDto>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // ─── CPT Codes ──────────────────────────────────────────────

  getCptCodes(caseId: number): Observable<CaseCptDto[]> {
    return this.http.get<CaseCptDto[]>(`${this.baseUrl}/${caseId}/cpt`);
  }

  createCpt(caseId: number, request: CreateCaseCptRequest): Observable<CaseCptDto> {
    return this.http.post<CaseCptDto>(`${this.baseUrl}/${caseId}/cpt`, request);
  }

  updateCpt(caseId: number, id: number, request: CreateCaseCptRequest): Observable<CaseCptDto> {
    return this.http.put<CaseCptDto>(`${this.baseUrl}/${caseId}/cpt/${id}`, request);
  }

  deleteCpt(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/cpt/${id}`);
  }

  // ─── ICD Codes ──────────────────────────────────────────────

  getIcdCodes(caseId: number): Observable<CaseIcdDto[]> {
    return this.http.get<CaseIcdDto[]>(`${this.baseUrl}/${caseId}/icd`);
  }

  createIcd(caseId: number, request: CreateCaseIcdRequest): Observable<CaseIcdDto> {
    return this.http.post<CaseIcdDto>(`${this.baseUrl}/${caseId}/icd`, request);
  }

  updateIcd(caseId: number, id: number, request: CreateCaseIcdRequest): Observable<CaseIcdDto> {
    return this.http.put<CaseIcdDto>(`${this.baseUrl}/${caseId}/icd/${id}`, request);
  }

  deleteIcd(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/icd/${id}`);
  }

  // ─── Tariffs ────────────────────────────────────────────────

  getTariffs(caseId: number): Observable<CaseTariffDto[]> {
    return this.http.get<CaseTariffDto[]>(`${this.baseUrl}/${caseId}/tariffs`);
  }

  createTariff(caseId: number, request: CreateCaseTariffRequest): Observable<CaseTariffDto> {
    return this.http.post<CaseTariffDto>(`${this.baseUrl}/${caseId}/tariffs`, request);
  }

  updateTariff(caseId: number, id: number, request: CreateCaseTariffRequest): Observable<CaseTariffDto> {
    return this.http.put<CaseTariffDto>(`${this.baseUrl}/${caseId}/tariffs/${id}`, request);
  }

  deleteTariff(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/tariffs/${id}`);
  }

  // ─── Facility Types ─────────────────────────────────────────

  getFacilityTypes(caseId: number): Observable<CaseFacilityTypeDto[]> {
    return this.http.get<CaseFacilityTypeDto[]>(`${this.baseUrl}/${caseId}/facility-types`);
  }

  createFacilityType(caseId: number, request: CreateCaseFacilityTypeRequest): Observable<CaseFacilityTypeDto> {
    return this.http.post<CaseFacilityTypeDto>(`${this.baseUrl}/${caseId}/facility-types`, request);
  }

  updateFacilityType(caseId: number, id: number, request: CreateCaseFacilityTypeRequest): Observable<CaseFacilityTypeDto> {
    return this.http.put<CaseFacilityTypeDto>(`${this.baseUrl}/${caseId}/facility-types/${id}`, request);
  }

  deleteFacilityType(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/facility-types/${id}`);
  }

  // ─── Notes ──────────────────────────────────────────────────

  getNotes(caseId: number): Observable<CaseNoteDto[]> {
    return this.http.get<CaseNoteDto[]>(`${this.baseUrl}/${caseId}/notes`);
  }

  createNote(caseId: number, request: CreateCaseNoteRequest): Observable<CaseNoteDto> {
    return this.http.post<CaseNoteDto>(`${this.baseUrl}/${caseId}/notes`, request);
  }

  updateNote(caseId: number, id: number, request: CreateCaseNoteRequest): Observable<CaseNoteDto> {
    return this.http.put<CaseNoteDto>(`${this.baseUrl}/${caseId}/notes/${id}`, request);
  }

  deleteNote(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/notes/${id}`);
  }

  // ─── Comments ───────────────────────────────────────────────

  getComments(caseId: number): Observable<CaseCommentDto[]> {
    return this.http.get<CaseCommentDto[]>(`${this.baseUrl}/${caseId}/comments`);
  }

  createComment(caseId: number, request: CreateCaseCommentRequest): Observable<CaseCommentDto> {
    return this.http.post<CaseCommentDto>(`${this.baseUrl}/${caseId}/comments`, request);
  }

  updateComment(caseId: number, id: number, request: CreateCaseCommentRequest): Observable<CaseCommentDto> {
    return this.http.put<CaseCommentDto>(`${this.baseUrl}/${caseId}/comments/${id}`, request);
  }

  deleteComment(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/comments/${id}`);
  }

  // ─── Exclusions ─────────────────────────────────────────────

  getExclusions(caseId: number): Observable<CaseExclusionDto[]> {
    return this.http.get<CaseExclusionDto[]>(`${this.baseUrl}/${caseId}/exclusions`);
  }

  createExclusion(caseId: number, request: CreateCaseExclusionRequest): Observable<CaseExclusionDto> {
    return this.http.post<CaseExclusionDto>(`${this.baseUrl}/${caseId}/exclusions`, request);
  }

  updateExclusion(caseId: number, id: number, request: CreateCaseExclusionRequest): Observable<CaseExclusionDto> {
    return this.http.put<CaseExclusionDto>(`${this.baseUrl}/${caseId}/exclusions/${id}`, request);
  }

  deleteExclusion(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/exclusions/${id}`);
  }

  // ─── Checklist ──────────────────────────────────────────────

  getChecklist(caseId: number): Observable<CaseChecklistDto[]> {
    return this.http.get<CaseChecklistDto[]>(`${this.baseUrl}/${caseId}/checklist`);
  }

  createChecklistItem(caseId: number, request: CreateCaseChecklistRequest): Observable<CaseChecklistDto> {
    return this.http.post<CaseChecklistDto>(`${this.baseUrl}/${caseId}/checklist`, request);
  }

  updateChecklistItem(caseId: number, id: number, request: CreateCaseChecklistRequest): Observable<CaseChecklistDto> {
    return this.http.put<CaseChecklistDto>(`${this.baseUrl}/${caseId}/checklist/${id}`, request);
  }

  deleteChecklistItem(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/checklist/${id}`);
  }

  // ─── NAPPI ──────────────────────────────────────────────────

  getNappi(caseId: number): Observable<CaseNappiDto[]> {
    return this.http.get<CaseNappiDto[]>(`${this.baseUrl}/${caseId}/nappi`);
  }

  createNappi(caseId: number, request: CreateCaseNappiRequest): Observable<CaseNappiDto> {
    return this.http.post<CaseNappiDto>(`${this.baseUrl}/${caseId}/nappi`, request);
  }

  updateNappi(caseId: number, id: number, request: CreateCaseNappiRequest): Observable<CaseNappiDto> {
    return this.http.put<CaseNappiDto>(`${this.baseUrl}/${caseId}/nappi/${id}`, request);
  }

  deleteNappi(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/nappi/${id}`);
  }

  // ─── Linked Files ───────────────────────────────────────────

  getFiles(caseId: number): Observable<CaseFileDto[]> {
    return this.http.get<CaseFileDto[]>(`${this.baseUrl}/${caseId}/files`);
  }

  uploadFile(caseId: number, file: File): Observable<CaseFileDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<CaseFileDto>(`${this.baseUrl}/${caseId}/files`, formData);
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
    return this.http.get<CaseLinkDto[]>(`${this.baseUrl}/${caseId}/links`);
  }

  createLink(caseId: number, request: CreateCaseLinkRequest): Observable<CaseLinkDto> {
    return this.http.post<CaseLinkDto>(`${this.baseUrl}/${caseId}/links`, request);
  }

  deleteLink(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/links/${id}`);
  }

  // ─── Case Copy ─────────────────────────────────────────────

  copyCase(caseId: number, request: CaseCopyRequest): Observable<CaseDto> {
    return this.http.post<CaseDto>(`${this.baseUrl}/${caseId}/copy`, request);
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

  // ─── Case Status Transition ─────────────────────────────────

  transitionCase(caseId: number, newStatusId: number): Observable<CaseDto> {
    return this.http.post<CaseDto>(`${this.baseUrl}/${caseId}/transition`, { newStatusId });
  }
}
