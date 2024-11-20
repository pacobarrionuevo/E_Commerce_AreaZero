import { Component, OnInit } from '@angular/core';
import { CatalogoService } from '../../services/catalogo.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CarritoService } from '../../services/carrito.service';
import { ApiService } from '../../services/api.service'; // Asegúrate de importar el ApiService
import { Result } from '../../models/result';
import { AuthService } from '../../services/auth.service';
@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.css']
})
export class CatalogComponent implements OnInit {
  productList: any[] = [];
  query: string = '';
  paginaActual: number;
  Ordenacion: number = 2;
  elementosPorPagina: number = 20;
  totalPaginas: number;
  
  // Agregar apiService aquí
  constructor(private catalogoService: CatalogoService, private carritoService: CarritoService, private apiService: ApiService) {}

  ngOnInit(): void {
    this.getProducts();
  }
  addProductToCart(productId: number, userId: number,quantity: number): void {
    this.carritoService.addProductToCart(productId, userId, quantity)
      .then(result => {
        console.log('Producto añadido al carrito', result);
      })
      .catch(error => {
        console.error('Error al añadir producto al carrito', error);
      });
  }

  // Obtener los productos de la API
  getProducts(): void {
    this.catalogoService.getAll(this.Ordenacion, this.paginaActual, this.elementosPorPagina, this.query, this.totalPaginas)
      .subscribe({
        next: (result) => {
          this.productList = result.resultados;
          this.elementosPorPagina = result.elementosPorPagina;
          this.paginaActual = result.paginaActual;
          this.totalPaginas = result.totalPaginas;
        },
        error: (error) => {
          console.error('Error al obtener productos:', error);
        }
      });
  }

  // Añadir producto al carrito
  
  // Método de búsqueda
  searchProducts(): void {
    this.paginaActual = 1; // Reinicia a la primera página para la nueva búsqueda
    this.getProducts();
  }

  // Cambiar de página para la paginación
  cambiarPagina(direccion: number): void {
    this.paginaActual = Math.min(Math.max(1, this.paginaActual + direccion), this.totalPaginas);
    this.getProducts();
  }
}
