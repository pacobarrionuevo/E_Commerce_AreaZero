import { Component, OnInit } from '@angular/core';
import { CarritoService } from '../../services/carrito.service';
import { ProductoCarrito } from '../../models/producto-carrito';
import { Product } from '../../models/product';
import { Result } from '../../models/result';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterModule } from '@angular/router';
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, RouterLink],
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  productosCarrito: ProductoCarrito[] = [];
  isLoading: boolean = true;
  errorMessage: string = '';
  userId: number | null = null;
  product: Product | any;

  constructor(private carritoService: CarritoService) {}

  ngOnInit(): void {
    const userIdString = localStorage.getItem('userId');
    this.userId = userIdString ? parseInt(userIdString, 10) : null;

<<<<<<< HEAD
=======
    if (this.userId) {
      this.associateCartToUser();
    } else {
      console.log("Usuario no autenticado, carrito anónimo.");
    }
>>>>>>> origin/paco_tercerarama
    this.loadCartProducts();
  }

  // Cargar los productos del carrito
  loadCartProducts(): void {
  const localCart = JSON.parse(localStorage.getItem('cart') || '[]');
  
  this.productosCarrito = []; // Reiniciar la lista de productos
        if (!this.userId){
          const productRequests = localCart.map((item: { productId: number, quantity: number }) =>
<<<<<<< HEAD
            this.carritoService.getProductById(item.productId).then((result: Result<Product>) => ({
              producto: result.data, // Aquí accedemos a 'data' desde la instancia de Result
=======
            this.carritoService.getProductById(item.productId).then(producto => ({
              producto,
>>>>>>> origin/paco_tercerarama
              cantidad: item.quantity
            }))
          );
      
          Promise.all(productRequests)
            .then(productosCarrito => {
              this.productosCarrito = productosCarrito;
<<<<<<< HEAD
              console.log('Estructura final de productosCarrito:', this.productosCarrito);
            console.log('Contenido de localCart:', localCart);
            console.log('Datos procesados de productosCarrito:', JSON.stringify(this.productosCarrito, null, 2));

=======
              console.log('Productos cargados:', this.productosCarrito);
>>>>>>> origin/paco_tercerarama
            })
            .catch(error => {
              console.error('Error al cargar productos del carrito:', error);
            });
<<<<<<< HEAD
            
        }
  }
  

=======
        }
  }
  

  async associateCartToUser(): Promise<void> {
    if (!this.userId) {
      this.errorMessage = 'No se encontró el ID de usuario.';
      return;
    }
  
    const carritoId = Number(localStorage.getItem('carritoId'));
    if (!carritoId) {
      this.errorMessage = 'No se encontró el carritoId en el almacenamiento local.';
      return;
    }
  
    this.isLoading = true;
    this.errorMessage = '';
    try {
      // Llamada al servicio para asociar el carrito
      const result = await this.carritoService.associateCart(this.userId);
      if (result.success) {
        console.log('Carrito anónimo asociado correctamente al usuario.');
      } else {
        this.errorMessage = `Error al asociar el carrito: ${result.error}`;
      }
    } catch (error) {
      this.errorMessage = `Se produjo un error al asociar el carrito: ${error.message}`;
    } finally {
      this.isLoading = false;
    }
  }
  
>>>>>>> origin/paco_tercerarama

  // Eliminar un producto del carrito
  async removeProduct(productId: number, carritoId: number): Promise<void> {
    const existingCart = localStorage.getItem('cart');
    let cart: { productId: number, quantity: number }[] = [];
      cart = JSON.parse(existingCart);
      const ProductIdCart = cart.findIndex(item => item.productId === productId);
        cart.splice(ProductIdCart, 1);
        localStorage.setItem('cart', JSON.stringify(cart));
        console.log(`Producto con ID ${productId} eliminado del localStorage`);
        this.loadCartProducts();
      return; 
      
  
    // Si no hay productos en el localStorage o no se encuentra el producto, manejar el servidor
    try {
      const result = await this.carritoService.removeProductFromCart(productId, carritoId);
      if (result.success) {
        // Actualizar la lista de productos en memoria
        this.productosCarrito = this.productosCarrito.filter(p => p.productoId !== productId);
        console.log(`Producto con ID ${productId} eliminado del servidor`);
      } else {
        this.errorMessage = `Error al eliminar el producto: ${result.error}`;
      }
    } catch (error) {
      this.errorMessage = `Se produjo un error: ${error.message}`;
    }
  }
  
  // Modificar la cantidad de un producto
  modifyQuantity(productId: number, carritoId: number, event: Event): void {
    const localCart = JSON.parse(localStorage.getItem('cart') || '[]');
    const input = event.target as HTMLInputElement;
    const quantity = parseInt(input.value, 10);
    const productIdCart = localCart.findIndex((item: { productId: number }) => item.productId === productId);
  
    if (productIdCart=== -1) {
      console.error(`Producto con ID ${productId} no encontrado en el carrito.`);
      return;
    }

    localCart[productIdCart].quantity = quantity;
    localStorage.setItem('cart', JSON.stringify(localCart));
  
  }
}
