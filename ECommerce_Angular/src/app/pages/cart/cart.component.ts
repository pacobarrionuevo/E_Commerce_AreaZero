import { Component, OnInit } from '@angular/core';
import { CarritoService } from '../../services/carrito.service';
import { ProductoCarrito } from '../../models/producto-carrito';
import { Product } from '../../models/product';
import { Result } from '../../models/result';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { CheckoutService } from '../../services/checkout.service';
import { ImageService } from '../../services/image.service'; // Añadimos el servicio de imágenes

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

  constructor(
    private carritoService: CarritoService,
    private checkoutService: CheckoutService,
    private router: Router,
    private imageService: ImageService 
  ) {}

  ngOnInit(): void {
    //Guardo el userid en una variable del local storage
    const userIdString = localStorage.getItem('usuarioId');
    this.userId = userIdString ? parseInt(userIdString, 10) : null;
    //cargar los productos
    this.loadCartProducts();
  }

  //boton para ir al pago y crear la orden temporal para reservar el stock
  async goToPagoTarjeta() {
    this.checkoutService.crearOrdenTemporal().subscribe({
      next: async (response) => {
        console.log('Orden temporal creada o actualizada correctamente:', response);
        const stripeSession = await this.checkoutService.getEmbededCheckout();
        if (stripeSession.success) {
          const sessionUrl = stripeSession.data.sessionUrl;
          this.router.navigate(['checkout'], {
            queryParams: { 'sessionUrl': sessionUrl, 'metodo_pago': 'stripe' }
          });
        } 
      },
    });
  }

  //Cargar los productos, si no hay userid los carga del localstorage y si hay userid los carga del servidor 
  loadCartProducts(): void {
    this.productosCarrito = []; 

    if (this.userId) {
      this.carritoService.getProductosCarrito()
        .then(result => {
          if (result.success) {
            this.productosCarrito = result.data.map((item: ProductoCarrito) => {
              return {...item, producto: {...item.producto, ruta: this.imageService.getImageUrl(item.producto.ruta) }
              };
            });
          } 
        })
        .catch(error => {
          console.error('Error en el servidor:', error);
        })
        .finally(() => {
          this.isLoading = false;
        });
    } else {
      const localCart = JSON.parse(localStorage.getItem('cart') || '[]');
      const productRequests = localCart.map((item: { productId: number, quantity: number }) =>
        this.carritoService.getProductById(item.productId).then((result: Result<Product>) => ({
          producto: {...result.data, ruta: this.imageService.getImageUrl(result.data.ruta) }, cantidad: item.quantity}))
      );

      Promise.all(productRequests)
        .then(productos => {
          this.productosCarrito = productos;
        })
        .catch(error => {
          console.error('Error al cargar productos locales:', error);
        })
        .finally(() => {
          this.isLoading = false;
        });
    }
  }

  //Borrar productos del local storage o del servidor, también dependiendo del userid guardado
  async removeProduct(productId: number): Promise<void> {
    if (!this.userId) {
      const existingCart = localStorage.getItem('cart');
      let cart: { productId: number, quantity: number }[] = JSON.parse(existingCart);
      const ProductIdCart = cart.findIndex(item => item.productId === productId);
      cart.splice(ProductIdCart, 1);
      localStorage.setItem('cart', JSON.stringify(cart));
      this.loadCartProducts();
    } else {
      try {
        const result = await this.carritoService.removeProductFromCart(productId, this.userId);
        this.productosCarrito = this.productosCarrito.filter(p => p.productoId !== productId);
        console.log(`Producto con ID ${productId} eliminado del servidor`);
      } catch (error) {
        this.errorMessage = `Se produjo un error: ${error.message}`;
      }
    }
    return;
  }

  modifyQuantity(productId: number, carritoId: number, event: Event): void {
    const input = event.target as HTMLInputElement;
    const quantity = parseInt(input.value, 10); 
    
    if (!this.userId) {
      const localCart = JSON.parse(localStorage.getItem('cart') || '[]');
      const productIdCart = localCart.findIndex((item: { productId: number }) => item.productId === productId);

      if (productIdCart === -1) {
        console.error(`Producto con ID ${productId} no encontrado en el carrito.`);
        return;
      }

      localCart[productIdCart].quantity = quantity;
      localStorage.setItem('cart', JSON.stringify(localCart));
    } else {
      this.carritoService.modifyProductQuantity(productId, carritoId, quantity)
      .then((result) => {
        if (result.success) {
          const productoCarrito = this.productosCarrito.find(item => item.productoId === productId && item.carritoId === carritoId);
          if (productoCarrito) {
            productoCarrito.cantidad = quantity;  
          }
        } 
      })
      .catch((error) => {
        this.errorMessage = `Se produjo un error al actualizar la cantidad: ${error.message}`;
      })
      .finally(() => {
        this.isLoading = false;
      });
    }
}}