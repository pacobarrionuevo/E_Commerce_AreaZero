import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductDetailService } from '../../services/product-detail.service';
import { ReviewService } from '../../services/review.service';
import { Product } from '../../models/product';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CarritoService } from '../../services/carrito.service';
import { AuthService } from '../../services/auth.service';

interface Review {
  id: number;
  fechaPublicacion: Date;
  textReview: string;
  label: number;
  usuarioId: number;
  productoId: number;
  usuarioNombre?: string;
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
  Media: number = 0;
  usuarioNombre: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private carritoService: CarritoService,
    private productDetailService: ProductDetailService,
    private reviewService: ReviewService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.jwt = localStorage.getItem('accessToken'); 
    this.usuarioId = Number(localStorage.getItem('usuarioId')); 
    this.loadUserData();
    this.getProduct();
  }

  addProductToCart(productId: number, quantity: number): void {
    if (!this.usuarioId) {
      const existingCart = localStorage.getItem('cart');
      let cart: { productId: number, quantity: number }[] = [];
      if (existingCart) {
        cart = JSON.parse(existingCart);
      }
      const existingProductIndex = cart.findIndex(item => item.productId === productId);
      if (existingProductIndex !== -1) {
        cart[existingProductIndex].quantity += quantity;
      } else {
        cart.push({ productId, quantity });
      }
      localStorage.setItem('cart', JSON.stringify(cart));
    }
  
    if (this.usuarioId != null){
      this.carritoService.addProductToCart(productId, this.usuarioId, quantity)
      .then(result => {
        console.log('Producto añadido al carrito', result);
      })
      .catch(error => {
        console.error('Error al añadir producto al carrito', error);
      });
    }
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
        this.calcularMedia();
      }, error => {
        console.error('Error al obtener las reseñas:', error);
      });
    }
  }

  addReview(): void {
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
        review.fechaPublicacion = new Date(review.fechaPublicacion);
        review.usuarioNombre = this.usuarioNombre;
        this.reviews.push(review);
        this.calcularMedia();
        this.newReview = '';
      }, error => {
        console.error('Error al añadir la reseña:', error);
      });
    }
  }

  calcularMedia(): void {
    if (this.reviews.length > 0) {
      const totalRating = this.reviews.reduce((total, review) => {
        let rating = 0;
        if (review.label === 1) {
          rating = 5;
        } else if (review.label === 0) {
          rating = 2.5;
        } else if (review.label === -1) {
          rating = 0;
        }
        return total + rating;
      }, 0);
      this.Media = totalRating / this.reviews.length;
    } else {
      this.Media = 0;
    }
  }

  reviewEmoji(label: number): string {
    if (label === 1) {
      return '😃';  
    } else if (label === 0) {
      return '😐';  
    } else if (label === -1) {
      return '🤮'; 
    } else {
      return '';
    }
  }

  emoji(): string {
    if (this.Media <= 1.66) {
      return '🤮';
    } else if (this.Media <= 3.33) {
      return '😐';
    } else {
      return '😃';
    }
  }

  loadUserData(): void {
    const user = this.authService.getUserDataFromToken();
    if (user) {
      this.usuarioNombre = user.name;
    }
  }
}
