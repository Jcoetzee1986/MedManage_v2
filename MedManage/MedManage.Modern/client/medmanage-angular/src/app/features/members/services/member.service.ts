import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  MemberDto, CreateMemberRequest, UpdateMemberRequest, MemberSearchRequest, PagedResult,
  MemberChronicIllnessDto, CreateMemberChronicIllnessRequest,
  MemberNoteDto, CreateMemberNoteRequest,
  MemberMedicalAidProductDto, CreateMemberMedicalAidProductRequest
} from '../models/member.models';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/members`;

  // ─── Member CRUD ─────────────────────────────────────────────

  search(request: MemberSearchRequest): Observable<PagedResult<MemberDto>> {
    return this.http.post<PagedResult<MemberDto>>(`${this.baseUrl}/search`, request);
  }

  getAll(): Observable<MemberDto[]> {
    return this.http.get<MemberDto[]>(this.baseUrl);
  }

  getById(id: number): Observable<MemberDto> {
    return this.http.get<MemberDto>(`${this.baseUrl}/${id}`);
  }

  create(request: CreateMemberRequest): Observable<MemberDto> {
    return this.http.post<MemberDto>(this.baseUrl, request);
  }

  update(id: number, request: UpdateMemberRequest): Observable<MemberDto> {
    return this.http.put<MemberDto>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  // ─── Chronic Illness ─────────────────────────────────────────

  getChronicIllnesses(memberId: number): Observable<MemberChronicIllnessDto[]> {
    return this.http.get<MemberChronicIllnessDto[]>(`${this.baseUrl}/${memberId}/chronic-illness`);
  }

  createChronicIllness(memberId: number, request: CreateMemberChronicIllnessRequest): Observable<MemberChronicIllnessDto> {
    return this.http.post<MemberChronicIllnessDto>(`${this.baseUrl}/${memberId}/chronic-illness`, request);
  }

  updateChronicIllness(memberId: number, id: number, request: CreateMemberChronicIllnessRequest): Observable<MemberChronicIllnessDto> {
    return this.http.put<MemberChronicIllnessDto>(`${this.baseUrl}/${memberId}/chronic-illness/${id}`, request);
  }

  deleteChronicIllness(memberId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${memberId}/chronic-illness/${id}`);
  }

  // ─── Medical Aid Products ────────────────────────────────────

  getMedicalAidProducts(memberId: number): Observable<MemberMedicalAidProductDto[]> {
    return this.http.get<MemberMedicalAidProductDto[]>(`${this.baseUrl}/${memberId}/medical-aid-products`);
  }

  createMedicalAidProduct(memberId: number, request: CreateMemberMedicalAidProductRequest): Observable<MemberMedicalAidProductDto> {
    return this.http.post<MemberMedicalAidProductDto>(`${this.baseUrl}/${memberId}/medical-aid-products`, request);
  }

  updateMedicalAidProduct(memberId: number, id: number, request: CreateMemberMedicalAidProductRequest): Observable<MemberMedicalAidProductDto> {
    return this.http.put<MemberMedicalAidProductDto>(`${this.baseUrl}/${memberId}/medical-aid-products/${id}`, request);
  }

  deleteMedicalAidProduct(memberId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${memberId}/medical-aid-products/${id}`);
  }

  // ─── Notes ───────────────────────────────────────────────────

  getNotes(memberId: number): Observable<MemberNoteDto[]> {
    return this.http.get<MemberNoteDto[]>(`${this.baseUrl}/${memberId}/notes`);
  }

  createNote(memberId: number, request: CreateMemberNoteRequest): Observable<MemberNoteDto> {
    return this.http.post<MemberNoteDto>(`${this.baseUrl}/${memberId}/notes`, request);
  }

  updateNote(memberId: number, id: number, request: CreateMemberNoteRequest): Observable<MemberNoteDto> {
    return this.http.put<MemberNoteDto>(`${this.baseUrl}/${memberId}/notes/${id}`, request);
  }

  deleteNote(memberId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${memberId}/notes/${id}`);
  }
}
