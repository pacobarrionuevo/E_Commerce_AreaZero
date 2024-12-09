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
  
  async localtoCart(): Promise<Result<string>> {
    const cart = JSON.parse(localStorage.getItem('cart') || '[]'); // Obtener el carrito del almacenamiento local
    const token = localStorage.getItem('token'); // Obtener el token del almacenamiento local

    // Verificar si el token existe
    if (!token) {
        return Result.error(401, "Token no encontrado. No se puede realizar la solicitud.");
    }

    this.api.jwt = token;
    const body = cart.map(item => ({ProductId: item.productId, Cantidad: item.quantity}));
  
    console.log('Payload:', body);

    try {
        const result = await this.api.post<string>(`${this.carritoEndpoint}/PasaProductoAlCarrito`, body, 'application/json');

        if (result.success) {
            console.log("Productos añadidos con éxito al carrito");
            return result;
        }

        // Manejar errores del backend
        console.error(`Error desde el backend: ${result.error}`);
        return result;

    } catch (err) {
        console.error("Error al enviar productos al carrito:", err);
        return Result.error(500, "Error inesperado al enviar productos al carrito.");
    }
}



  // Obtener todos los productos en el carrito
  async getProductosCarrito(): Promise<Result<ProductoCarrito[]>> {
    const userId = localStorage.getItem('usuarioId')
    const result = await this.api.get<ProductoCarrito[]>(`${this.productoCarritoEndpoint}/productosCarrito/${userId}`);
  
    return result;
  }
  
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
  
  //////////////////////
  //PARA IR AL CHECKOUT
  //////////////////////
  async goToCheckout(paymentMethod: string): Promise<Result<{ orderId: number }>> {
    const token = localStorage.getItem('token');
  
    // Verificar si el token existe para autenticar al usuario
    if (!token) {
      return Result.error(401, "Token no encontrado. No se puede proceder al checkout.");
    }
  
    this.api.jwt = token;
  
    try {
      // Llamar al endpoint para crear o recuperar la orden temporal
      const response = await this.api.post<{ orderId: number }>(
        `${this.carritoEndpoint}/crearOrdenTemporal`,
        null, // Asumimos que no se requiere body en esta llamada
        'application/json'
      );
  
      if (response.success) {
        console.log(`Orden temporal creada exitosamente con ID: ${response.data.orderId}`);
        return response;
      } else {
        console.error('Error desde el backend:', response.error);
        return response;
      }
    } catch (err) {
      console.error('Error al intentar crear la orden temporal:', err);
      return Result.error(500, "Error inesperado al crear la orden temporal.");
    }
  }
  


}