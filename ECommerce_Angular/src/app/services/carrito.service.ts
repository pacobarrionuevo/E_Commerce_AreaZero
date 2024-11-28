import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { ProductoCarrito } from '../models/producto-carrito';

@Injectable({
  providedIn: 'root'
})
export class CarritoService {

  private carritoEndpoint = 'ControladorCarrito';
  private productoCarritoEndpoint = 'ControladorProductoCarrito';
  private imageBaseUrl = 'https://localhost:7133/'; 
  constructor(private api: ApiService) {}

  async getCarritos(): Promise<Result<ProductoCarrito[]>> {
    return this.api.get<ProductoCarrito[]>(`${this.carritoEndpoint}/carritos`);
  }

  async addProductToCart(productId: number, userId: number, quantity: number): Promise<Result<string>> {
    const body = { productId, userId, quantity };
    return this.api.post<string>(`${this.carritoEndpoint}/addtoshopcart`, body, 'application/json');
  }

  async getProductosCarrito(): Promise<Result<ProductoCarrito[]>> {
    try {
      const response = await this.api.get<ProductoCarrito[]>(`${this.productoCarritoEndpoint}/productosCarrito`);
      if (response.success) {
        const productosCarrito = response.data.map(item => {
          item.producto.ruta = `${this.imageBaseUrl}/${item.producto.ruta}`; 
          return new ProductoCarrito(
            item.productoId,
            item.carritoId,
            item.cantidad,
            item.producto
          );
        });
        return Result.success(response.statusCode, productosCarrito);
      } else {
        return Result.error(response.statusCode, response.error);
      }
    } catch (error) {
      return Result.error(500, error.message);
    }
  }

  async removeProductFromCart(productId: number, carritoId: number): Promise<Result<string>> {
    const body = { productId, carritoId };
    return this.api.put<string>(`${this.productoCarritoEndpoint}/eliminarproductocarrito`, body, 'application/json');
  }

  async modifyProductQuantity(productId: number, carritoId: number, quantity: number): Promise<Result<string>> {
    const body = { productId, carritoId, quantity };
    return this.api.put<string>(`${this.productoCarritoEndpoint}/cambiarcantidad`, body, 'application/json');
  }
}
