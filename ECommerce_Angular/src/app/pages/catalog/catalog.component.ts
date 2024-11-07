import { Component, OnInit } from '@angular/core';
import { CatalogoService } from '../../services/catalogo.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
  constructor(private catalogoService: CatalogoService) {}

  ngOnInit(): void {
    this.getProducts();
  }

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

  searchProducts(): void {
    this.paginaActual = 1; // Reinicia a la primera página para la nueva búsqueda
    this.getProducts();
  }

  cambiarPagina(direccion: number): void {
    this.paginaActual = Math.min(Math.max(1, this.paginaActual + direccion), this.totalPaginas);
    this.getProducts();
  }
}