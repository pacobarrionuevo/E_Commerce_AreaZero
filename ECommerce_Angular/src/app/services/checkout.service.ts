import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Result } from '../models/result';
import { Product } from '../models/product';
import { CheckoutSession } from '../models/checkout-session';
import { CheckoutSessionStatus } from '../models/checkout-session-status';
import { Carrito } from '../models/carrito';
import { Observable, tap } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  crearOrdenTemporal() {
    throw new Error('Method not implemented.');
  }

  constructor(private api: ApiService, private http: HttpClient) { }
  private Url = 'https://localhost:7133/api';
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

  crearOrdenTemporal(): Observable<any> {
    // Recuperar el sessionId y usuarioId del localStorage
    const ordenId = localStorage.getItem('ordenId');
    const userId = localStorage.getItem('usuarioId');
    
    // Obtener el carrito desde localStorage y formatearlo
    const localcart = JSON.parse(localStorage.getItem('cart') || '[]');
    const cart = localcart.map(item => ({
        ProductoId: item.productId,
        Cantidad: item.quantity
    }));
    
    // Preparar los parámetros, añadiendo los dos si están presentes
    const params: any = {};
    if (ordenId) {
        params.ordenId = ordenId;
    }
    if (userId) {
        params.userId = userId;
    }

    // Enviar la solicitud HTTP con ambos parámetros si están disponibles
    return this.http.post(`${this.Url}/ControladorCheckout/CrearOrdenTemporal`, cart, {
        headers: { 'Content-Type': 'application/json' },
        params: params  // Pasamos los dos parámetros si están presentes
    }).pipe(
        // Aquí se guarda el sessionId (o OrdenId) devuelto por el backend
        tap(response => {
            if (response && response.ordenId) {
                localStorage.setItem('ordenId', response.ordenId); // Guardamos el nuevo sessionId
                console.log('Nuevo sessionId guardado en localStorage:', response.ordenId);
            }
        })
    );
}


    
  }
  
