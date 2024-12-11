import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { loadStripe, Stripe, StripeElements } from '@stripe/stripe-js';
import { CheckoutService } from '../../services/checkout.service';
import { StripeEmbeddedCheckoutOptions } from '@stripe/stripe-js';
import { environment } from '../../../environments/environment';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-stripe',
  standalone: true,
  templateUrl: './stripe.component.html',
  styleUrls: ['./stripe.component.css'],
  imports: [CommonModule]
})
export class StripeComponent implements OnInit {
  @ViewChild('checkoutDialogRef', { static: false }) checkoutDialogRef: ElementRef<HTMLDialogElement>;
  intervalId: any;
  sessionUrl: string = '';
  stripe: Stripe | null = null;
  elements: StripeElements | null = null;
  stripeEmbedCheckout: any;

  constructor(
    private checkoutService: CheckoutService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  async ngOnInit() {
    this.stripe = await loadStripe(environment.stripePublicKey);

    if (this.stripe) {
      this.elements = this.stripe.elements();
    } else {
      console.error('Error al inicializar Stripe');
    }

    this.route.queryParamMap.subscribe(queryMap => this.init(queryMap));
    this.embeddedCheckout();
  }

  async init(queryMap: ParamMap) {
    this.sessionUrl = queryMap.get('sessionUrl')!;
    if (this.sessionUrl) {
      const request = await this.checkoutService.getStatus(this.sessionUrl);
      console.log(this.sessionUrl);
      if (request.success) {
        console.log(request.data);
      } else {
        console.log("request null");
      }
    }
  }

  async embeddedCheckout() {
    const result = await this.checkoutService.getEmbededCheckout();

    if (result.success) {
      console.log("Embedded checkout obtenido con éxito");

      const options: StripeEmbeddedCheckoutOptions = {
        clientSecret: result.data.clientSecret,
        onComplete: () => this.confirmacion() // Redirigir al confirmar
      };

      // Almacenar el sessionUrl obtenido del backend
      this.sessionUrl = result.data.sessionUrl;

      if (this.stripe) {
        const checkout = await this.stripe.initEmbeddedCheckout(options);
        this.stripeEmbedCheckout = checkout;
        if (this.checkoutDialogRef && this.checkoutDialogRef.nativeElement) {
          checkout.mount('#checkout');
          this.checkoutDialogRef.nativeElement.showModal(); // Mostrar el dialogo
        } else {
          console.error('checkoutDialogRef no está correctamente referenciado o no tiene nativeElement');
        }
      } else {
        console.error('El objeto stripe no está correctamente inicializado o no tiene el método initEmbeddedCheckout');
      }

    } else {
      console.error('Error al obtener el checkout embebido:', result.error);
    }
  }

  confirmacion() {
    this.router.navigateByUrl("compraconfirmada");
  }
}