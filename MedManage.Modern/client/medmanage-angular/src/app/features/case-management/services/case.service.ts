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

  getMyCases(mainClientId?: number): Observable<CaseDto[]> {
    const params = mainClientId ? `?mainClientId=${mainClientId}` : '';
    return this.http.get<ApiResponse<CaseDto[]>>(`${this.baseUrl}/my-cases${params}`)
      .pipe(map(r => r.data));
  }

  getById(id: number): Observable<CaseDto> {
    return this.http.get<ApiResponse<CaseDto>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  create(request: CreateCaseRequest): Observable<CaseDto> {
    return this.http.post<ApiResponse<any>>(this.baseUrl, request)
      .pipe(map(r => {
        const d = r.data;
        return { ...d, id: d.caseId ?? d.id ?? 0 };
      }));
  }

  update(id: number, request: UpdateCaseRequest): Observable<CaseDto> {
    return this.http.put<ApiResponse<CaseDto>>(`${this.baseUrl}/${id}`, request)
      .pipe(map(r => r.data));
  }

  /** Partially update a case — only the fields provided are updated, others remain unchanged */
  patch(id: number, fields: Record<string, any>): Observable<CaseDto> {
    return this.http.patch<ApiResponse<CaseDto>>(`${this.baseUrl}/${id}`, fields)
      .pipe(map(r => r.data));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  /** Generate a report for cases between dates */
  exportReport(request: CaseSearchRequest): Observable<Blob> {
    // Map CaseSearchRequest fields to what the report generation API expects
    const today = new Date();
    const thirtyDaysAgo = new Date(today.getTime() - 30 * 24 * 60 * 60 * 1000);
    const formatDate = (d: Date) => d.toISOString().split('T')[0];
    const reportRequest = {
      dateFrom: request.dateCreatedFrom || formatDate(thirtyDaysAgo),
      dateTo: request.dateCreatedTo || formatDate(today),
      statusId: request.statusId || null,
      mainClientId: request.mainClientId || null,
      format: 'excel'
    };
    return this.http.post(`${this.baseUrl.replace('/cases', '')}/report/cases-between-dates`, reportRequest, {
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
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/${caseId}/facility-types`)
      .pipe(map(r => (r.data || []).map((item: any) => this.mapFacilityType(item))));
  }

  createFacilityType(caseId: number, request: CreateCaseFacilityTypeRequest): Observable<CaseFacilityTypeDto> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/${caseId}/facility-types`, request)
      .pipe(map(r => this.mapFacilityType(r.data)));
  }

  updateFacilityType(caseId: number, id: number, request: CreateCaseFacilityTypeRequest): Observable<CaseFacilityTypeDto> {
    return this.http.put<ApiResponse<any>>(`${this.baseUrl}/${caseId}/facility-types/${id}`, request)
      .pipe(map(r => this.mapFacilityType(r.data)));
  }

  deleteFacilityType(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/facility-types/${id}`);
  }

  private mapFacilityType(item: any): CaseFacilityTypeDto {
    return {
      id: item.caseIdFacilityTypeId ?? item.id ?? 0,
      caseId: item.caseId,
      facilityTypeId: item.facilityTypeId,
      facilityTypeCode: item.facilityTypeCode,
      facilityTypeName: item.facilityTypeName,
      dateAdmitted: item.dateAdmitted,
      dateDischarged: item.dateDischarged,
      los: item.los,
      minutesOnVentilator: item.minutesOnVentilator,
      comments: item.comments
    };
  }

  // ─── Notes ──────────────────────────────────────────────────

  getNotes(caseId: number): Observable<CaseNoteDto[]> {
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/${caseId}/notes`)
      .pipe(map(r => (r.data || []).map((item: any) => this.mapNote(item))));
  }

  createNote(caseId: number, request: CreateCaseNoteRequest): Observable<CaseNoteDto> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/${caseId}/notes`, request)
      .pipe(map(r => this.mapNote(r.data)));
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
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/${caseId}/comments`)
      .pipe(map(r => (r.data || []).map((item: any) => ({
        id: item.caseCommentId ?? item.id ?? 0,
        caseId: item.caseId,
        text: item.comment || item.text || '',
        dateCreated: item.dateCreated,
        createdBy: item.userName || item.userID || item.userId || item.createdBy || ''
      }))));
  }

  createComment(caseId: number, request: CreateCaseCommentRequest): Observable<CaseCommentDto> {
    // Backend expects 'comment' not 'text'
    const body = { comment: request.text };
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/${caseId}/comments`, body)
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
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/${caseId}/exclusions`)
      .pipe(map(r => (r.data || []).map((item: any) => ({
        id: item.exclusionId ?? item.id ?? 0,
        caseId: item.caseId,
        exclusionId: item.exclusionId,
        exclusionName: item.exclusionName,
        comment: item.comment
      }))));
  }

  createExclusion(caseId: number, request: CreateCaseExclusionRequest): Observable<CaseExclusionDto> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/${caseId}/exclusions`, request)
      .pipe(map(r => r.data));
  }

  updateExclusion(caseId: number, id: number, request: { comment?: string }): Observable<CaseExclusionDto> {
    return this.http.put<ApiResponse<any>>(`${this.baseUrl}/${caseId}/exclusions/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteExclusion(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/exclusions/${id}`);
  }

  // ─── Checklist ──────────────────────────────────────────────

  getChecklist(caseId: number): Observable<CaseChecklistDto[]> {
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/${caseId}/checklist`)
      .pipe(map(r => (r.data || []).map((item: any) => ({
        id: item.checklistTemplateId ?? item.id ?? 0,
        caseId: item.caseId,
        templateId: item.checklistTemplateId,
        templateName: item.templateName,
        checked: item.checked ?? false,
        notApplicable: item.notApplicable ?? false,
        comments: item.comments || '',
        date: item.date
      }))));
  }

  createChecklistItem(caseId: number, request: CreateCaseChecklistRequest): Observable<CaseChecklistDto> {
    const body = { checklistTemplateId: request.templateId, ...request };
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/${caseId}/checklist`, body)
      .pipe(map(r => r.data));
  }

  updateChecklistItem(caseId: number, id: number, request: CreateCaseChecklistRequest): Observable<CaseChecklistDto> {
    return this.http.put<ApiResponse<any>>(`${this.baseUrl}/${caseId}/checklist/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteChecklistItem(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/checklist/${id}`);
  }

  // ─── NAPPI ──────────────────────────────────────────────────

  getNappi(caseId: number): Observable<CaseNappiDto[]> {
    return this.http.get<ApiResponse<any[]>>(`${this.baseUrl}/${caseId}/nappi`)
      .pipe(map(r => (r.data || []).map((item: any) => this.mapNappi(item))));
  }

  createNappi(caseId: number, request: CreateCaseNappiRequest): Observable<CaseNappiDto> {
    return this.http.post<ApiResponse<any>>(`${this.baseUrl}/${caseId}/nappi`, request)
      .pipe(map(r => this.mapNappi(r.data)));
  }

  updateNappi(caseId: number, id: number, request: CreateCaseNappiRequest): Observable<CaseNappiDto> {
    return this.http.put<ApiResponse<any>>(`${this.baseUrl}/${caseId}/nappi/${id}`, request)
      .pipe(map(r => this.mapNappi(r.data)));
  }

  deleteNappi(caseId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${caseId}/nappi/${id}`);
  }

  private mapNappi(item: any): CaseNappiDto {
    return {
      id: item.caseIdNappiId ?? item.id ?? 0,
      caseId: item.caseId,
      nappiId: item.nappiId,
      nappiCode: item.nappiCode,
      nappiDescription: item.nappiDescription,
      price1: item.price1,
      value: item.value,
      quantity: item.quantity,
      measure: item.measure,
      units: item.units,
      dispensary: item.dispensary,
      ward: item.ward,
      theater: item.theater,
      tto: item.tto,
      _0201: item._0201,
      date: item.date
    };
  }

  private mapNote(item: any): CaseNoteDto {
    return {
      id: item.caseNoteId ?? item.id ?? 0,
      caseId: item.caseId,
      note: item.note,
      interimAmount: item.interimAmount,
      interimHospital: item.interimHospital,
      interimRadiology: item.interimRadiology,
      interimDialysis: item.interimDialysis,
      interimSpecialist: item.interimSpecialist,
      interimPhysio: item.interimPhysio,
      interimTransport: item.interimTransport,
      interimAccomodation: item.interimAccomodation,
      interimScript: item.interimScript,
      dateCreated: item.dateCreated,
      createdBy: item.createdBy || item.userId
    };
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
