export interface ImportResultDto {
  importHistoryId: number;
  importType: string;
  fileName: string;
  importDate: string;
  importedBy: string | null;
  totalRecords: number;
  importedRecords: number;
  skippedRecords: number;
  errorRecords: number;
  status: string;
  errorDetails: string | null;
  validationErrors: ImportValidationError[];
}

export interface ImportValidationError {
  row: number;
  field: string;
  message: string;
  value: string | null;
}

export interface ImportHistoryDto {
  importHistoryId: number;
  importType: string;
  fileName: string;
  importDate: string;
  importedBy: string | null;
  totalRecords: number;
  importedRecords: number;
  skippedRecords: number;
  errorRecords: number;
  status: string;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message: string | null;
  errors: string[] | null;
}
