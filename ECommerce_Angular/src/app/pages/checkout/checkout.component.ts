import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { CheckoutService } from '../../services/checkout.service';
import { Carrito } from '../../models/carrito';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {

  @ViewChild('checkoutDialog') checkoutDialogRef!: ElementRef<HTMLDialogElement>;

  carrito: Carrito | null = null;
  sessionId: string = '';
  routeQueryMap$!: Subscription;
  orderId!: string;
  productos: any[] = [];
  total: number = 0;

  constructor(
    private service: CheckoutService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      this.orderId = params.get('orderId') || '';
      const metodoPago = params.get('metodoPago');

      if (this.orderId) {
        this.loadOrderDetails(this.orderId);
      }

      if (metodoPago === 'tarjeta') {
        this.iniciarPagoConTarjeta();
      }
    });
  }

  iniciarPagoConTarjeta() {
    const productos = this.productos.map(p => ({
      Nombre: p.nombre,
      Precio: p.precio,
      Ruta: p.ruta
    }));

    this.service.getHostedCheckout(productos).then(response => {
      if (response.success && response.data.sessionUrl) {
        window.location.href = response.data.sessionUrl;
      } else {
        console.error('Error iniciando el pago:', response.error);
      }
    }).catch(err => {
      console.error('Error en la solicitud:', err);
    });
  }

  loadOrderDetails(orderId: string) {
    this.service.getOrderDetails(orderId).subscribe(
      (data: any) => {
        this.productos = data.Productos;
        this.total = data.Total;
      },
      error => {
        console.error('Error al cargar detalles del pedido:', error);
      }
    );
  }
}
