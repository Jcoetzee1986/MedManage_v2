/**
 * Generic interface for reference data items.
 * All lookup tables share this common shape.
 */
export interface ReferenceDataItem {
  id: number;
  name: string;
  description?: string;
  isActive?: boolean;
}

/**
 * Request model for creating/updating reference data.
 */
export interface ReferenceDataRequest {
  name: string;
  description?: string;
  isActive?: boolean;
}

/**
 * Supported reference data resource types and their API endpoint names.
 */
export type ReferenceDataResource =
  | 'gender'
  | 'language'
  | 'race'
  | 'title'
  | 'marrital-status'
  | 'member-status'
  | 'period-in-country'
  | 'country'
  | 'speciality'
  | 'facility-type'
  | 'case-status'
  | 'case-type'
  | 'case-category'
  | 'checklist-template'
  | 'billing-status'
  | 'exclusion'
  | 'suspended-reason';

/**
 * Display configuration for each reference data resource.
 */
export interface ReferenceDataConfig {
  resource: ReferenceDataResource;
  displayName: string;
  description: string;
}

/**
 * All available reference data configurations for the admin UI.
 */
export const REFERENCE_DATA_CONFIGS: ReferenceDataConfig[] = [
  { resource: 'gender', displayName: 'Gender', description: 'Gender options for members' },
  { resource: 'language', displayName: 'Language', description: 'Languages spoken' },
  { resource: 'race', displayName: 'Race', description: 'Race classifications' },
  { resource: 'title', displayName: 'Title', description: 'Name titles (Mr, Mrs, Dr, etc.)' },
  { resource: 'marrital-status', displayName: 'Marital Status', description: 'Marital status options' },
  { resource: 'member-status', displayName: 'Member Status', description: 'Member enrollment statuses' },
  { resource: 'period-in-country', displayName: 'Period in Country', description: 'Duration categories' },
  { resource: 'country', displayName: 'Country', description: 'Countries' },
  { resource: 'speciality', displayName: 'Speciality', description: 'Medical specialities' },
  { resource: 'facility-type', displayName: 'Facility Type', description: 'Types of medical facilities' },
  { resource: 'case-status', displayName: 'Case Status', description: 'Case workflow statuses' },
  { resource: 'case-type', displayName: 'Case Type', description: 'Types of cases' },
  { resource: 'case-category', displayName: 'Case Category', description: 'Case categories' },
  { resource: 'checklist-template', displayName: 'Checklist Template', description: 'Case checklist templates' },
  { resource: 'billing-status', displayName: 'Billing Status', description: 'Billing workflow statuses' },
  { resource: 'exclusion', displayName: 'Exclusion', description: 'Exclusion reasons' },
  { resource: 'suspended-reason', displayName: 'Suspended Reason', description: 'Case suspension reasons' }
];
