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
  addProductToCart(productId: number, quantity: number): void {
    if (!this.userId) {
      // Recuperar la colección existente del localStorage
      const existingCart = localStorage.getItem('cart');
      let cart: { productId: number, quantity: number }[] = [];
  
      if (existingCart) {
        // Parsear el contenido existente si lo hay
        cart = JSON.parse(existingCart);
      }
  
      // Verificar si el producto ya está en el carrito
      const existingProductIndex = cart.findIndex(item => item.productId === productId);
  
      if (existingProductIndex !== -1) {
        // Si ya existe, actualizar la cantidad
        cart[existingProductIndex].quantity += quantity;
      } else {
        // Si no existe, agregar el nuevo producto al carrito
        cart.push({ productId, quantity });
      }
  
      // Guardar el carrito actualizado en el localStorage
      localStorage.setItem('cart', JSON.stringify(cart));
    }
  
    // También puedes llamar al servicio para manejar carritos en el servidor
    if (this.userId!=null){
      this.carritoService.addProductToCart(productId, this.userId, quantity)
      .then(result => {
        console.log('Producto añadido al carrito', result);
      })
      .catch(error => {
        console.error('Error al añadir producto al carrito', error);
      });
    }
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
    this.paginaActual = 1; 
    this.getProducts();
  }

  cambiarPagina(direccion: number): void {
    this.paginaActual = Math.min(Math.max(1, this.paginaActual + direccion), this.totalPaginas);
    this.getProducts();
  }
}