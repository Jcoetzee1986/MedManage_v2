/**
 * Finance/Billing domain models.
 * These correspond to the API DTOs from the .NET CaseBillingController.
 */

export interface CaseBillingDto {
  id: number;
  caseId: number;
  caseNumber?: string;
  memberId?: number;
  memberName?: string;
  memberNumber?: string;
  providerId?: number;
  providerName?: string;
  practiceNumber?: string;
  accountNumber?: string;
  invoiceNumber?: string;
  billingStatusId?: number;
  billingStatusName?: string;
  dateReceived?: string;
  dateSubmitted?: string;
  datePaid?: string;
  amount?: number;
  amountPaid?: number;
  finalInvoiceAmount?: number;
  remittanceNumber?: string;
  submitted?: boolean;
  reported?: boolean;
  reportedDate?: string;
  comments?: string;
  dateCreated?: string;
  dateUpdated?: string;
  createdBy?: string;
}

export interface CreateBillingRequest {
  caseId: number;
  accountNumber?: string;
  invoiceNumber?: string;
  billingStatusId?: number;
  dateReceived?: string;
  dateSubmitted?: string;
  datePaid?: string;
  accountDateFrom?: string;
  accountDateTo?: string;
  amount?: number;
  finalInvoiceAmount?: number;
  discount?: number;
  penalty?: number;
  rejectedAmount?: number;
  remittanceNumber?: string;
  submitted?: boolean;
  reported?: boolean;
  reportedDate?: string;
  patientName?: string;
  patientSurname?: string;
  patientInitials?: string;
  comments?: string;
}

export interface UpdateBillingRequest extends CreateBillingRequest {
  id: number;
}

export interface BillingSearchRequest {
  providerId?: number;
  providerName?: string;
  accountNumber?: string;
  memberName?: string;
  memberNumber?: string;
  billingStatusId?: number;
  isPaid?: boolean;
  remittance?: string;
  remittanceNumber?: string;
  dateFrom?: string;
  dateTo?: string;
  mainClientId?: number;
  pageNumber?: number;
  pageSize?: number;
  sortField?: string;
  sortDirection?: 'asc' | 'desc';
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface BulkPaymentRequest {
  billingIds: number[];
  amountPaid: number;
  datePaid: string;
  comments?: string;
}

export interface BulkPaymentResult {
  updatedCount: number;
  failedIds: number[];
}

export interface RemittanceUpdateRequest {
  billingIds: number[];
  remittanceNumber: string;
}

export interface BillingSummary {
  caseId: number;
  totalAmount: number;
  totalPaid: number;
  outstanding: number;
  billingCount: number;
}

// ─── Billing Comments ─────────────────────────────────────────

export interface BillingCommentDto {
  caseBillingCommentId: number;
  caseBillingId?: number;
  comment?: string;
  dateInserted?: string;
  userID?: string;
}

export interface CreateBillingCommentRequest {
  comment: string;
}
