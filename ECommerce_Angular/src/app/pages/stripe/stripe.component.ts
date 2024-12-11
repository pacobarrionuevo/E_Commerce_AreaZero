import { Component, OnInit } from '@angular/core';
import { StripeEmbeddedCheckout, StripeEmbeddedCheckoutOptions } from '@stripe/stripe-js';
import { StripeService } from 'ngx-stripe';
import { CheckoutService } from '../../services/checkout.service';
import { ParamMap, Router } from '@angular/router';
import { Result } from '../../models/result';
@Component({
  selector: 'app-stripe',
  imports: [],
  templateUrl: './stripe.component.html',
  styleUrl: './stripe.component.css'
})
export class StripeComponent implements OnInit {
  stripe: any;
  sessionId: string = '';
  stripeEmbedCheckout: any;
  service: CheckoutService;
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
  async init(queryMap: ParamMap) {
    this.sessionId = queryMap.get('ordenId');
    if (this.sessionId) {
      const request = await this.service.getStatus(this.sessionId);
      console.log(this.sessionId)
      if (request.success) {
        console.log(request.data);
      }else {
        console.log("request null");
      }
    }
  }

  constructor( private checkoutService: CheckoutService, private router: Router) {}
  
  async embeddedCheckout() {
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
  

