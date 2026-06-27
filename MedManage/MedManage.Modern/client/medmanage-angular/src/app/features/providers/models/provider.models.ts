export interface ProviderDto {
  id: number;
  providerNumber: string;
  practiceName: string;
  firstName: string;
  lastName: string;
  specialityId?: number | null;
  specialityName?: string | null;
  practiceGroupNumber?: string | null;
  numberOfPartners?: number | null;
  serviceArea?: string | null;
  isHospital?: boolean;
  contactNumber?: string | null;
  cellNumber?: string | null;
  fax?: string | null;
  email?: string | null;
  countryId?: number | null;
  countryName?: string | null;
  languageId?: number | null;
  languageName?: string | null;
  // Physical address
  address1?: string | null;
  address2?: string | null;
  address3?: string | null;
  address4?: string | null;
  addressCode?: string | null;
  // Postal address
  postalAddress1?: string | null;
  postalAddress2?: string | null;
  postalAddress3?: string | null;
  postalAddress4?: string | null;
  postalAddressCode?: string | null;
  // Banking
  bankName?: string | null;
  branchName?: string | null;
  branchCode?: string | null;
  accountType?: string | null;
  accountNumber?: string | null;
  // Legacy compat
  address?: string | null;
  bHFNumber?: string | null;
  hpcsaNumber?: string | null;
  isActive?: boolean;
  dateCreated?: string | null;
}

export interface CreateProviderRequest {
  providerNumber: string;
  practiceName: string;
  firstName: string;
  lastName: string;
  specialityId?: number | null;
  practiceGroupNumber?: string | null;
  numberOfPartners?: number | null;
  serviceArea?: string | null;
  isHospital?: boolean;
  contactNumber?: string | null;
  cellNumber?: string | null;
  fax?: string | null;
  email?: string | null;
  countryId?: number | null;
  languageId?: number | null;
  address1?: string | null;
  address2?: string | null;
  address3?: string | null;
  address4?: string | null;
  addressCode?: string | null;
  postalAddress1?: string | null;
  postalAddress2?: string | null;
  postalAddress3?: string | null;
  postalAddress4?: string | null;
  postalAddressCode?: string | null;
  bankName?: string | null;
  branchName?: string | null;
  branchCode?: string | null;
  accountType?: string | null;
  accountNumber?: string | null;
  address?: string | null;
  bHFNumber?: string | null;
  hpcsaNumber?: string | null;
  isActive?: boolean;
}

export interface UpdateProviderRequest extends CreateProviderRequest {}

export interface ProviderSearchRequest {
  serviceProviderName?: string;
  serviceProviderSurname?: string;
  practiceName?: string;
  practiceNr?: string;
  specialityId?: number;
  specialityName?: string;
  isHospital?: boolean;
  visible?: boolean;
  pageNumber?: number;
  pageSize?: number;
  sortField?: string;
  sortDirection?: 'asc' | 'desc';
}

export interface ProviderAutocompleteResult {
  id: number;
  providerNumber: string;
  practiceName: string;
  firstName: string;
  lastName: string;
  specialityName: string | null;
}

export interface ProviderTariffDto {
  id: number;
  serviceProviderId: number;
  tariffId: number;
  tariffName: string;
  tariffCode: string;
  dateFrom: string | null;
  dateTo: string | null;
}

export interface CreateProviderTariffRequest {
  tariffId: number;
  dateFrom?: string | null;
  dateTo?: string | null;
}

export interface ProviderCustomTariffDto {
  id: number;
  serviceProviderId: number;
  tariffId: number;
  tariffName: string;
  tariffCode: string;
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

export interface ProviderDiscountDto {
  id: number;
  serviceProviderId: number;
  mainClientId: number;
  mainClientName: string;
  discountPercentage: number;
  dateFrom: string | null;
  dateTo: string | null;
}

export interface CreateProviderDiscountRequest {
  mainClientId: number;
  discountPercentage: number;
  dateFrom?: string | null;
  dateTo?: string | null;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}
