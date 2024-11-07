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
  paginaActual: number = 1;
  elementosPorPagina: number = 10;
  filtroPrecio: string = 'Ascendente';
  filtroNombre: string = 'DeAaZ';
  totalPaginas: number = 0;

  constructor(private catalogoService: CatalogoService) {}

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.catalogoService.getAll(this.filtroPrecio, this.filtroNombre, this.paginaActual, this.elementosPorPagina, this.query)
      .subscribe({
        next: (result) => {
          this.productList = result.resultados;
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
    try {
      const result = await this.smartSearchService.search(this.query);
      if (result.success) {
        const searchResults = result.data;
        
        this.productList = searchResults.length
          ? this.productList.filter(product => searchResults.includes(product.nombre))
          : [];
      } else {
        console.error('Error al buscar productos');
      }
    } catch (error) {
      console.error('Error en la solicitud de búsqueda:', error);
    }
    console.log(this.productList);
  }

  cambiarPagina(direccion: number): void {
    this.paginaActual = Math.min(Math.max(1, this.paginaActual + direccion), this.totalPaginas);
    this.getProducts();
  }
}
