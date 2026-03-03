import { inject, Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { API_BASE_URL } from '../../config/api';
import { AuthResponse, LoginRequest, RegisterRequest, UserRole } from '../../models/auth';

interface AuthState {
  token: string;
  email: string;
  role: UserRole;
}

@Injectable({
  providedIn: 'root',
})
export class Auth {
  private http = inject(HttpClient);
  private router = inject(Router);

  private state = signal<AuthState | null>(this.loadState());

  isAuthenticated = computed(() => this.state() !== null);
  role = computed<UserRole | null>(() => this.state()?.role ?? null);
  email = computed<string | null>(() => this.state()?.email ?? null);

  login(request: LoginRequest) {
    return this.http.post<AuthResponse>(`${API_BASE_URL}/Auth/login`, request);
  }

  register(request: RegisterRequest) {
    return this.http.post<{ message: string }>(`${API_BASE_URL}/Auth/register`, request);
  }

  setAuth(response: AuthResponse) {
    const newState: AuthState = {
      token: response.token,
      email: response.email,
      role: response.role,
    };
    this.state.set(newState);
    localStorage.setItem('auth_state', JSON.stringify(newState));
  }

  clearAuth() {
    this.state.set(null);
    localStorage.removeItem('auth_state');
    this.router.navigate(['/login']);
  }

  getToken() {
    return this.state()?.token ?? null;
  }

  private loadState(): AuthState | null {
    const raw = localStorage.getItem('auth_state');
    if (!raw) {
      return null;
    }
    try {
      const parsed = JSON.parse(raw) as AuthState;
      if (!parsed.token || !parsed.email || !parsed.role) {
        return null;
      }
      return parsed;
    } catch {
      return null;
    }
  }
}

