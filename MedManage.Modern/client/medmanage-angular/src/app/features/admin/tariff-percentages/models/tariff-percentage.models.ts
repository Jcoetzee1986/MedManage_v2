/** Tariff percentage record from the API */
export interface TariffPercentage {
  tariffPercentageId: number;
  percentageAdded: number;
  tariffPeriodName: number;
  startActiveDate: string;
  endActiveDate: string | null;
  status: 'Pending' | 'Processing' | 'Completed' | 'Failed';
  recordsAffected: number | null;
  notes: string | null;
  dateInserted: string | null;
  userID: string | null;
}

/** Request body for creating a new tariff percentage */
export interface CreateTariffPercentageRequest {
  percentageAdded: number;
  tariffPeriodName: number;
  startActiveDate: string;
  endActiveDate?: string;
  notes?: string;
}

/** Request body for updating an existing tariff percentage */
export interface UpdateTariffPercentageRequest {
  percentageAdded?: number;
  startActiveDate?: string;
  endActiveDate?: string;
  notes?: string;
}

/** Job status for a tariff propagation job */
export interface TariffUpdateJobStatus {
  jobId: string;
  status: 'Queued' | 'Processing' | 'Completed' | 'Failed';
  recordsAffected: number | null;
  errorMessage: string | null;
  startedAt: string | null;
  completedAt: string | null;
}
