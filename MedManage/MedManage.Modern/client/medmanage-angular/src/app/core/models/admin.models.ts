/** System configuration DTO */
export interface SystemData {
  systemDataId: number;
  systemCountryId: number | null;
  systemUniqueIdentifier: string | null;
  systemEmailAddress: string | null;
  smtpServer: string | null;
  ssl: boolean | null;
  username: string | null;
  specialIcu: number | null;
  icu: number | null;
  highCare: number | null;
  neuroWard: number | null;
  inIsolation: number | null;
  generalWard: number | null;
  paediatric: number | null;
  maternity: number | null;
  dayCase: number | null;
  stepDown: number | null;
  psychiatric: number | null;
  address1: string | null;
  address2: string | null;
  address3: string | null;
  address4: string | null;
  addressCode: string | null;
  email: string | null;
  fax: string | null;
  telephone: string | null;
  website: string | null;
  hasLogo: boolean;
  defaultProviderId: number | null;
}

/** Create/Update system data request */
export interface SystemDataRequest {
  systemCountryId?: number | null;
  systemEmailAddress?: string | null;
  smtpServer?: string | null;
  ssl?: boolean | null;
  username?: string | null;
  password?: string | null;
  specialIcu?: number | null;
  icu?: number | null;
  highCare?: number | null;
  neuroWard?: number | null;
  inIsolation?: number | null;
  generalWard?: number | null;
  paediatric?: number | null;
  maternity?: number | null;
  dayCase?: number | null;
  stepDown?: number | null;
  psychiatric?: number | null;
  address1?: string | null;
  address2?: string | null;
  address3?: string | null;
  address4?: string | null;
  addressCode?: string | null;
  email?: string | null;
  fax?: string | null;
  telephone?: string | null;
  website?: string | null;
  defaultProviderId?: number | null;
}

/** User DTO from user management API */
export interface UserDto {
  userId: string;
  userName: string;
  email: string | null;
  isApproved: boolean;
  isLockedOut: boolean;
  createDate: string;
  lastLoginDate: string;
  lastActivityDate: string;
  roles: string[];
}

/** Role DTO */
export interface RoleDto {
  roleId: string;
  roleName: string;
  description: string | null;
}

/** Assign roles request */
export interface AssignRolesRequest {
  userId: string;
  roles: string[];
}

/** Generic API response wrapper */
export interface ApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
  errors?: string[];
}
