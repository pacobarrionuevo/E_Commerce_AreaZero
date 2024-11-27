import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { Product } from '../models/product';
import { CheckoutSession } from '../models/checkout-session';
import { CheckoutSessionStatus } from '../models/checkout-session-status';
import { Carrito } from '../models/carrito';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  constructor(private api: ApiService) { }

  getAllProducts(): Promise<Result<Carrito[]>> {
    return this.api.get<Carrito[]>('/ControladorCheckout/products');
  }

  getHostedCheckout(products: any): Promise<Result<CheckoutSession>> {
    return this.api.get<CheckoutSession>('/controladorcheckout/hosted', products);
  }

  getEmbededCheckout(products: any): Promise<Result<CheckoutSession>> {
    return this.api.get<CheckoutSession>('/controladorcheckout/embedded', products);
  }

  getStatus(sessionId: string): Promise<Result<CheckoutSessionStatus>> {
    return this.api.get<CheckoutSessionStatus>(`checkout/status/${sessionId}`);
  }
}
