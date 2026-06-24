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
  amount?: number;
  finalInvoiceAmount?: number;
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
  remittanceNumber?: string;
  dateFrom?: string;
  dateTo?: string;
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
