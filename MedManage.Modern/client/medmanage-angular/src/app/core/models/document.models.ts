/** Document metadata returned from the API */
export interface DocumentDto {
  id: number;
  entityType: string;
  entityId: number;
  fileName: string;
  fileType: string | null;
  fileSize: number;
  contentType: string | null;
  isImage: boolean;
  hasThumbnail: boolean;
  dateUploaded: string;
  uploadedBy: string | null;
}

/** Standard API response wrapper */
export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message: string | null;
  errors: string[];
}
