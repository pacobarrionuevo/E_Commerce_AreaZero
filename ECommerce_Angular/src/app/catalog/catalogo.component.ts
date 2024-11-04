import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Products } from '../models/products';
import { ApiService } from '../services/api.service';
@Component({
  selector: 'app-catalog',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './catalog.component.html',
  styleUrl: './catalog.component.css'
})
export class CatalogComponent implements OnInit {

  productList: Products[] = [];


  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.getProducts();
  }
  async getProducts() {
    try {
      const result = await this.apiService.get<Products[]>('/controladorproducto');
      
      if (result.success) {
        this.productList = result.data || [];  // Asignar productos a la lista
        console.log(this.productList);
      } else {
        console.error('Error al obtener productos:',);
      }
    } catch (error) {
      console.error('Error en la solicitud:', error);
    }
  }
}