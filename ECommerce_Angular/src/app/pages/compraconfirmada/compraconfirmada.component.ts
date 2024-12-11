import { Component } from '@angular/core';
import { Carrito } from '../../models/carrito';
import { AuthService } from '../../services/auth.service';
import { CarritoService } from '../../services/carrito.service';
import { ProductoCarrito } from '../../models/producto-carrito';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-compraconfirmada',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './compraconfirmada.component.html',
  styleUrl: './compraconfirmada.component.css'
})
export class CompraconfirmadaComponent {
  productosCarrito: ProductoCarrito[] = [];


  constructor(private authService: AuthService, private carritoService: CarritoService) {
    this.cargarProductos();
  }


  async cargarProductos() {
    try {
      const result = await this.carritoService.getProductosCarrito();
      if (result.success) {
        this.productosCarrito = result.data;
      } else {
        console.error('Error al cargar productos:', result.error);
      }
    } catch (error) {
      console.error('Error en el servidor:', error);
    }
  }
}
