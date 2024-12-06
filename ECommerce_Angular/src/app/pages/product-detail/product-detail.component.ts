import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductDetailService } from '../../services/product-detail.service';
import { ReviewService } from '../../services/review.service';
import { Product } from '../../models/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CarritoService } from '../../services/carrito.service';

interface Review {
  id: number;
  fechaPublicacion: Date;
  textReview: string;
  label: number;
  usuarioId: number;
  productoId: number;
}

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css']
})
export class ProductDetailComponent implements OnInit {
  product: Product | undefined;
  reviews: Review[] = [];
  newReview: string = '';
  usuarioId: number | null = null;
  jwt: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private carritoService: CarritoService,
    private productDetailService: ProductDetailService,
    private reviewService: ReviewService
  ) {}

  ngOnInit(): void {
    // Obtenermos el token y el id desde el almacenamiento local
    this.jwt = localStorage.getItem('accessToken'); 
    this.usuarioId = Number(localStorage.getItem('usuarioId')); 
    // Mensaje en la consola para controlar si se ha iniciado sesion
    console.log('Usuario ID:', this.usuarioId); 
    this.getProduct();
  }

  addProductToCart(productId: number, userId: number, quantity: number): void {
    this.carritoService.addProductToCart(productId, userId, quantity)
      .then(result => {
        console.log('Producto añadido al carrito', result);
      })
      .catch(error => {
        console.error('Error al añadir producto al carrito', error);
      });
  }

  getProduct(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.productDetailService.getProducto(id).subscribe((product: Product) => {
        this.product = product;
        this.getReviews();
      }, error => {
        console.error('Error al obtener el producto:', error);
      });
    }
  }

  getReviews(): void {
    if (this.product) {
      this.reviewService.getAllReviews().subscribe((reviews: Review[]) => {
        this.reviews = reviews.filter(review => review.productoId === this.product!.id);
      }, error => {
        console.error('Error al obtener las reseñas:', error);
      });
    }
  }

  addReview(): void {
    // Reaseguramos que se obtiene el token y el id antes de añadir una reseña
    this.jwt = localStorage.getItem('accessToken'); 
    this.usuarioId = Number(localStorage.getItem('usuarioId')); 

    console.log('Verificando token antes de añadir la reseña:', this.jwt); // Mensaje de consola para saber si tenemos el token para poder reseñar

    if (!this.jwt) {
      alert('Por favor, inicie sesión para añadir una reseña.');
      return;
    }

    if (this.newReview.trim()) {
      const reviewDto = {
        textReview: this.newReview,
        usuarioId: this.usuarioId,
        productoId: this.product?.id || 0
      };
      this.reviewService.addReview(reviewDto).subscribe((review: Review) => {
        this.reviews.push(review);
        this.newReview = '';
      }, error => {
        console.error('Error al añadir la reseña:', error);
      });
    }
  }
}
