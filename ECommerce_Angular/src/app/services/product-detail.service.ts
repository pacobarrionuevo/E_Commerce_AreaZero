import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Product } from '../models/product';  // Importa la clase Product

@Injectable({
  providedIn: 'root'
})
export class ProductDetailService {
  private baseUrl = 'https://localhost:7133/api/ControladorCatalogo';

  constructor(private http: HttpClient) {}

  getProducto(id: number): Observable<Product> {
    const url = `${this.baseUrl}/${id}`;
    return this.http.get<Product>(url);
  }
}
