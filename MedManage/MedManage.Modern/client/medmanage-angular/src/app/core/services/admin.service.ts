import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  SystemData,
  SystemDataRequest,
  UserDto,
  RoleDto,
  AssignRolesRequest,
  CreateUserRequest,
  AdminResetPasswordRequest,
  ApiResponse
} from '../models/admin.models';

/**
 * Service for system administration operations.
 * Provides methods for system configuration CRUD and user management.
 */
@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private readonly http = inject(HttpClient);
  private readonly systemDataUrl = `${environment.apiUrl}/system-data`;
  private readonly usersUrl = `${environment.apiUrl}/users`;

  // ─── System Data ───────────────────────────────────────────

  /** Get current system configuration */
  getSystemData(): Observable<SystemData | null> {
    return this.http.get<ApiResponse<SystemData | null>>(`${this.systemDataUrl}`)
      .pipe(map(r => r.data));
  }

  /** Get system configuration by ID */
  getSystemDataById(id: number): Observable<SystemData> {
    return this.http.get<ApiResponse<SystemData>>(`${this.systemDataUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  /** Create system configuration */
  createSystemData(request: SystemDataRequest): Observable<SystemData> {
    return this.http.post<ApiResponse<SystemData>>(`${this.systemDataUrl}`, request)
      .pipe(map(r => r.data));
  }

  /** Update system configuration */
  updateSystemData(id: number, request: SystemDataRequest): Observable<SystemData> {
    return this.http.put<ApiResponse<SystemData>>(`${this.systemDataUrl}/${id}`, request)
      .pipe(map(r => r.data));
  }

  /** Delete system configuration */
  deleteSystemData(id: number): Observable<boolean> {
    return this.http.delete<ApiResponse<boolean>>(`${this.systemDataUrl}/${id}`)
      .pipe(map(r => r.data));
  }

  /** Upload system logo */
  uploadLogo(id: number, file: File): Observable<boolean> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<ApiResponse<boolean>>(`${this.systemDataUrl}/${id}/logo`, formData)
      .pipe(map(r => r.data));
  }

  /** Get the logo URL for a given system data ID */
  getLogoUrl(id: number): string {
    return `${this.systemDataUrl}/${id}/logo`;
  }

  // ─── User Management ──────────────────────────────────────

  /** Get all users */
  getUsers(): Observable<UserDto[]> {
    return this.http.get<ApiResponse<UserDto[]>>(`${this.usersUrl}`)
      .pipe(map(r => r.data));
  }

  /** Get a user by ID */
  getUserById(userId: string): Observable<UserDto> {
    return this.http.get<ApiResponse<UserDto>>(`${this.usersUrl}/${userId}`)
      .pipe(map(r => r.data));
  }

  /** Approve a pending user */
  approveUser(userId: string): Observable<boolean> {
    return this.http.post<ApiResponse<boolean>>(`${this.usersUrl}/${userId}/approve`, {})
      .pipe(map(r => r.data));
  }

  /** Lock a user account */
  lockUser(userId: string): Observable<boolean> {
    return this.http.put<ApiResponse<boolean>>(`${this.usersUrl}/${userId}/lock`, {})
      .pipe(map(r => r.data));
  }

  /** Unlock a user account */
  unlockUser(userId: string): Observable<boolean> {
    return this.http.put<ApiResponse<boolean>>(`${this.usersUrl}/${userId}/unlock`, {})
      .pipe(map(r => r.data));
  }

  /** Assign roles to a user */
  assignRoles(userId: string, roles: string[]): Observable<boolean> {
    const request: AssignRolesRequest = { userId, roles };
    return this.http.put<ApiResponse<boolean>>(`${this.usersUrl}/${userId}/roles`, request)
      .pipe(map(r => r.data));
  }

  /** Get all available roles */
  getRoles(): Observable<RoleDto[]> {
    return this.http.get<ApiResponse<RoleDto[]>>(`${this.usersUrl}/roles`)
      .pipe(map(r => r.data));
  }

  /** Create a new user (admin) */
  createUser(request: CreateUserRequest): Observable<UserDto> {
    return this.http.post<ApiResponse<UserDto>>(`${this.usersUrl}`, request)
      .pipe(map(r => r.data));
  }

  /** Reset a user's password (admin) */
  adminResetPassword(userId: string, request: AdminResetPasswordRequest): Observable<boolean> {
    return this.http.post<ApiResponse<boolean>>(`${this.usersUrl}/${userId}/reset-password`, request)
      .pipe(map(r => r.data));
  }

  /** Clear failed login attempts */
  clearFailedAttempts(userId: string): Observable<boolean> {
    return this.http.post<ApiResponse<boolean>>(`${this.usersUrl}/${userId}/clear-attempts`, {})
      .pipe(map(r => r.data));
  }

  /** Permanently block a user */
  permanentlyBlockUser(userId: string): Observable<boolean> {
    return this.http.put<ApiResponse<boolean>>(`${this.usersUrl}/${userId}/block`, {})
      .pipe(map(r => r.data));
  }
}
