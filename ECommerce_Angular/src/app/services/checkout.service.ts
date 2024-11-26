import { Injectable } from '@angular/core';
import { Product } from '../models/product';
import { Result } from '../models/result';
import { ApiService } from './api.service';
import { Checkout } from '../models/checkout';
import { CheckoutStatus } from '../models/checkout-status';



@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  constructor(private api: ApiService) { }

  getAllProducts(): Promise<Result<Product[]>> {
    return this.api.get<Product[]>('ControladorCheckout/products');
  }

  getHostedCheckout(): Promise<Result<Checkout>> {
    return this.api.get<Checkout>('ControladorCheckout/hosted');
  }

  getEmbededCheckout(): Promise<Result<Checkout>> {
    return this.api.get<Checkout>('ControladorCheckout/embedded');
  }

  getStatus(sessionId: string): Promise<Result<CheckoutStatus>> {
    return this.api.get<CheckoutStatus>(`ControladorCheckout/status/${sessionId}`);
  }
}