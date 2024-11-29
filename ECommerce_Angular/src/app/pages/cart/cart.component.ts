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

    this.loadCartProducts();
  }

  // Cargar los productos del carrito
  loadCartProducts(): void {
  const localCart = JSON.parse(localStorage.getItem('cart') || '[]');
  
  this.productosCarrito = []; // Reiniciar la lista de productos
        if (!this.userId){
          const productRequests = localCart.map((item: { productId: number, quantity: number }) =>
            this.carritoService.getProductById(item.productId).then((result: Result<Product>) => ({
              producto: result.data, // Aquí accedemos a 'data' desde la instancia de Result
              cantidad: item.quantity
            }))
          );
      
          Promise.all(productRequests)
            .then(productosCarrito => {
              this.productosCarrito = productosCarrito;
              console.log('Estructura final de productosCarrito:', this.productosCarrito);
            console.log('Contenido de localCart:', localCart);
            console.log('Datos procesados de productosCarrito:', JSON.stringify(this.productosCarrito, null, 2));

            })
            .catch(error => {
              console.error('Error al cargar productos del carrito:', error);
            });
            
        }
  }
  


  // Eliminar un producto del carrito
  async removeProduct(productId: number, carritoId: number): Promise<void> {
    try {
      const result = await this.carritoService.removeProductFromCart(productId, carritoId);
      if (result.success) {
        this.productosCarrito = this.productosCarrito.filter(p => p.productoId !== productId);
      } else {
        this.errorMessage = `Error al eliminar el producto: ${result.error}`;
      }
    } catch (error) {
      this.errorMessage = `Se produjo un error: ${error.message}`;
    }
  }

  // Modificar la cantidad de un producto
  modifyQuantity(productId: number, carritoId: number, event: Event): void {
    const input = event.target as HTMLInputElement; // Hacemos el casting a HTMLInputElement
    const quantity = parseInt(input.value, 10); // Ahora accedemos a la propiedad `value`
  
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
          this.errorMessage = `Error al actualizar la cantidad: ${result.error}`;
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
