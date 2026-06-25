/**
 * Case management domain models.
 * These correspond to the API DTOs from the .NET backend.
 */

export interface CaseDto {
  id: number;
  caseId?: number;
  caseNumber?: string;
  authNumber?: string;
  accountNr?: string;
  caseStatusId?: number;
  caseStatusName?: string;
  caseStatus?: string;
  statusId?: number;
  caseTypeId?: number;
  caseTypeName?: string;
  authTypeId?: number;
  caseCategoryId?: number;
  caseCategoryName?: string;
  memberId?: number;
  memberName?: string;
  memberNumber?: string;
  memberSurname?: string;
  memberIdNumber?: string;
  memberDateOfBirth?: string;
  memberMedicalAidName?: string;
  memberProductName?: string;
  memberStatusName?: string;
  surname?: string;
  name?: string;
  initials?: string;
  idNumber?: string;
  dateOfBirth?: string;
  referToId?: number;
  referToName?: string;
  referTo?: string;
  referToPracticeName?: string;
  referToPersonSurname?: string;
  referToPersonName?: string;
  referToSpeciality?: string;
  referToContact?: string;
  referFromId?: number;
  referFromName?: string;
  referFrom?: string;
  referFromPracticeName?: string;
  referFromPersonSurname?: string;
  referFromPersonName?: string;
  referFromSpeciality?: string;
  referFromContact?: string;
  practiceId?: number;
  practiceName?: string;
  mainClientId?: number;
  mainClientName?: string;
  medicalAidId?: number;
  medicalAidName?: string;
  medAidProductName?: string;
  productId?: number;
  productName?: string;
  dateOfService?: string;
  dateAdmitted?: string;
  admissionDate?: string;
  admissionTime?: string;
  dateDischarged?: string;
  dischargeDate?: string;
  dischargeTime?: string;
  dateCreated?: string;
  dateUpdated?: string;
  createdBy?: string;
  userID?: string;
  diagnosis?: string;
  comments?: string;
  caseDescription?: string;
  description?: string;
  isBooking?: boolean;
  hasBooking?: boolean;
  wasBooking?: boolean;
  wcaIod?: boolean;
  primaryICD?: string;
  primaryCPT?: string;
  changeToCaseDate?: string;
  interimAmount?: number;
  finalInvoiceAmount?: number;
  penaltyPercentage?: number;
  finalInvoiceWithPenalty?: number;
  totalLos?: number;
  totalLengthOfStay?: number;
}

export interface CreateCaseRequest {
  authNumber?: string;
  accountNr?: string;
  caseStatusId?: number;
  caseTypeId?: number;
  caseCategoryId?: number;
  memberId?: number;
  referToId?: number;
  referFromId?: number;
  practiceId?: number;
  mainClientId?: number;
  medicalAidId?: number;
  productId?: number;
  dateOfService?: string;
  dateAdmitted?: string;
  admissionTime?: string;
  dateDischarged?: string;
  dischargeTime?: string;
  diagnosis?: string;
  description?: string;
  comments?: string;
  isBooking?: boolean;
  wasBooking?: boolean;
  wcaIod?: boolean;
  interimAmount?: number;
  finalInvoiceAmount?: number;
  penaltyPercentage?: number;
}

export interface DuplicateCheckResult {
  isDuplicate: boolean;
  existingCaseId?: number;
  existingCaseNumber?: string;
  message?: string;
}

export interface UpdateCaseRequest extends CreateCaseRequest {
  id: number;
}

export interface CaseSearchRequest {
  authNumber?: string;
  memberNumber?: string;
  memberSurname?: string;
  memberName?: string;
  statusId?: number;
  caseCategoryId?: number;
  authTypeId?: number;
  referToId?: number;
  referFromId?: number;
  practiceName?: string;
  admissionDateFrom?: string;
  admissionDateTo?: string;
  dischargeDateFrom?: string;
  dischargeDateTo?: string;
  dateCreatedFrom?: string;
  dateCreatedTo?: string;
  icdCode?: string;
  cptCode?: string;
  medicalAidId?: number;
  pageNumber?: number;
  pageSize?: number;
  sortBy?: string;
  sortDescending?: boolean;
}

/** Wrapper for API responses that use the ApiResponse<T> envelope */
export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

/** CPT Code */
export interface CaseCptDto {
  id: number;
  caseId: number;
  cptId?: number;
  cptCode?: string;
  cptDescription?: string;
  dateOfProcedure?: string;
  primaryCode?: boolean;
  secondaryCode?: boolean;
}

export interface CreateCaseCptRequest {
  cptId?: number;
  dateOfProcedure?: string;
  primaryCode?: boolean;
  secondaryCode?: boolean;
}

/** ICD Code */
export interface CaseIcdDto {
  id: number;
  caseId: number;
  icdId?: number;
  icdCode?: string;
  icdDescription?: string;
  dateOfProcedure?: string;
  primaryCode?: boolean;
  secondaryCode?: boolean;
  coMorbidityCode?: boolean;
}

export interface CreateCaseIcdRequest {
  icdId?: number;
  dateOfProcedure?: string;
  primaryCode?: boolean;
  secondaryCode?: boolean;
  coMorbidityCode?: boolean;
}

/** Tariff */
export interface CaseTariffDto {
  id: number;
  caseId: number;
  tariffId?: number;
  tariffCode?: string;
  tariffDescription?: string;
  value?: number;
  qty?: number;
  agreedRateOverride?: number;
  valueIsTotal?: boolean;
  rejected?: boolean;
  dateOfProcedure?: string;
}

export interface CreateCaseTariffRequest {
  tariffId?: number;
  value?: number;
  qty?: number;
  agreedRateOverride?: number;
  valueIsTotal?: boolean;
  rejected?: boolean;
  dateOfProcedure?: string;
}

/** Facility Type */
export interface CaseFacilityTypeDto {
  id: number;
  caseId: number;
  facilityTypeId?: number;
  facilityTypeCode?: string;
  facilityTypeName?: string;
  dateAdmitted?: string;
  dateDischarged?: string;
  los?: number;
  minutesOnVentilator?: number;
  comments?: string;
}

export interface CreateCaseFacilityTypeRequest {
  facilityTypeId?: number;
  dateAdmitted?: string;
  dateDischarged?: string;
  los?: number;
  minutesOnVentilator?: number;
  comments?: string;
}

/** Notes */
export interface CaseNoteDto {
  id: number;
  caseId: number;
  note?: string;
  interimAmount1?: number;
  interimAmount2?: number;
  interimAmount3?: number;
  interimAmount4?: number;
  interimAmount5?: number;
  interimAmount6?: number;
  interimAmount7?: number;
  interimAmount8?: number;
  dateCreated?: string;
  createdBy?: string;
}

export interface CreateCaseNoteRequest {
  note?: string;
  interimAmount1?: number;
  interimAmount2?: number;
  interimAmount3?: number;
  interimAmount4?: number;
  interimAmount5?: number;
  interimAmount6?: number;
  interimAmount7?: number;
  interimAmount8?: number;
}

/** Comments */
export interface CaseCommentDto {
  id: number;
  caseId: number;
  text?: string;
  dateCreated?: string;
  createdBy?: string;
}

export interface CreateCaseCommentRequest {
  text?: string;
}

/** Exclusions */
export interface CaseExclusionDto {
  id: number;
  caseId: number;
  exclusionId?: number;
  exclusionName?: string;
  comment?: string;
}

export interface CreateCaseExclusionRequest {
  exclusionId?: number;
  comment?: string;
}

/** Checklist */
export interface CaseChecklistDto {
  id: number;
  caseId: number;
  templateId?: number;
  templateName?: string;
  checked?: boolean;
  notApplicable?: boolean;
  comments?: string;
  date?: string;
}

export interface CreateCaseChecklistRequest {
  templateId?: number;
  checked?: boolean;
  notApplicable?: boolean;
  comments?: string;
  date?: string;
}

/** NAPPI */
export interface CaseNappiDto {
  id: number;
  caseId: number;
  nappiId?: number;
  nappiCode?: string;
  nappiDescription?: string;
  value?: number;
  quantity?: number;
  dispensary?: boolean;
  ward?: boolean;
  theater?: boolean;
  tto?: boolean;
  date?: string;
}

export interface CreateCaseNappiRequest {
  nappiId?: number;
  value?: number;
  quantity?: number;
  dispensary?: boolean;
  ward?: boolean;
  theater?: boolean;
  tto?: boolean;
  date?: string;
}

/** Linked Files */
export interface CaseFileDto {
  id: number;
  caseId: number;
  fileName?: string;
  fileType?: string;
  fileSize?: number;
  dateUploaded?: string;
  uploadedBy?: string;
}

/** Linked Cases */
export interface CaseLinkDto {
  id: number;
  parentCaseId: number;
  childCaseId: number;
  linkedCaseNumber?: string;
  linkedCaseMember?: string;
}

export interface CreateCaseLinkRequest {
  childCaseId: number;
}

/** Case Copy */
export interface CaseCopyRequest {
  includeNotes?: boolean;
  includeComments?: boolean;
  includeCpt?: boolean;
  includeIcd?: boolean;
  includeTariffs?: boolean;
  includeFacilityTypes?: boolean;
  includeExclusions?: boolean;
  includeNappi?: boolean;
  includeChecklist?: boolean;
}
