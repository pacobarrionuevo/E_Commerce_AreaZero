import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { Carrito } from '../models/carrito';
import { ProductoCarrito } from '../models/producto-carrito';

@Injectable({
  providedIn: 'root'
})
export class CarritoService {

  private carritoEndpoint = 'ControladorCarrito';
  private productoCarritoEndpoint = 'ControladorProductoCarrito';

  constructor(private api: ApiService) {}

  async getCarritos(): Promise<Result<Carrito[]>> {
    return this.api.get<Carrito[]>(`${this.carritoEndpoint}/carritos`);
  }

  // Añadir un producto al carrito
  async addProductToCart(productId: number, userId: number, quantity: number): Promise<Result<string>> {
    // Enviar solo productId y quantity, sin userId
    const body = { productId, userId, quantity };  // Sin userId
    return this.api.post<string>(`${this.carritoEndpoint}/addtoshopcart`, body, 'application/json');
  }
  

  // Obtener todos los productos en el carrito
  async getProductosCarrito(): Promise<Result<ProductoCarrito[]>> {
    return this.api.get<ProductoCarrito[]>(`${this.productoCarritoEndpoint}/productosCarrito`);
  }

  // Eliminar un producto del carrito
  async removeProductFromCart(productId: number, carritoId: number): Promise<Result<string>> {
    const body = { productId, carritoId };
    return this.api.put<string>(`${this.productoCarritoEndpoint}/eliminarproductocarrito`, body, 'application/json');
  }

  // Modificar la cantidad de un producto en el carrito
  async modifyProductQuantity(productId: number, carritoId: number, quantity: number): Promise<Result<string>> {
    const body = { productId, carritoId, quantity };
    return this.api.put<string>(`${this.productoCarritoEndpoint}/cambiarcantidad`, body, 'application/json');
  }
}
