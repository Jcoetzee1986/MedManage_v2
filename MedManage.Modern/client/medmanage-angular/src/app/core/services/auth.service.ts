import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap, map } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import {
  LoginRequest,
  RegisterRequest,
  ChangePasswordRequest,
  ForgotPasswordRequest,
  ResetPasswordRequest,
  VerifyPinRequest,
  RefreshTokenRequest,
  AuthResponse,
  UserInfo,
  PasswordResetResponse,
  UsernameCheckResponse,
  VerifyPinResponse,
  AvailableClientDto,
  SwitchClientRequest
} from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);
  
  private readonly TOKEN_KEY = 'medmanage_token';
  private readonly REFRESH_TOKEN_KEY = 'medmanage_refresh_token';
  private readonly USER_KEY = 'medmanage_user';
  private readonly ACTIVE_CLIENT_KEY = 'medmanage_active_client_id';
  private readonly API_URL = `${environment.apiUrl}/auth`;

  private currentUserSubject = new BehaviorSubject<UserInfo | null>(this.getUserFromStorage());
  private activeClientSubject = new BehaviorSubject<number | null>(this.getStoredClientId());

  /** Observable that emits whenever the active client changes */
  activeClient$ = this.activeClientSubject.asObservable();

  /** Get the current active client ID */
  get activeClientId(): number | null {
    return this.activeClientSubject.value;
  }

  /** Set the active client ID (persists to localStorage and notifies subscribers) */
  setActiveClient(mainClientId: number): void {
    localStorage.setItem(this.ACTIVE_CLIENT_KEY, String(mainClientId));
    this.activeClientSubject.next(mainClientId);
  }

  private getStoredClientId(): number | null {
    const stored = localStorage.getItem('medmanage_active_client_id');
    return stored ? parseInt(stored, 10) : null;
  }
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor() { }

  /**
   * Login with username and password
   */
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/login`, request).pipe(
      tap(response => {
        if (response.success && response.token && response.user) {
          this.setToken(response.token);
          if (response.refreshToken) {
            this.setRefreshToken(response.refreshToken);
          }
          this.setUser(response.user);
          this.currentUserSubject.next(response.user);
        }
      })
    );
  }

  /**
   * Register a new user
   */
  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/register`, request).pipe(
      tap(response => {
        if (response.success && response.token && response.user) {
          this.setToken(response.token);
          if (response.refreshToken) {
            this.setRefreshToken(response.refreshToken);
          }
          this.setUser(response.user);
          this.currentUserSubject.next(response.user);
        }
      })
    );
  }

  /**
   * Get current logged-in user info
   */
  getCurrentUser(): Observable<UserInfo> {
    return this.http.get<UserInfo>(`${this.API_URL}/me`).pipe(
      tap(user => {
        this.setUser(user);
        this.currentUserSubject.next(user);
      })
    );
  }

  /**
   * Change current user's password
   */
  changePassword(request: ChangePasswordRequest): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.API_URL}/change-password`, request);
  }

  /**
   * Check if username is available
   */
  checkUsernameAvailability(username: string): Observable<boolean> {
    return this.http.get<UsernameCheckResponse>(`${this.API_URL}/check-username/${username}`)
      .pipe(map(response => response.available));
  }

  /**
   * Request password reset PIN
   */
  forgotPassword(request: ForgotPasswordRequest): Observable<PasswordResetResponse> {
    return this.http.post<PasswordResetResponse>(`${this.API_URL}/forgot-password`, request);
  }

  /**
   * Verify password reset PIN
   */
  verifyResetPin(request: VerifyPinRequest): Observable<boolean> {
    return this.http.post<VerifyPinResponse>(`${this.API_URL}/verify-pin`, request)
      .pipe(map(response => response.valid));
  }

  /**
   * Reset password with PIN
   */
  resetPassword(request: ResetPasswordRequest): Observable<PasswordResetResponse> {
    return this.http.post<PasswordResetResponse>(`${this.API_URL}/reset-password`, request);
  }

  /**
   * Refresh access token using refresh token
   */
  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      throw new Error('No refresh token available');
    }

    const request: RefreshTokenRequest = { refreshToken };
    return this.http.post<AuthResponse>(`${this.API_URL}/refresh`, request).pipe(
      tap(response => {
        if (response.success && response.token) {
          this.setToken(response.token);
          if (response.refreshToken) {
            this.setRefreshToken(response.refreshToken);
          }
          if (response.user) {
            this.setUser(response.user);
            this.currentUserSubject.next(response.user);
          }
        }
      })
    );
  }

  /**
   * Revoke a specific refresh token
   */
  revokeToken(refreshToken: string): Observable<{ message: string }> {
    const request: RefreshTokenRequest = { refreshToken };
    return this.http.post<{ message: string }>(`${this.API_URL}/revoke`, request);
  }

  /**
   * Revoke all refresh tokens for current user (logout from all devices)
   */
  revokeAllTokens(): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.API_URL}/revoke-all`, {});
  }

  /**
   * Get available main clients for the current user
   */
  getAvailableClients(): Observable<AvailableClientDto[]> {
    return this.http.get<AvailableClientDto[]>(`${this.API_URL}/available-clients`);
  }

  /**
   * Switch to a different main client context
   */
  switchClient(request: SwitchClientRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.API_URL}/switch-client`, request).pipe(
      tap(response => {
        if (response.success && response.token) {
          this.setToken(response.token);
          if (response.user) {
            this.setUser(response.user);
            this.currentUserSubject.next(response.user);
          }
        }
      })
    );
  }

  /**
   * Logout current user. Releases all case locks and revokes tokens.
   */
  logout(): void {
    // Guard against re-entrant logout (prevents infinite loop)
    const token = this.getToken();
    if (!token) {
      // Already logged out — just clean up and navigate
      this.removeToken();
      this.removeRefreshToken();
      this.removeUser();
      this.currentUserSubject.next(null);
      this.router.navigate(['/login']);
      return;
    }

    // Only attempt lock release and token revocation if token is still valid
    const expiry = this.getTokenExpiration();
    const tokenStillValid = !expiry || expiry > new Date();

    if (tokenStillValid) {
      // Release all case locks held by this user (fire-and-forget)
      this.http.delete(`${environment.apiUrl}/cases/locks/mine`).subscribe({
        error: () => {} // best-effort
      });

      // Try to revoke refresh token
      const refreshToken = this.getRefreshToken();
      if (refreshToken) {
        this.revokeToken(refreshToken).subscribe({
          error: () => {} // best-effort
        });
      }
    }

    this.removeToken();
    this.removeRefreshToken();
    this.removeUser();
    this.currentUserSubject.next(null);
    this.router.navigate(['/login']);
  }

  /**
   * Check if user is authenticated
   */
  isAuthenticated(): boolean {
    const token = this.getToken();
    if (!token) return false;

    // Check if token is expired
    const expiry = this.getTokenExpiration();
    if (expiry && expiry < new Date()) {
      // Token expired — remove access token but keep refresh token
      // so the interceptor can attempt a refresh on the next 401
      this.removeToken();
      return false;
    }

    return true;
  }

  /**
   * Get JWT token
   */
  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  /**
   * Get refresh token
   */
  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  /**
   * Get current user from observable
   */
  getCurrentUserValue(): UserInfo | null {
    return this.currentUserSubject.value;
  }

  /**
   * Check if user has specific role
   */
  hasRole(role: string): boolean {
    const user = this.getCurrentUserValue();
    return user?.roles?.includes(role) ?? false;
  }

  // Private helper methods

  private setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  private removeToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }

  private setRefreshToken(token: string): void {
    localStorage.setItem(this.REFRESH_TOKEN_KEY, token);
  }

  private removeRefreshToken(): void {
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
  }

  private setUser(user: UserInfo): void {
    localStorage.setItem(this.USER_KEY, JSON.stringify(user));
  }

  private removeUser(): void {
    localStorage.removeItem(this.USER_KEY);
  }

  private getUserFromStorage(): UserInfo | null {
    const userJson = localStorage.getItem(this.USER_KEY);
    if (!userJson) return null;
    
    try {
      return JSON.parse(userJson);
    } catch {
      return null;
    }
  }

  private getTokenExpiration(): Date | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      if (payload.exp) {
        return new Date(payload.exp * 1000);
      }
    } catch {
      return null;
    }

    return null;
  }
}
