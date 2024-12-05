import { Component, OnInit } from '@angular/core';
import { CarritoService } from '../../services/carrito.service';
import { ProductoCarrito } from '../../models/producto-carrito';
import { Product } from '../../models/product';
import { Result } from '../../models/result';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
<<<<<<< HEAD
import { RouterLink, RouterModule } from '@angular/router';
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, RouterLink],
=======
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
>>>>>>> origin/Fitin
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

    if (this.userId) {
      this.associateCartToUser();
    } else {
      console.log("Usuario no autenticado, carrito anónimo.");
    }
    this.loadCartProducts();
  }

  // Cargar los productos del carrito
  loadCartProducts(): void {
<<<<<<< HEAD
  const localCart = JSON.parse(localStorage.getItem('cart') || '[]');
  
  this.productosCarrito = []; // Reiniciar la lista de productos
        if (!this.userId){
          const productRequests = localCart.map((item: { productId: number, quantity: number }) =>
            this.carritoService.getProductById(item.productId).then(producto => ({
              producto,
              cantidad: item.quantity
            }))
          );
      
          Promise.all(productRequests)
            .then(productosCarrito => {
              this.productosCarrito = productosCarrito;
              console.log('Productos cargados:', this.productosCarrito);
            })
            .catch(error => {
              console.error('Error al cargar productos del carrito:', error);
            });
        }
=======
    const carritoId = Number(localStorage.getItem('carritoId'));
    this.carritoService.getProductosCarrito()
      .then(result => {
        if (result.success) {
          const carritoId = Number(localStorage.getItem('carritoId'));
          if (carritoId) {
            this.productosCarrito = result.data.filter(producto => producto.carritoId === carritoId);
            console.log('Productos cargados para carritoId:', carritoId, this.productosCarrito);
          } else {
            console.error('No se encontró carritoId en localStorage.');
          }
        } else {
          console.error('Error al obtener productos del carrito:', result.error);
        }
      })
      .catch(error => {
        console.error('Error al cargar productos del carrito:', error);
      });
>>>>>>> origin/Fitin
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
  

  // Eliminar un producto del carrito
  async removeProduct(productId: number, carritoId: number): Promise<void> {
    try {
      const result = await this.carritoService.removeProductFromCart(productId, carritoId);
      if (result.success) {
        this.productosCarrito = this.productosCarrito.filter(p => p.productoId !== productId);
      } else {
        this.errorMessage =` Error al eliminar el producto: ${result.error}`;
      }
    } catch (error) {
      this.errorMessage =` Se produjo un error: ${error.message}`;
    }
  }

  // Modificar la cantidad de un producto
  modifyQuantity(productId: number, carritoId: number, event: Event): void {
    const input = event.target as HTMLInputElement; // Hacemos el casting a HTMLInputElement
    const quantity = parseInt(input.value, 10); // Ahora accedemos a la propiedad value
  
    if (quantity < 1) {
      this.errorMessage = 'La cantidad debe ser al menos 1.';
      return;
    }
  
    this.isLoading = true;
    this.errorMessage = '';
    this.carritoService.modifyProductQuantity(productId, carritoId, quantity)
      .then((result) => {
        if (result.success) {
          // Encuentra el producto en el carrito y actualiza la cantidad
          const productoCarrito = this.productosCarrito.find(item => item.productoId === productId && item.carritoId === carritoId);
          if (productoCarrito) {
            productoCarrito.cantidad = quantity;  // Actualiza la cantidad en el array
          }
        } else {
          this.errorMessage =` Error al actualizar la cantidad: ${result.error}`;
        }
      })
      .catch((error) => {
        this.errorMessage = `Se produjo un error al actualizar la cantidad: ${error.message}`;
      })
      .finally(() => {
        this.isLoading = false;
      });
  }
  
}