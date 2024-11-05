import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthRequest} from '../models/auth-request';
import { AuthResponse } from '../models/auth-response';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private URL = `${environment.apiUrl}`;

  constructor(private http: HttpClient) {}

  register(authData: AuthRequest): Observable<AuthResponse> {
    const resultado: Observable<AuthResponse> = this.http.post<AuthResponse>(`${this.URL}api/ControladorUsuario/Registro`, authData)

     return resultado;
  }

  login(authData: AuthRequest): Observable<AuthResponse> {
    const resultado: Observable<AuthResponse> = this.http.post<AuthResponse>(`${this.URL}api/ControladorUsuario/`, authData)

     return resultado;
  }
}
