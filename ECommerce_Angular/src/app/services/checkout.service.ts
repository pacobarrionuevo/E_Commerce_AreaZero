import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { Product } from '../models/product';
import { CheckoutSession } from '../models/checkout-session';
import { CheckoutSessionStatus } from '../models/checkout-session-status';
import { Carrito } from '../models/carrito';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  crearOrdenTemporal() {
    throw new Error('Method not implemented.');
  }

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

}