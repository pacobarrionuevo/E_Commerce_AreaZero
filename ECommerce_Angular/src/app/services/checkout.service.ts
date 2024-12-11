import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { CheckoutSession } from '../models/checkout-session';
import { CheckoutSessionStatus } from '../models/checkout-session-status';
import { Carrito } from '../models/carrito';
import { Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {

  constructor(private api: ApiService, private http: HttpClient) { }
  private Url = 'https://areazero.runasp.net/api';

  getAllProducts(): Promise<Result<Carrito[]>> {
    return this.api.get<Carrito[]>('ControladorCheckout/products');
  }


  // Recoger el checkout embedido
  async getEmbededCheckout(): Promise<Result<CheckoutSession>> {
    const token = localStorage.getItem('token');
    this.api.jwt = token;
    try {
      const result = await this.api.post<CheckoutSession>('ControladorCheckout/embedded');
        return result;
    } catch (err) {
      return Result.error<CheckoutSession>(500, 'Failed to fetch embedded checkout session.');
    }
  }

  // Obtener el estado de la orden
  getStatus(sessionUrl: string): Promise<Result<CheckoutSessionStatus>> { 
    return this.api.get<CheckoutSessionStatus>(`ControladorCheckout/status/${sessionUrl}`); 
  }

  // Mandar los productos para crear la orden temporal
  crearOrdenTemporal(): Observable<any> {
    const ordenId = localStorage.getItem('ordenId');
    const userId = localStorage.getItem('usuarioId');
    
    const localcart = JSON.parse(localStorage.getItem('cart') || '[]');
    const cart = localcart.map(item => ({
        ProductoId: item.productId,
        Cantidad: item.quantity
    }));
    
    const params: any = {};
    if (ordenId) {
        params.ordenId = ordenId;
    }
    if (userId) {
        params.userId = userId;
    }
    return this.http.post(`${this.Url}/ControladorCheckout/CrearOrdenTemporal`, cart, {
        headers: { 'Content-Type': 'application/json' },
        params: params
    }).pipe(
        tap(response => {
            if (response && response.ordenId) {
                localStorage.setItem('ordenId', response.ordenId);
            }
        })
    );
  }
}