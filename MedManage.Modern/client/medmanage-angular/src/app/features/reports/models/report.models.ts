/** Available report types */
export type ReportType = 'case-letter' | 'cases-between-dates' | 'wip-extract' | 'billing-summary';

/** Output format for report generation */
export type ReportFormat = 'pdf' | 'excel';

/** Metadata describing a report */
export interface ReportDefinition {
  type: ReportType;
  title: string;
  description: string;
  supportedFormats: ReportFormat[];
}

/** Parameters for Case Letter report */
export interface CaseLetterParams {
  caseId: number;
}

/** Parameters for Cases Between Dates report */
export interface CasesBetweenDatesParams {
  dateFrom: string;
  dateTo: string;
  statusId?: number;
  caseTypeId?: number;
  mainClientId?: number;
}

/** Parameters for WIP Extract report */
export interface WipExtractParams {
  dateFrom: string;
  dateTo: string;
  mainClientId?: number;
}

/** Parameters for Billing Summary report */
export interface BillingSummaryParams {
  dateFrom: string;
  dateTo: string;
  serviceProviderId?: number;
  mainClientId?: number;
}

/** Union type for all report parameters */
export type ReportParams =
  | CaseLetterParams
  | CasesBetweenDatesParams
  | WipExtractParams
  | BillingSummaryParams;

/** All available report definitions */
export const REPORT_DEFINITIONS: ReportDefinition[] = [
  {
    type: 'case-letter',
    title: 'Case Letter',
    description: 'Generate an authorization/case letter for a specific case.',
    supportedFormats: ['pdf']
  },
  {
    type: 'cases-between-dates',
    title: 'Cases Between Dates',
    description: 'List of cases within a specified date range with optional filters.',
    supportedFormats: ['pdf', 'excel']
  },
  {
    type: 'wip-extract',
    title: 'WIP Extract',
    description: 'Work In Progress financial extract for a date range.',
    supportedFormats: ['pdf', 'excel']
  },
  {
    type: 'billing-summary',
    title: 'Billing Summary',
    description: 'Summary of billing data filtered by dates, provider, and status.',
    supportedFormats: ['pdf', 'excel']
  }
];
