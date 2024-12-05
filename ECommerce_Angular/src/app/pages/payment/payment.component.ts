import { Component, inject, signal, ViewChild } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, Validators } from '@angular/forms';
import { StripeElementsOptions, StripePaymentElementOptions } from '@stripe/stripe-js';
import { NgxStripeModule, StripePaymentElementComponent, injectStripe } from 'ngx-stripe';
import { CheckoutService } from '../../services/checkout.service';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [ReactiveFormsModule, StripePaymentElementComponent, CommonModule, NgxStripeModule],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {
  @ViewChild(StripePaymentElementComponent)
  paymentElement!: StripePaymentElementComponent;

  private readonly fb = inject(UntypedFormBuilder);
  private readonly checkoutService = inject(CheckoutService);

  paymentElementForm = this.fb.group({
    name: ['John Doe', [Validators.required]],
    email: ['support@ngx-stripe.dev', [Validators.required]],
    address: [''],
    zipcode: [''],
    city: [''],
    amount: [2500, [Validators.required, Validators.pattern(/\d+/)]]
  });

  elementsOptions: StripeElementsOptions = {
    locale: 'en',
    clientSecret: '', 
    appearance: {
      theme: 'flat'
    }
  };

  paymentElementOptions: StripePaymentElementOptions = {
    layout: {
      type: 'tabs',
      defaultCollapsed: false,
      radios: false,
      spacedAccordionItems: false
    }
  };

  stripe = injectStripe('pk_test_Dt4ZBIt...');
  paying = signal(false);

  ngOnInit() {
    
    this.checkoutService.getCreateCheckoutSession().subscribe({
      next: (response) => {
        this.elementsOptions.clientSecret = response.clientSecret;
        console.log('Client Secret received:', response.clientSecret);
      },
      error: (err) => {
        console.error('Error retrieving client secret:', err);
      }
    });
  }

  pay() {
    if (this.paying() || this.paymentElementForm.invalid) return;
    this.paying.set(true);

    const { name, email, address, zipcode, city } = this.paymentElementForm.getRawValue();

    this.stripe
      .confirmPayment({
        elements: this.paymentElement.elements,
        confirmParams: {
          payment_method_data: {
            billing_details: {
              name: name as string,
              email: email as string,
              address: {
                line1: address as string,
                postal_code: zipcode as string,
                city: city as string
              }
            }
          }
        },
        redirect: 'if_required'
      })
      .subscribe({
        next: (result) => {
          this.paying.set(false);
          if (result.error) {
          
            alert({ success: false, error: result.error.message });
          } else if (result.paymentIntent.status === 'succeeded') {
            
            alert({ success: true });
          }
        },
        error: (err) => {
          console.error('Error confirming payment:', err);
          this.paying.set(false);
        }
      });
  }
}
