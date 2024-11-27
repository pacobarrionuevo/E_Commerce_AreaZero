import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private baseURL = 'https://localhost:7133/api/images'; // Cambia esto según tu configuración

  constructor(private http: HttpClient) {}

  getImage(imageName: string) {
    return `${this.baseURL}/${imageName}`;
  }
}
