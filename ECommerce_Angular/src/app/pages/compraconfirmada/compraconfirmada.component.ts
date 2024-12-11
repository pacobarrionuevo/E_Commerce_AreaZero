import { Component } from '@angular/core';
import { Carrito } from '../../models/carrito';
import { AuthService } from '../../services/auth.service';
import { CarritoService } from '../../services/carrito.service';

@Component({
  selector: 'app-compraconfirmada',
  imports: [],
  templateUrl: './compraconfirmada.component.html',
  styleUrl: './compraconfirmada.component.css'
})
export class CompraconfirmadaComponent {
  productos: Carrito[] = [];


  constructor(private authService: AuthService, private carritoService: CarritoService) {
    this.obtenerNombreUsuario();
    this.cargarProductos();
  }

  async obtenerNombreUsuario() {
    this.name = await this.authService.getNameUser();
  }

  cargarProductos() {
    this.carritoService.getProductosCarrito().subscribe((productos: CarritoEntero[]) => {
      this.productos = productos;
    });
  }
}
