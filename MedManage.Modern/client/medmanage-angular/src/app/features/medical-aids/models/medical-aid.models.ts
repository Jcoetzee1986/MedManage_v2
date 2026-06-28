export interface MedicalAidDto {
  medicalAidId: number;
  mainClientId: number | null;
  medicalAidName: string | null;
  medicalAidInitiationDate: string | null;
  medicalAidReinstatedDate: string | null;
  medicalAidTerminatedDate: string | null;
  casePrefix: string | null;
  reportTemplate: string | null;
  dateCreated: string;
  dateModified: string | null;
  dateDeleted: string | null;
}

export interface CreateMedicalAidRequest {
  mainClientId: number | null;
  medicalAidName: string | null;
  medicalAidInitiationDate: string | null;
  medicalAidReinstatedDate: string | null;
  medicalAidTerminatedDate: string | null;
  casePrefix: string | null;
  reportTemplate: string | null;
}

export interface UpdateMedicalAidRequest {
  medicalAidId: number;
  mainClientId: number | null;
  medicalAidName: string | null;
  medicalAidInitiationDate: string | null;
  medicalAidReinstatedDate: string | null;
  medicalAidTerminatedDate: string | null;
  casePrefix: string | null;
  reportTemplate: string | null;
}

export interface MedicalAidProductDto {
  medAidProductId: number;
  mainClientId: number | null;
  medAidProductName: string | null;
  allowServices: boolean | null;
}

export interface CreateMedicalAidProductRequest {
  medAidProductName: string | null;
  allowServices: boolean | null;
}

export interface UpdateMedicalAidProductRequest {
  medAidProductId: number;
  medAidProductName: string | null;
  allowServices: boolean | null;
}

export interface MedicalAidExclusionDto {
  medicalAidId: number;
  baseTariffId: string;
  baseTariffDescription: string | null;
}

export interface CreateMedicalAidExclusionRequest {
  baseTariffId: string;
}

export interface MedicalAidTariffDto {
  medicalAidId: number;
  tariffNameId: number;
  tariffName: string | null;
}

export interface CreateMedicalAidTariffRequest {
  tariffNameId: number;
}

export interface MedicalAidSearchFilters {
  medicalAidName?: string;
  activeOnly?: boolean;
  includeDeleted?: boolean;
}
