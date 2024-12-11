import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

interface ProductoDto {
    id: number;
    nombre: string;
    ruta: string;
    precio: number;
    stock: number;
  }


interface Paginacion<T> {
  resultados: T[];
  numeroFilas: number;
  totalPaginas: number;
  elementosPorPagina: number;
  paginaActual: number;
}

@Injectable({
  providedIn: 'root'
})
export class CatalogoService {
  private apiUrl = 'https://localhost:7133/api/ControladorCatalogo'; 

  constructor(private http: HttpClient) {}

  // Recibe el catálogo entero con unas opciones predeterminadas
  getAll(
    Ordenacion: number = 2,
    paginaActual: number = 1,
    elementosPorPagina: number =20,
    query: string = '',
    totalPaginas: number=1
  ): Observable<Paginacion<ProductoDto>> {

    
    let params = new HttpParams()
     .set('filtro', Ordenacion)
      .set('paginaActual', paginaActual.toString())
      .set('elementosPorPagina', elementosPorPagina.toString());
    if (query) {
      params = params.set('query', query);
    }

    return this.http.get<Paginacion<ProductoDto>>(this.apiUrl, { params });
  }
}
