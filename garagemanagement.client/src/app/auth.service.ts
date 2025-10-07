import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, BehaviorSubject } from 'rxjs';
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

  private currentUserSubject = new BehaviorSubject<Decoded | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  private loggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  loggedIn$ = this.loggedInSubject.asObservable();
    profile: any;

  constructor(private http: HttpClient, private router: Router) {
    this.restore();
  }

  register(data: any): Observable<any> {
    return this.http.post<AuthResponse>(`${this.base}/Auth/register`, data)
      .pipe(tap(res => this.setToken(res.token)));
  }

  login(credentials: any) {
    return this.http.post<{ token: string }>(`${this.base}/Auth/login`, credentials)
      .pipe(
        tap(response => {
          this.setToken(response.token);
          const decoded: Decoded = jwtDecode(response.token);
          this.currentUserSubject.next(decoded);
          this.loggedInSubject.next(true);
        })
      );
  }

  getUserProfile() {
    return this.http.get<any>(`${this.base}/Auth/me`)
          
  }

  forgotPassword(emailOrUsername: string): Observable<{exists:boolean}> {
    return this.http.post<{ exists: boolean, emailOrUsername ?:string}>(`${this.base}/Auth/forgot-password`, { emailOrUsername });
  }

  resetPassword(data: { emailOrUsername: string; newPassword: string; confirmPassword: string }): Observable<any> {
    return this.http.post(`${this.base}/Auth/reset-password`, data);
  }

  // helpers
  get token() { return this._token; }

  getRole(): string | null {
    //  return this.currentUserSubject.value ? (this._role ?? null) : null;
    return this._role ?? localStorage.getItem('role');

  }

  isLoggedIn(): boolean {
    const t = localStorage.getItem('token');
    if (!t) return false;
    try {
      const d = jwtDecode<Decoded>(t);
      if (!d.exp) return false;
      return d.exp * 1000 > Date.now();
    } catch { return false; }
  }

  isAdmin(): boolean {
    //return role?.toLowerCase() === 'admin';
    const role = this.getRole();
    return role === 'Admin';


  }

  logout() {
    localStorage.removeItem('token');
    this._token = null;
    this._role = null;
    this.currentUserSubject.next(null);
    this.loggedInSubject.next(false);
    this.router.navigate(['/login']);
  }

  private setToken(t: string) {
    this._token = t;
    localStorage.setItem('token', t);
    try {
      const decoded = jwtDecode<Decoded>(t);
      this._role = decoded.role ?? null;
      //if (this._role) {
      //  localStorage.setItem('role', this._role);
      //}
      this.currentUserSubject.next(decoded);
      this.loggedInSubject.next(true);
      console.log("Full decoded token:", decoded);


    } catch {
      this._role = null;
      this.currentUserSubject.next(null);
      this.loggedInSubject.next(false);
    }
  }

  private restore() {
    const t = localStorage.getItem('token');
    if (t && !this.isExpired(t)) {
      this._token = t;
      try {
        const decoded = jwtDecode<Decoded>(t);
        this._role = decoded.role ?? null;
        if (this._role) {
          localStorage.setItem('role', this._role);
        }
        this.currentUserSubject.next(decoded);
        this.loggedInSubject.next(true);
        this.getUserProfile().subscribe(profile => {
          if (profile?.role) {
            this._role = profile.role;
            localStorage.setItem('role', profile.role);
          }
        });
      } catch {
        this._role = null;
        this.currentUserSubject.next(null);
        this.loggedInSubject.next(false);
      }
    } else {
      localStorage.removeItem('token');
      this.loggedInSubject.next(false);
    }
  }

  private isExpired(t: string) {
    try {
      const d = jwtDecode<Decoded>(t);
      if (!d.exp) return true;
      return d.exp * 1000 < Date.now();
    } catch { return true; }
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }

  getUser() {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  }
}
