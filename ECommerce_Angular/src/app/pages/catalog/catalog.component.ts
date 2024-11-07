import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Product } from '../../models/product';
import { CommonModule } from '@angular/common';
import { SmartSearchService } from '../../services/smart-search.service';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.css'
})
export class CatalogComponent implements OnInit {

  productList: Product[] = [];
  query: string = '';


    constructor(
      private apiService: ApiService,
      private smartSearchService: SmartSearchService
    ) {}

  ngOnInit(): void {
    this.getProducts();
  }
  async getProducts() {
    try {
      const result = await this.apiService.get<Product[]>('controladorproducto');
      
      if (result.success) {
        this.productList = result.data;
        console.log(this.productList);
      } else {
        console.error('Error al obtener productos:',);
      }
    } catch (error) {
      console.error('Error en la solicitud:', error);
    }
  }

  async searchProducts() {
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
}