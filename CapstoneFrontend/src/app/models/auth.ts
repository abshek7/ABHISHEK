export type UserRole = 'Admin' | 'Agent' | 'Customer' | 'ClaimsOfficer';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  dateOfBirth: string;
  phone: string;
  address: string;
  occupation: string;
}

export interface AuthResponse {
  token: string;
  email: string;
  role: UserRole;
}

