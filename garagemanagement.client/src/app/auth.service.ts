import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { environment } from '../environments/environment';
import { Router } from '@angular/router';

interface AuthResponse { token: string; }
interface Decoded { role?: string; exp?: number; name?: string; email?: string; sub?: string; }

@Injectable({ providedIn: 'root' })
export class AuthService {
  private base = environment.apiUrl;
  private _token: string | null = null;
  private _role: string | null = null;

  constructor(private http: HttpClient, private router: Router) { this.restore(); }

  register(data: any): Observable<any> {
    return this.http.post<AuthResponse>(`${this.base}/Auth/register`, data)
      .pipe(tap(res => this.setToken(res.token)));
  }

  login(credentials: any) {
    return this.http.post<{ token: string }>("https://localhost:7086/api/Auth/login", credentials)
      .pipe(
        tap(response => {
          // Save token
          //localStorage.setItem('token', response.token);
          this.setToken(response.token);

        })
      );
  }

  getUserProfile() {

    return this.http.get<any>(`${this.base}/Auth/me`);
      
  }

  forgot(emailOrUsername: string) {
    return this.http.post<{ message: string; devToken?: string }>(`${this.base}/Auth/forgot`, { emailOrUsername });
  }

  reset(emailOrUsername: string, token: string, newPassword: string) {
    return this.http.post<{ message: string }>(`${this.base}/Auth/reset`, { emailOrUsername, token, newPassword });
  }


  // helpers
  get token() { return this._token; }
  get role() { return this._role; }
  isLoggedIn() {
    const t = localStorage.getItem('token');
    if (!t) return false;
    try {
      const d = jwtDecode<Decoded>(t);
      if (!d.exp) return false;
      return d.exp * 1000 > Date.now();
    } catch { return false; }
  }

  isAdmin() { return this.role === 'Admin'; }

  logout() {
    localStorage.removeItem('token');
    this._token = null;
    this._role = null;
  }

  private setToken(t: string) {
    this._token = t;
    localStorage.setItem('token', t);
    try { this._role = jwtDecode<Decoded>(t).role ?? null; } catch { this._role = null; }
  }

  private restore() {
    const t = localStorage.getItem('token');
    if (t && !this.isExpired(t)) {
      this._token = t;
      try { this._role = jwtDecode<Decoded>(t).role ?? null; } catch { this._role = null; }
    } else {
      localStorage.removeItem('token');
    }
  }

  private isExpired(t: string) {
    try {
      const d = jwtDecode<Decoded>(t);
      if (!d.exp) return true;
      return d.exp * 1000 < Date.now();
    } catch { return true; }
  }
}
