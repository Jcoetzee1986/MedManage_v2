export interface EpisodeDto {
  episodeId: number;
  episodeDescription: string | null;
  memberId: number | null;
  dateCreated: string | null;
  dateInserted: string;
  dateModified: string | null;
  dateDeleted: string | null;
}

export interface CreateEpisodeDto {
  episodeDescription?: string | null;
  memberId?: number | null;
  dateCreated?: string | null;
}

export interface UpdateEpisodeDto {
  episodeId: number;
  episodeDescription?: string | null;
  memberId?: number | null;
  dateCreated?: string | null;
}

export interface EpisodeSearchFilters {
  episodeName?: string | null;
  memberId?: number | null;
  dateFrom?: string | null;
  dateTo?: string | null;
  includeDeleted?: boolean;
}

export interface EpisodeCaseDto {
  episodeId: number;
  caseId: number;
  dateCreated: string | null;
  dateInserted: string | null;
  dateUpdated: string | null;
}

export interface LinkCaseToEpisodeDto {
  caseId: number;
  dateCreated?: string | null;
}

export interface ApiResponse<T> {
  data: T;
  success: boolean;
  message?: string;
  errors?: string[];
}
