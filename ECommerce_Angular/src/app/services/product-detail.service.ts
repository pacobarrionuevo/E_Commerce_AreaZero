import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product';  

@Injectable({
  providedIn: 'root'
})
export class ProductDetailService {
  private baseUrl = 'https://areazero.runasp.net/api/ControladorCatalogo';

  constructor(private http: HttpClient) {}

  // Obtener los detalles del producto por su id
  getProducto(id: number): Observable<Product> {
    const url = `${this.baseUrl}/${id}`;
    return this.http.get<Product>(url);
  }
}