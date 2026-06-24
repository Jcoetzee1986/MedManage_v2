export interface MemberDto {
  id: number;
  memberNumber: string;
  firstName: string;
  lastName: string;
  dateOfBirth: string | null;
  genderId: number | null;
  genderName: string | null;
  idNumber: string | null;
  contactNumber: string | null;
  email: string | null;
  address: string | null;
  medicalAidId: number | null;
  medicalAidName: string | null;
  medicalAidProductId: number | null;
  medicalAidProductName: string | null;
  memberStatusId: number | null;
  memberStatusName: string | null;
  allowServices: boolean;
  dateCreated: string | null;
}

export interface CreateMemberRequest {
  memberNumber: string;
  firstName: string;
  lastName: string;
  dateOfBirth?: string | null;
  genderId?: number | null;
  idNumber?: string | null;
  contactNumber?: string | null;
  email?: string | null;
  address?: string | null;
  medicalAidId?: number | null;
  medicalAidProductId?: number | null;
  memberStatusId?: number | null;
  allowServices?: boolean;
}

export interface UpdateMemberRequest extends CreateMemberRequest {}

export interface MemberSearchRequest {
  memberNumber?: string;
  firstName?: string;
  lastName?: string;
  idNumber?: string;
  medicalAidId?: number;
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
  createdBy: string | null;
  dateCreated: string | null;
}

export interface CreateMemberNoteRequest {
  note: string;
}

export interface MemberMedicalAidProductDto {
  id: number;
  memberId: number;
  medicalAidProductId: number;
  medicalAidProductName: string;
  medicalAidName: string;
  dateFrom: string | null;
  dateTo: string | null;
}

export interface CreateMemberMedicalAidProductRequest {
  medicalAidProductId: number;
  dateFrom?: string | null;
  dateTo?: string | null;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}
