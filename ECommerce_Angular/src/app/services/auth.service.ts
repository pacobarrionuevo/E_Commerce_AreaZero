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

  getUserDataFromToken(): any {
    const token = localStorage.getItem('accessToken');
    console.log('JWT Token:', token); 
    if (token) {
      const parts = token.split('.');
      if (parts.length !== 3) {
        console.error('El token no está bien estructurado.');
        return null;
      }
      //Payload del token para controlar bien el token en la vista usuario
      const payloadBase64 = parts[1];
      console.log('Payload Base64:', payloadBase64); 

      const payloadJson = atob(payloadBase64);
      console.log('Payload JSON:', payloadJson); 

      try {
        const payload = JSON.parse(payloadJson);
        console.log('Payload del JWT:', payload);
        return {
          id: payload.id || 'ID no disponible',
          name: payload.Nombre || 'Nombre no disponible',
          email: payload.Email || 'Correo no disponible',
          address: payload.Direccion || 'Dirección no disponible'
        };
      } catch (e) {
        console.error('Error al parsear el JSON del payload:', e);
        return null;
      }
    }
    return null;
  }
  updateUserData(user: any): Observable<any> {
    return this.http.post<any>(`${this.URL}ControladorUsuario/update`, user);
  }
  
}
