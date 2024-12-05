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

  @ViewChild('checkoutDialog')
  checkoutDialogRef: ElementRef<HTMLDialogElement>;

  productosCarrito: ProductoCarrito[] = [];
  product: Product = null;
  sessionId: string = '';
  routeQueryMap$: Subscription;
  stripeEmbedCheckout: StripeEmbeddedCheckout;

  constructor(
    private service: CheckoutService, 
    private route: ActivatedRoute, 
    private router: Router,
    private stripe: StripeService,
    private carritoService: CarritoService) {}

   ngOnInit() {
    // El evento ngOnInit solo se llama una vez en toda la vida del componente.
    // Por tanto, para poder captar los cambios en la url nos suscribimos al queryParamMap del route.
    // Cada vez que se cambie la url se llamará al método onInit
    this.routeQueryMap$ = this.route.queryParamMap.subscribe(queryMap => this.init(queryMap));
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
  }

  ngOnDestroy(): void {
    // Cuando este componente se destruye hay que cancelar la suscripción.
    // Si no se cancela se seguirá llamando aunque el usuario no esté ya en esta página
    this.routeQueryMap$.unsubscribe();
  }

  

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
        this.product = request.data[0];
      }
    }
  }

  async hostedCheckout() {
    const request = await this.service.getHostedCheckout();

    if (request.success) {
      // Abrimos la url de la session de stripe sin crear una nueva pestaña en el navegador 
      window.open(request.data.sessionUrl, '_self');
    }
  }

  async embeddedCheckout() {
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