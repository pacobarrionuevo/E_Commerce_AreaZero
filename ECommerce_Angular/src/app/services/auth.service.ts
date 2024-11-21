import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthRequest } from '../models/auth-request';
import { AuthResponse } from '../models/auth-response';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private URL = `${environment.apiUrl}`;

  constructor(private http: HttpClient) {}

  register(authData: AuthRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.URL}ControladorUsuario/Registro`, authData);
  }

  login(authData: AuthRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.URL}ControladorUsuario/login`, authData);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}
