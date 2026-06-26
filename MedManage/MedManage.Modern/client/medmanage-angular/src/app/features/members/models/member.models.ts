export interface MemberDto {
  id: number;
  memberNumber: string;
  firstName: string;
  lastName: string;
  initials: string | null;
  dateOfBirth: string | null;
  genderId: number | null;
  genderName: string | null;
  titleId: number | null;
  titleName: string | null;
  idNumber: string | null;
  marritalStatusId: number | null;
  marritalStatusName: string | null;
  languageId: number | null;
  languageName: string | null;
  raceId: number | null;
  raceName: string | null;
  isPensioner: boolean;
  isMbodRma: boolean;

  // Medical Aid
  medicalAidId: number | null;
  medicalAidName: string | null;
  medicalAidProductId: number | null;
  medicalAidProductName: string | null;
  memberStatusId: number | null;
  memberStatusName: string | null;
  dateOfBenefit: string | null;
  dateJoined: string | null;
  isMedAidExhausted: boolean;
  dateExhausted: string | null;
  isWaitingPeriod: boolean;
  isReinstated: boolean;
  dateReinstated: string | null;
  isDeceased: boolean;
  dateDeceased: string | null;

  // Logistics
  countryId: number | null;
  countryName: string | null;
  passportNumber: string | null;
  passportExpiryDate: string | null;
  periodInCountryId: number | null;
  periodInCountryName: string | null;
  addressLine1: string | null;
  addressLine2: string | null;
  addressLine3: string | null;
  addressCode: string | null;
  phoneNumber: string | null;
  cellNumber: string | null;

  // Other (Next of Kin & Employer)
  dependents: string | null;
  nextOfKin: string | null;
  relationship: string | null;
  contactNumber: string | null;
  employerCountryId: number | null;
  employerCountryName: string | null;
  employerAddress: string | null;
  employerAddressCode: string | null;
  employerPhoneNumber: string | null;

  // Suspension
  isSuspended: boolean;
  dateSuspended: string | null;
  suspendReasonId: number | null;
  suspendReasonName: string | null;

  // System
  allowServices: boolean;
  dateCreated: string | null;

  // Legacy compat
  email: string | null;
  address: string | null;
}

export interface CreateMemberRequest {
  memberNumber: string;
  firstName: string;
  lastName: string;
  initials?: string | null;
  dateOfBirth?: string | null;
  genderId?: number | null;
  titleId?: number | null;
  idNumber?: string | null;
  marritalStatusId?: number | null;
  languageId?: number | null;
  raceId?: number | null;
  isPensioner?: boolean;
  isMbodRma?: boolean;

  // Medical Aid
  medicalAidId?: number | null;
  medicalAidProductId?: number | null;
  memberStatusId?: number | null;
  dateOfBenefit?: string | null;
  dateJoined?: string | null;
  isMedAidExhausted?: boolean;
  dateExhausted?: string | null;
  isWaitingPeriod?: boolean;
  isReinstated?: boolean;
  dateReinstated?: string | null;
  isDeceased?: boolean;
  dateDeceased?: string | null;

  // Logistics
  countryId?: number | null;
  passportNumber?: string | null;
  passportExpiryDate?: string | null;
  periodInCountryId?: number | null;
  addressLine1?: string | null;
  addressLine2?: string | null;
  addressLine3?: string | null;
  addressCode?: string | null;
  phoneNumber?: string | null;
  cellNumber?: string | null;

  // Other
  dependents?: string | null;
  nextOfKin?: string | null;
  relationship?: string | null;
  contactNumber?: string | null;
  employerCountryId?: number | null;
  employerAddress?: string | null;
  employerAddressCode?: string | null;
  employerPhoneNumber?: string | null;

  // Suspension
  isSuspended?: boolean;
  dateSuspended?: string | null;
  suspendReasonId?: number | null;

  allowServices?: boolean;
  email?: string | null;
  address?: string | null;
}

export interface UpdateMemberRequest extends CreateMemberRequest {}

export interface MemberSearchRequest {
  memberNumber?: string;
  firstName?: string;
  lastName?: string;
  idNumber?: string;
  passportNumber?: string;
  dateOfBirth?: string;
  medicalAidId?: number;
  mainClientId?: number;
  memberStatusId?: number;
  pageNumber?: number;
  pageSize?: number;
  sortField?: string;
  sortDirection?: 'asc' | 'desc';
}

export interface MemberChronicIllnessDto {
  id: number;
  memberId: number;
  chronicIllnessId: number;
  chronicIllnessName: string;
  dateFrom: string | null;
  dateTo: string | null;
  comments: string | null;
}

export interface CreateMemberChronicIllnessRequest {
  chronicIllnessId: number;
  dateFrom?: string | null;
  dateTo?: string | null;
  comments?: string | null;
}

export interface MemberNoteDto {
  id: number;
  memberId: number;
  note: string;
  noteDate: string | null;
  createdBy: string | null;
  dateCreated: string | null;
}

export interface CreateMemberNoteRequest {
  note: string;
  noteDate?: string | null;
}

export interface MemberMedicalAidProductDto {
  id: number;
  medAidProductIdMemberId: number;
  memberId: number;
  medAidProductId: number;
  medicalAidProductId: number;
  medicalAidProductName: string;
  medicalAidName: string;
  startDate: string | null;
  endDate: string | null;
  dateFrom: string | null;
  dateTo: string | null;
}

export interface CreateMemberMedicalAidProductRequest {
  medAidProductId: number;
  startDate: string;
  endDate?: string | null;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}
