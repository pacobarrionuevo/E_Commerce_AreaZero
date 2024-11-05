import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { Product } from '../../models/product';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.css'
})
export class CatalogComponent implements OnInit {

  productList: Product[] = [];


  constructor(private apiService: ApiService) {}

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
}