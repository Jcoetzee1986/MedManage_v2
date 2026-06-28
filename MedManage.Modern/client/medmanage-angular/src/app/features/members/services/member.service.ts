import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment';
import {
  MemberDto, CreateMemberRequest, UpdateMemberRequest, MemberSearchRequest, PagedResult,
  MemberChronicIllnessDto, CreateMemberChronicIllnessRequest,
  MemberNoteDto, CreateMemberNoteRequest,
  MemberMedicalAidProductDto, CreateMemberMedicalAidProductRequest
} from '../models/member.models';

interface ApiResponse<T> { success: boolean; data: T; message?: string; }

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/members`;

  // ─── Member CRUD ─────────────────────────────────────────────

  search(request: MemberSearchRequest): Observable<PagedResult<MemberDto>> {
    return this.http.post<ApiResponse<PagedResult<any>>>(`${this.baseUrl}/search`, request)
      .pipe(map(r => ({
        ...r.data,
        items: (r.data.items || []).map((item: any) => this.mapMemberResponse(item))
      })));
  }

  getAll(): Observable<MemberDto[]> {
    return this.http.get<ApiResponse<MemberDto[]>>(this.baseUrl)
      .pipe(map(r => r.data));
  }

  getById(id: number): Observable<MemberDto> {
    return this.http.get<ApiResponse<any>>(`${this.baseUrl}/${id}`)
      .pipe(map(r => this.mapMemberResponse(r.data)));
  }

  create(request: CreateMemberRequest): Observable<MemberDto> {
    return this.http.post<ApiResponse<any>>(this.baseUrl, request)
      .pipe(map(r => this.mapMemberResponse(r.data)));
  }

  update(id: number, request: UpdateMemberRequest): Observable<MemberDto> {
    // Map frontend field names to backend field names
    const body: any = {
      memberId: (request as any).memberId ?? id,
      memberNumber: request.memberNumber,
      surname: request.lastName,
      name: request.firstName,
      initials: request.initials,
      dateOfBirth: request.dateOfBirth,
      genderId: request.genderId,
      titleId: request.titleId,
      idnumber: request.idNumber,
      marritalStatusId: request.marritalStatusId,
      memberLanguageId: request.languageId,
      memberRaceId: request.raceId,
      pensioner: request.isPensioner,
      mbodRma: request.isMbodRma,
      medicalAidId: request.medicalAidId,
      memberStatusId: request.memberStatusId,
      dateOfBenefit: request.dateOfBenefit,
      dateJoined: request.dateJoined,
      medicalAidExhausted: request.isMedAidExhausted,
      dateMedicalAidExhausted: request.dateExhausted,
      waitingPeriodApplicable: request.isWaitingPeriod,
      fundReinstated: request.isReinstated,
      fundReinstatedDate: request.dateReinstated,
      deceased: request.isDeceased,
      deceasedDate: request.dateDeceased,
      memberCountryId: request.countryId,
      passportNumber: request.passportNumber,
      passportExpiryDate: request.passportExpiryDate,
      periodInCountryId: request.periodInCountryId,
      memberAddress1: request.addressLine1,
      memberAddress2: request.addressLine2,
      memberAddress3: request.addressLine3,
      memberAddressCode: request.addressCode,
      memberPhoneNumber: request.phoneNumber,
      memberCellNumber: request.cellNumber,
      memberDependents: request.dependents,
      nextOfKin: request.nextOfKin,
      nextOfKinRelationship: request.relationship,
      nextOfKinContactNumber: request.contactNumber,
      employerCountryId: request.employerCountryId,
      employerAddress: request.employerAddress,
      employerAddressCode: request.employerAddressCode,
      employerPhoneNumber: request.employerPhoneNumber,
      suspended: request.isSuspended,
      dateSuspended: request.dateSuspended,
      suspendedReasonId: request.suspendReasonId,
      allowServices: request.allowServices
    };
    return this.http.put<ApiResponse<any>>(`${this.baseUrl}/${id}`, body)
      .pipe(map(r => this.mapMemberResponse(r.data)));
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  checkDuplicateMemberNumber(memberNumber: string): Observable<boolean> {
    return this.http.get<ApiResponse<boolean>>(`${this.baseUrl}/check-duplicate?memberNumber=${encodeURIComponent(memberNumber)}`)
      .pipe(map(r => r.data));
  }

  // ─── Chronic Illness ─────────────────────────────────────────

  getChronicIllnesses(memberId: number): Observable<MemberChronicIllnessDto[]> {
    return this.http.get<ApiResponse<MemberChronicIllnessDto[]>>(`${this.baseUrl}/${memberId}/chronic-illness`)
      .pipe(map(r => r.data));
  }

  createChronicIllness(memberId: number, request: CreateMemberChronicIllnessRequest): Observable<MemberChronicIllnessDto> {
    return this.http.post<ApiResponse<MemberChronicIllnessDto>>(`${this.baseUrl}/${memberId}/chronic-illness`, request)
      .pipe(map(r => r.data));
  }

  updateChronicIllness(memberId: number, id: number, request: CreateMemberChronicIllnessRequest): Observable<MemberChronicIllnessDto> {
    return this.http.put<ApiResponse<MemberChronicIllnessDto>>(`${this.baseUrl}/${memberId}/chronic-illness/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteChronicIllness(memberId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${memberId}/chronic-illness/${id}`);
  }

  // ─── Medical Aid Products ────────────────────────────────────

  getMedicalAidProducts(memberId: number): Observable<MemberMedicalAidProductDto[]> {
    return this.http.get<ApiResponse<MemberMedicalAidProductDto[]>>(`${this.baseUrl}/${memberId}/medical-aid-products`)
      .pipe(map(r => r.data));
  }

  createMedicalAidProduct(memberId: number, request: CreateMemberMedicalAidProductRequest): Observable<MemberMedicalAidProductDto> {
    return this.http.post<ApiResponse<MemberMedicalAidProductDto>>(`${this.baseUrl}/${memberId}/medical-aid-products`, request)
      .pipe(map(r => r.data));
  }

  updateMedicalAidProduct(memberId: number, id: number, request: CreateMemberMedicalAidProductRequest): Observable<MemberMedicalAidProductDto> {
    return this.http.put<ApiResponse<MemberMedicalAidProductDto>>(`${this.baseUrl}/${memberId}/medical-aid-products/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteMedicalAidProduct(memberId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${memberId}/medical-aid-products/${id}`);
  }

  // ─── Notes ───────────────────────────────────────────────────

  getNotes(memberId: number): Observable<MemberNoteDto[]> {
    return this.http.get<ApiResponse<MemberNoteDto[]>>(`${this.baseUrl}/${memberId}/notes`)
      .pipe(map(r => r.data));
  }

  createNote(memberId: number, request: CreateMemberNoteRequest): Observable<MemberNoteDto> {
    return this.http.post<ApiResponse<MemberNoteDto>>(`${this.baseUrl}/${memberId}/notes`, request)
      .pipe(map(r => r.data));
  }

  updateNote(memberId: number, id: number, request: CreateMemberNoteRequest): Observable<MemberNoteDto> {
    return this.http.put<ApiResponse<MemberNoteDto>>(`${this.baseUrl}/${memberId}/notes/${id}`, request)
      .pipe(map(r => r.data));
  }

  deleteNote(memberId: number, id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${memberId}/notes/${id}`);
  }

  // ─── Response Mapping ────────────────────────────────────────
  // Backend returns C# property names (Surname, Name, Idnumber) 
  // Frontend model uses (lastName, firstName, idNumber)
  private mapMemberResponse(data: any): MemberDto {
    if (!data) return data;
    return {
      id: data.memberId ?? data.id,
      memberNumber: data.memberNumber,
      firstName: data.name ?? data.firstName,
      lastName: data.surname ?? data.lastName,
      initials: data.initials,
      dateOfBirth: data.dateOfBirth,
      genderId: data.genderId,
      genderName: data.genderName,
      titleId: data.titleId,
      titleName: data.titleName,
      idNumber: data.idnumber ?? data.idNumber,
      marritalStatusId: data.marritalStatusId,
      marritalStatusName: data.marritalStatusName,
      languageId: data.memberLanguageId ?? data.languageId,
      languageName: data.languageName,
      raceId: data.memberRaceId ?? data.raceId,
      raceName: data.raceName,
      isPensioner: data.pensioner ?? data.isPensioner ?? false,
      isMbodRma: data.mbodRma ?? data.isMbodRma ?? false,
      medicalAidId: data.medicalAidId,
      medicalAidName: data.medicalAidName,
      medicalAidProductId: data.medAidProductId ?? data.medicalAidProductId,
      medicalAidProductName: data.medicalAidProductName,
      memberStatusId: data.memberStatusId,
      memberStatusName: data.memberStatusName,
      dateOfBenefit: data.dateOfBenefit,
      dateJoined: data.dateJoined,
      isMedAidExhausted: data.medicalAidExhausted ?? data.isMedAidExhausted ?? false,
      dateExhausted: data.dateMedicalAidExhausted ?? data.dateExhausted,
      isWaitingPeriod: data.waitingPeriodApplicable ?? data.isWaitingPeriod ?? false,
      isReinstated: data.fundReinstated ?? data.isReinstated ?? false,
      dateReinstated: data.fundReinstatedDate ?? data.dateReinstated,
      isDeceased: data.deceased ?? data.isDeceased ?? false,
      dateDeceased: data.deceasedDate ?? data.dateDeceased,
      countryId: data.memberCountryId ?? data.countryId,
      countryName: data.countryName,
      passportNumber: data.passportNumber,
      passportExpiryDate: data.passportExpiryDate,
      periodInCountryId: data.periodInCountryId,
      periodInCountryName: data.periodInCountryName,
      addressLine1: data.memberAddress1 ?? data.addressLine1,
      addressLine2: data.memberAddress2 ?? data.addressLine2,
      addressLine3: data.memberAddress3 ?? data.addressLine3,
      addressCode: data.memberAddressCode ?? data.addressCode,
      phoneNumber: data.memberPhoneNumber ?? data.phoneNumber,
      cellNumber: data.memberCellNumber ?? data.cellNumber,
      dependents: data.memberDependents ?? data.dependents,
      nextOfKin: data.nextOfKinName ?? data.nextOfKin,
      relationship: data.nextOfKinRelationship ?? data.relationship,
      contactNumber: data.nextOfKinContactNumber ?? data.contactNumber,
      employerCountryId: data.employerCountryId,
      employerCountryName: data.employerCountryName,
      employerAddress: data.employerAddress,
      employerAddressCode: data.employerAddressCode,
      employerPhoneNumber: data.employerPhoneNumber,
      isSuspended: data.suspended ?? data.isSuspended ?? false,
      dateSuspended: data.dateSuspended,
      suspendReasonId: data.suspendedReasonId ?? data.suspendReasonId,
      suspendReasonName: data.suspendReasonName,
      allowServices: data.allowServices ?? true,
      dateCreated: data.dateInserted ?? data.dateCreated,
      email: data.email,
      address: data.address
    };
  }
}
