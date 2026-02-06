import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { Router } from '@angular/router';
import { AuthResponse } from '../models/auth';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = environment.apiAuthUrl;

  currentUser = signal<AuthResponse | null>(this.getUserFromStorage());

  private getUserFromStorage(): AuthResponse | null {
    const token = localStorage.getItem('token');
    const username = localStorage.getItem('username');
    const role = localStorage.getItem('role');

    if (token && username && role) {
      return { token, username, role } as AuthResponse;
    }
    return null;
  }

  login(credentials: any) {
    return this.http.post<AuthResponse>(this.apiUrl, credentials).pipe(
      tap((res) => {
        localStorage.setItem('token', res.token);
        localStorage.setItem('username', res.username);
        localStorage.setItem('role', res.role);
        this.currentUser.set(res);
      }),
    );
  }

  logout() {
    this.currentUser.set(null);
    localStorage.clear();
    this.router.navigate(['/login']);
  }
}
