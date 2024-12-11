import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

interface Review {
  id: number;
  fechaPublicacion: Date;
  textReview: string;
  label: number;
  usuarioId: number;
  productoId: number;
}

interface CreateReviewDto {
  textReview: string;
  usuarioId: number;
  productoId: number;
}

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private apiUrl = `${environment.apiUrl}ControladorReview`;

  constructor(private http: HttpClient) {}

  // Mandar el token en el header de la petición
  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('accessToken');
    let headers = new HttpHeaders();
    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);
    }
    return headers;
  }

  // Obtener todas las reviews
  getAllReviews(): Observable<Review[]> {
    return this.http.get<Review[]>(this.apiUrl, { headers: this.getAuthHeaders() });
  }

  // Añadir una review nueva
  addReview(reviewDto: CreateReviewDto): Observable<Review> {
    return this.http.post<Review>(this.apiUrl, reviewDto, { headers: this.getAuthHeaders() });
  }
}