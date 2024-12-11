import { Component, OnInit, OnDestroy, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, ParamMap, Router, RouterModule } from '@angular/router';
import { Subscription } from 'rxjs';
import { CheckoutService } from '../../services/checkout.service';
import { StripeService } from 'ngx-stripe';
import { StripeEmbeddedCheckout, StripeEmbeddedCheckoutOptions } from '@stripe/stripe-js';
import { Carrito } from '../../models/carrito';
import { StripeComponent } from '../stripe/stripe.component';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [RouterModule, StripeComponent],
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit, OnDestroy {

  @ViewChild('checkoutDialog')
  checkoutDialogRef: ElementRef<HTMLDialogElement>;

  carrito: Carrito = null;
  sessionUrl: string = '';
  routeQueryMap$: Subscription;
  stripeEmbedCheckout: StripeEmbeddedCheckout;

  constructor(
    private service: CheckoutService, 
    private route: ActivatedRoute, 
    private router: Router,
    private stripe: StripeService) {}

  ngOnInit() {
    this.routeQueryMap$ = this.route.queryParamMap.subscribe(queryMap => this.init(queryMap));
  }

  ngOnDestroy(): void {
    this.routeQueryMap$.unsubscribe();
  }

  async init(queryMap: ParamMap) {
    this.sessionUrl = queryMap.get('sessionUrl');
    
    if (this.sessionUrl) {
      const request = await this.service.getStatus(this.sessionUrl);

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
