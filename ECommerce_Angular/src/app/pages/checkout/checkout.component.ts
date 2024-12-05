<<<<<<< HEAD
import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Product } from '../../models/product';
import { CheckoutService } from '../../services/checkout.service';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { StripeEmbeddedCheckout, StripeEmbeddedCheckoutOptions } from '@stripe/stripe-js';
import { StripeService } from 'ngx-stripe';
import { Carrito } from '../../models/carrito';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit, OnDestroy  {
=======
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router, ParamMap, RouterModule } from '@angular/router';
import { StripeEmbeddedCheckout, StripeEmbeddedCheckoutOptions } from '@stripe/stripe-js';
import { StripeService } from 'ngx-stripe';
import { Subscription } from 'rxjs';
import { Product } from '../../models/product';
import { CheckoutService } from '../../services/checkout.service';
import { ProductoCarrito } from '../../models/producto-carrito';
import { CarritoService } from '../../services/carrito.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent implements OnInit {
>>>>>>> origin/Fitin

  @ViewChild('checkoutDialog')
  checkoutDialogRef: ElementRef<HTMLDialogElement>;

<<<<<<< HEAD
  carrito: Carrito = null;
=======
  productosCarrito: ProductoCarrito[] = [];
  product: Product = null;
>>>>>>> origin/Fitin
  sessionId: string = '';
  routeQueryMap$: Subscription;
  stripeEmbedCheckout: StripeEmbeddedCheckout;

  constructor(
    private service: CheckoutService, 
    private route: ActivatedRoute, 
    private router: Router,
<<<<<<< HEAD
    private stripe: StripeService) {}
=======
    private stripe: StripeService,
    private carritoService: CarritoService) {}
>>>>>>> origin/Fitin

   ngOnInit() {
    // El evento ngOnInit solo se llama una vez en toda la vida del componente.
    // Por tanto, para poder captar los cambios en la url nos suscribimos al queryParamMap del route.
    // Cada vez que se cambie la url se llamará al método onInit
    this.routeQueryMap$ = this.route.queryParamMap.subscribe(queryMap => this.init(queryMap));
<<<<<<< HEAD
=======
    this.loadCheckoutProduct();
  }
  loadCheckoutProduct() {
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

  ngOnDestroy(): void {
    // Cuando este componente se destruye hay que cancelar la suscripción.
    // Si no se cancela se seguirá llamando aunque el usuario no esté ya en esta página
    this.routeQueryMap$.unsubscribe();
  }

<<<<<<< HEAD
=======
  

>>>>>>> origin/Fitin
  async init(queryMap: ParamMap) {
    this.sessionId = queryMap.get('session_id');

    if (this.sessionId) {
      const request = await this.service.getStatus(this.sessionId);

      if (request.success) {
        console.log(request.data);
      }
    } else {
      const request = await this.service.getAllProducts();

      if (request.success) {
<<<<<<< HEAD
        this.carrito = request.data[0];
=======
        this.product = request.data[0];
>>>>>>> origin/Fitin
      }
    }
  }

  async hostedCheckout() {
<<<<<<< HEAD
    if (!this.carrito || !this.carrito.productoCarrito) {
      console.error('El carrito está vacío.');
      return;
    }
  
    const productos = this.carrito.productoCarrito.map(p => ({
      Id: p.producto.id,
      Nombre: p.producto.nombre,
      Precio: p.producto.precio,
      Ruta: p.producto.ruta,
      Stock: p.producto.stock,
      Cantidad: p.cantidad,
    }));
  
    const request = await this.service.getHostedCheckout(productos);
  
    if (request.success) {
      window.open(request.data.sessionUrl, '_self');
    } else {
      console.error('Error al iniciar el checkout hospedado.', request.error);
    }
  }
  

  async embeddedCheckout() {
    if (!this.carrito || !this.carrito.productoCarrito) {
      console.error('El carrito está vacío.');
      return;
    }
  
    const productos = this.carrito.productoCarrito.map(p => ({
      Id: p.producto.id,
      Nombre: p.producto.nombre,
      Precio: p.producto.precio,
      Ruta: p.producto.ruta,
      Stock: p.producto.stock,
      Cantidad: p.cantidad,
    }));
    
  
    const request = await this.service.getEmbededCheckout(productos);
  
=======
    const request = await this.service.getHostedCheckout();

    if (request.success) {
      // Abrimos la url de la session de stripe sin crear una nueva pestaña en el navegador 
      window.open(request.data.sessionUrl, '_self');
    }
  }

  async embeddedCheckout() {
    const request = await this.service.getEmbededCheckout();

>>>>>>> origin/Fitin
    if (request.success) {
      const options: StripeEmbeddedCheckoutOptions = {
        clientSecret: request.data.clientSecret
      };
<<<<<<< HEAD
  
=======

>>>>>>> origin/Fitin
      this.stripe.initEmbeddedCheckout(options)
        .subscribe((checkout) => {
          this.stripeEmbedCheckout = checkout;
          checkout.mount('#checkout');
          this.checkoutDialogRef.nativeElement.showModal();
        });
<<<<<<< HEAD
    } else {
      console.error('Error al iniciar el checkout incrustado.', request.error);
    }
  } 
=======
      }
  }
>>>>>>> origin/Fitin

  reload() {
    this.router.navigate(['checkout']);
  }

  cancelCheckoutDialog() {
    this.stripeEmbedCheckout.destroy();
    this.checkoutDialogRef.nativeElement.close();
  }
}