export interface BookingDto {
  bookingId: number;
  travelDate: string | null;
  travelTime: string | null;
  appointmentDate: string | null;
  referringPracticeId: number | null;
  memberId: number | null;
  caseId: number | null;
  discipline: string | null;
  consultation: boolean | null;
  admission: boolean | null;
  currentPracticeId: number | null;
  hospital: string | null;
  arrived: boolean | null;
  tisch: string | null;
  authNumber: string | null;
  comments: string | null;
  dateCreated: string;
  dateModified: string | null;
  dateDeleted: string | null;
}

export interface CreateBookingDto {
  travelDate?: string | null;
  travelTime?: string | null;
  appointmentDate?: string | null;
  referringPracticeId?: number | null;
  memberId?: number | null;
  caseId?: number | null;
  discipline?: string | null;
  consultation?: boolean | null;
  admission?: boolean | null;
  currentPracticeId?: number | null;
  hospital?: string | null;
  arrived?: boolean | null;
  tisch?: string | null;
  authNumber?: string | null;
  comments?: string | null;
}

export interface UpdateBookingDto {
  bookingId: number;
  travelDate?: string | null;
  travelTime?: string | null;
  appointmentDate?: string | null;
  referringPracticeId?: number | null;
  memberId?: number | null;
  caseId?: number | null;
  discipline?: string | null;
  consultation?: boolean | null;
  admission?: boolean | null;
  currentPracticeId?: number | null;
  hospital?: string | null;
  arrived?: boolean | null;
  tisch?: string | null;
  authNumber?: string | null;
  comments?: string | null;
}

export interface BookingSearchFilters {
  surname?: string | null;
  name?: string | null;
  dateFrom?: string | null;
  dateTo?: string | null;
  serviceProviderId?: number | null;
  memberNumber?: string | null;
  includeDeleted?: boolean;
  pageNumber?: number;
  pageSize?: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
  errors?: string[];
}
