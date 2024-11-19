import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductDetailService } from '../../services/product-detail.service';
import { Product } from '../../models/product';  
import { CommonModule } from '@angular/common'; 

@Component({
  selector: 'app-product-detail',
  standalone: true,  
  imports: [CommonModule], 
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  product: Product | undefined;

  constructor(
    private route: ActivatedRoute,
    private productDetailService: ProductDetailService
  ) {}

  ngOnInit(): void {
    this.getProduct();
  }

  getProduct(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    console.log('Producto ID:', id); // Verifica el ID del producto
    if (id) {
      this.productDetailService.getProducto(id).subscribe((product: Product) => {
        console.log('Producto obtenido:', product); // Verifica el producto obtenido
        this.product = product;
      }, error => {
        console.error('Error al obtener el producto:', error); 
      });
    }
  }
}