// Authentication Models and Interfaces

export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}

export interface ForgotPasswordRequest {
  email: string;
}

export interface ResetPasswordRequest {
  email: string;
  pin: string;
  newPassword: string;
  confirmPassword: string;
}

export interface VerifyPinRequest {
  email: string;
  pin: string;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface AuthResponse {
  success: boolean;
  token?: string;
  refreshToken?: string;
  expiresAt?: Date;
  refreshTokenExpiresAt?: Date;
  message?: string;
  user?: UserInfo;
}

export interface UserInfo {
  userId: string;
  username: string;
  email: string;
  roles: string[];
}

export interface PasswordResetResponse {
  success: boolean;
  message: string;
}

export interface UsernameCheckResponse {
  available: boolean;
}

export interface VerifyPinResponse {
  valid: boolean;
  message?: string;
}
