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

  async associateCart(userId: number): Promise<Result<string>> {
    const body = { userId };
    return this.api.post<string>(`${this.carritoEndpoint}/associate-cart`, body, 'application/json');

  }

  // Añadir un producto al carrito
  async addProductToCart(productId: number, userId: number, quantity: number): Promise<Result<number>> {
    // Enviar los datos al backend
    const body = { productId, userId, quantity };
    const result = await this.api.post<number>(`${this.carritoEndpoint}/addtoshopcart`, body, 'application/json');
  
    if (result.success) {
      console.log(`Producto añadido con éxito al carrito con ID: ${result.data}`);
    }
  
    return result; 
  }
  

  // Obtener todos los productos en el carrito
  async getProductosCarrito(): Promise<Result<ProductoCarrito[]>> {
    const result = await this.api.get<ProductoCarrito[]>(`${this.productoCarritoEndpoint}/productosCarrito`);
  
    if (result.success && result.data.length > 0) {
      // Buscar el carritoId del último producto añadido
      const lastProduct = result.data.reduce((latest, current) => {
        return current.id > latest.id ? current : latest;
      });
  
      const carritoId = lastProduct.carritoId;
  
      // Guardar el carritoId en el localStorage si no está o si es diferente
      const storedCarritoId = localStorage.getItem('carritoId');
      if (!storedCarritoId || Number(storedCarritoId) !== carritoId) {
        localStorage.setItem('carritoId', carritoId.toString());
        console.log(`CarritoId actualizado en localStorage: ${carritoId}`);
      }
    }
  
    return result;
  }
  

  async getProductosCarritoId(carritoId: number): Promise<Result<ProductoCarrito[]>> {
    const result = await this.api.get<ProductoCarrito[]>(`${this.productoCarritoEndpoint}/productosCarrito/${carritoId}`);
    
    if (result.success && result.data.length > 0) {
      // Buscar el carritoId del último producto añadido
      const lastProduct = result.data.reduce((latest, current) => {
        return current.id > latest.id ? current : latest;
      });
  
      const lastCarritoId = lastProduct.carritoId;
  
      // Guardar el carritoId en el localStorage si no está o si es diferente
      const storedCarritoId = localStorage.getItem('carritoId');
      if (!storedCarritoId || Number(storedCarritoId) !== lastCarritoId) {
        localStorage.setItem('carritoId', lastCarritoId.toString());
        console.log(`CarritoId actualizado en localStorage desde getProductosCarritoId: ${lastCarritoId}`);
      }
    }
  
    return result;
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
