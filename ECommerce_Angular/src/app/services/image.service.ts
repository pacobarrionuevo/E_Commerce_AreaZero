import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private baseURL = 'https://localhost:7133/images';

  constructor() {}

  // Obtener la url de la imagen
  getImageUrl(imageName: string): string {
    return `${this.baseURL}/${imageName}`;
  }
}
