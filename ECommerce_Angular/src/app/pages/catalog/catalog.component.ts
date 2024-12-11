import { Component, OnInit } from '@angular/core';
import { CatalogoService } from '../../services/catalogo.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CarritoService } from '../../services/carrito.service';

@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
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
  userId: any;
  constructor(private catalogoService: CatalogoService, private carritoService: CarritoService) {}

  ngOnInit(): void {
    this.getProducts();
  }
//Recoger los productos del servidor
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
//Buscador
  searchProducts(): void {
    this.paginaActual = 1; 
    this.getProducts();
  }
//Paginación
  cambiarPagina(direccion: number): void {
    this.paginaActual = Math.min(Math.max(1, this.paginaActual + direccion), this.totalPaginas);
    this.getProducts();
  }
}