import { Injectable } from '@angular/core';
<<<<<<< HEAD
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { Product } from '../models/product';
import { CheckoutSession } from '../models/checkout-session';
import { CheckoutSessionStatus } from '../models/checkout-session-status';
import { Carrito } from '../models/carrito';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
=======
import { Product } from '../models/product';
import { Result } from '../models/result';
import { ApiService } from './api.service';
import { Checkout } from '../models/checkout';
import { CheckoutStatus } from '../models/checkout-status';


>>>>>>> origin/Fitin

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

<<<<<<< HEAD
  constructor(private api: ApiService, private http: HttpClient) { }
  private Url = 'https://localhost:7133';
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

  getCreateCheckoutSession(): Observable<CheckoutSession> {
    return this.http.post<CheckoutSession>(this.Url, {});
  }

=======
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
>>>>>>> origin/Fitin
}