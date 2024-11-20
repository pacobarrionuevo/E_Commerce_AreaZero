import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = `${environment.apiUrl}ControladorUsuario`;
  private tokenKey = 'authToken';

  constructor(private http: HttpClient) {}

  login(authData: { email: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, authData);
  }

  register(authData: { nombre: string; email: string; password: string; direccion: string }): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Registro`, authData);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }
}
