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

  @ViewChild('checkoutDialog')
  checkoutDialogRef: ElementRef<HTMLDialogElement>;

  carrito: Carrito = null;
  sessionId: string = '';
  routeQueryMap$: Subscription;
  stripeEmbedCheckout: StripeEmbeddedCheckout;

  constructor(
    private service: CheckoutService, 
    private route: ActivatedRoute, 
    private router: Router,
    private stripe: StripeService) {}

   ngOnInit() {
    // El evento ngOnInit solo se llama una vez en toda la vida del componente.
    // Por tanto, para poder captar los cambios en la url nos suscribimos al queryParamMap del route.
    // Cada vez que se cambie la url se llamará al método onInit
    this.routeQueryMap$ = this.route.queryParamMap.subscribe(queryMap => this.init(queryMap));
  }

  ngOnDestroy(): void {
    // Cuando este componente se destruye hay que cancelar la suscripción.
    // Si no se cancela se seguirá llamando aunque el usuario no esté ya en esta página
    this.routeQueryMap$.unsubscribe();
  }

  async init(queryMap: ParamMap) {
    this.sessionId = queryMap.get('ordenId');

    if (this.sessionId) {
      const request = await this.service.getStatus(this.sessionId);

      if (request.success) {
        console.log(request.data);
      }
    } else {
      const request = await this.service.getAllProducts();

      if (request.success) {
        this.carrito = request.data[0];
      }
    }
  }

  async hostedCheckout() {
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
    
  
    const request = await this.service.getEmbededCheckout();
  
    if (request.success) {
      const options: StripeEmbeddedCheckoutOptions = {
        clientSecret: request.data.clientSecret
      };
  
      this.stripe.initEmbeddedCheckout(options)
        .subscribe((checkout) => {
          this.stripeEmbedCheckout = checkout;
          checkout.mount('#checkout');
          this.checkoutDialogRef.nativeElement.showModal();
        });
    } else {
      console.error('Error al iniciar el checkout incrustado.', request.error);
    }
  } 

  reload() {
    this.router.navigate(['checkout']);
  }

  cancelCheckoutDialog() {
    this.stripeEmbedCheckout.destroy();
    this.checkoutDialogRef.nativeElement.close();
  }
}