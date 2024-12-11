import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { Carrito } from '../models/carrito';
import { ProductoCarrito } from '../models/producto-carrito';
import { Product } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class CarritoService {

  private carritoEndpoint = 'ControladorCarrito';
  private productoCarritoEndpoint = 'ControladorProductoCarrito';
  private productoEndpoint = 'ControladorProducto';

  constructor(private api: ApiService) {}

  // Obtiene los carritos
  async getCarritos(): Promise<Result<Carrito[]>> {
    return this.api.get<Carrito[]>(`${this.carritoEndpoint}/carritos`);
  }

  // Añadir un producto al carrito
  async addProductToCart(productId: number, userId: number, quantity: number): Promise<Result<number>> {
    const body = { productId, userId, quantity };
    const result = await this.api.post<number>(`${this.carritoEndpoint}/addtoshopcart`, body, 'application/json');
    return result; 
  }

  // Pasar los productos del local storage al servidor
  async localtoCart(): Promise<Result<string>> {
    const cart = JSON.parse(localStorage.getItem('cart') || '[]'); // Obtener el carrito del almacenamiento local
    const token = localStorage.getItem('token'); // Obtener el token del almacenamiento local


    this.api.jwt = token;
    const body = cart.map(item => ({ProductId: item.productId, Cantidad: item.quantity}));

        const result = await this.api.post<string>(`${this.carritoEndpoint}/PasaProductoAlCarrito`, body, 'application/json');
        return result;

   
}



  // Obtiene todos los productos en el carrito
  async getProductosCarrito(): Promise<Result<ProductoCarrito[]>> {
    const userId = localStorage.getItem('usuarioId')
    const result = await this.api.get<ProductoCarrito[]>(`${this.productoCarritoEndpoint}/productosCarrito/${userId}`);
  
    return result;
  }

  // Lllamar los productos por el id
  async getProductById(productId: number): Promise<Result<Product>> {
    const result = await this.api.get<Product>(`${this.productoEndpoint}/${productId}`);
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
  async removeProductFromCart(productId: number, userId: number): Promise<Result<string>> {
    const body = { productId, userId };
    return this.api.put<string>(`${this.productoCarritoEndpoint}/eliminarproductocarrito`, body, 'application/json');
  }

  // Modificar la cantidad de un producto en el carrito
  async modifyProductQuantity(productId: number, carritoId: number, quantity: number): Promise<Result<string>> {
    const body = { productId, carritoId, quantity };
    return this.api.put<string>(`${this.productoCarritoEndpoint}/cambiarcantidad`, body, 'application/json');
  }
  
  
}