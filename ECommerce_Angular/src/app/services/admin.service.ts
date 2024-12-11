import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private URL = `${environment.apiUrl}ControladorAdmin/`;

  constructor(private http: HttpClient) {}


  getUsuarios(): Observable<any[]> {
    return this.http.get<any[]>(`${this.URL}usuarios`);
  }


  updateUsuario(id: number, usuario: any): Observable<any> {
    return this.http.put(`${this.URL}usuarios/${id}`, usuario);
  }


  deleteUsuario(userId: number): Observable<any> {
    return this.http.delete(`${this.URL}usuarios/${userId}`);
  }

 
  getProductos(): Observable<any[]> {
    return this.http.get<any[]>(`${this.URL}productos`);
  }

  
  createProducto(producto: any): Observable<any> {
    return this.http.post(`${this.URL}productos`, producto);
  }

  updateProducto(id: number, producto: any): Observable<any> {
    return this.http.put(`${this.URL}productos/${id}`, producto);
  }

  
  deleteProducto(id: number): Observable<any> {
    return this.http.delete(`${this.URL}productos/${id}`);
  }
}
