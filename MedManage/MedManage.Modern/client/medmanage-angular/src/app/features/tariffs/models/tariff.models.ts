export interface BaseTariffDto {
  id: number;
  code: string;
  description: string;
  category: string | null;
  isActive: boolean;
  dateCreated: string | null;
}

export interface CreateBaseTariffRequest {
  code: string;
  description: string;
  category?: string | null;
  isActive?: boolean;
}

export interface UpdateBaseTariffRequest extends CreateBaseTariffRequest {}

export interface TariffRateDto {
  id: number;
  tariffId: number;
  tariffCode: string;
  value: number;
  dateFrom: string;
  dateTo: string | null;
  rateType: string | null;
}

export interface CreateTariffRateRequest {
  tariffId: number;
  value: number;
  dateFrom: string;
  dateTo?: string | null;
  rateType?: string | null;
}

export interface UpdateTariffRateRequest extends CreateTariffRateRequest {}

export interface TariffNameDto {
  id: number;
  tariffId: number;
  tariffCode: string;
  name: string;
  language: string | null;
}

export interface CreateTariffNameRequest {
  tariffId: number;
  name: string;
  language?: string | null;
}

export interface UpdateTariffNameRequest extends CreateTariffNameRequest {}

export interface TariffLookupResult {
  id: number;
  code: string;
  description: string;
  category: string | null;
  currentRate: number | null;
}

export interface TariffCalculationResult {
  caseId: number;
  totalTariff: number;
  items: TariffCalculationItem[];
}

export interface TariffCalculationItem {
  tariffCode: string;
  description: string;
  value: number;
  quantity: number;
  total: number;
}

export interface ProviderTariffAssignmentDto {
  id: number;
  serviceProviderId: number;
  tariffId: number;
  tariffCode: string;
  tariffDescription: string;
  dateFrom: string | null;
  dateTo: string | null;
}

export interface CreateProviderTariffAssignmentRequest {
  tariffId: number;
  dateFrom?: string | null;
  dateTo?: string | null;
}

export interface ProviderCustomTariffDto {
  id: number;
  serviceProviderId: number;
  tariffId: number;
  tariffCode: string;
  tariffDescription: string;
  customValue: number;
  dateFrom: string | null;
  dateTo: string | null;
}

export interface CreateProviderCustomTariffRequest {
  tariffId: number;
  customValue: number;
  dateFrom?: string | null;
  dateTo?: string | null;
}
