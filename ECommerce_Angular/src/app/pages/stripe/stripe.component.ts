import { Component, OnInit } from '@angular/core';
import { StripeEmbeddedCheckout, StripeEmbeddedCheckoutOptions } from '@stripe/stripe-js';
import { StripeService } from 'ngx-stripe';
import { CheckoutService } from '../../services/checkout.service';
import { Router } from '@angular/router';
import { Result } from '../../models/result';
@Component({
  selector: 'app-stripe',
  imports: [],
  templateUrl: './stripe.component.html',
  styleUrl: './stripe.component.css'
})
export class StripeComponent implements OnInit {
  stripe: any;
  stripeEmbedCheckout: any;
  checkoutDialogRef: any;
  intervalId: NodeJS.Timeout;
  routeQueryMap$: any;
  route: any;
  ngOnInit() {
    this.intervalId = setInterval(() => {
      this.routeQueryMap$ = this.route.queryParamMap.subscribe(queryMap => this.init(queryMap));
    }, 5000);
    this.embeddedCheckout()
  }
  init(queryMap: any) {
    throw new Error('Method not implemented.');
  }

  constructor( private checkoutService: CheckoutService, private router: Router) {}
  
  async embeddedCheckout(): Promise<void> {
    this.checkoutService.getEmbededCheckout()
      .then(result => {
        if (result.success) {
          const options: StripeEmbeddedCheckoutOptions = {
            clientSecret: result.data.clientSecret,
            onComplete: () => this.confirmacion()
          };

          this.stripe.initEmbeddedCheckout(options)
            .subscribe((checkout) => {
              this.stripeEmbedCheckout = checkout;
              checkout.mount('#checkout');
              this.checkoutDialogRef.nativeElement.showModal();
            });
        } else {
          console.error('Error al obtener el checkout embebido:', result.error);
        }
      })
      .catch(error => {
        console.error('Error al inicializar el checkout embebido:', error);
      });
  }
  confirmacion(){
    this.router.navigateByUrl("confirmacion")
  }

    }
  

