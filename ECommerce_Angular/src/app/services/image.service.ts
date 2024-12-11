import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private baseURL = 'https://areazero.runasp.net/images';

  constructor() {}

  // Obtener la url de la imagen
  getImageUrl(imageName: string): string {
    return `${this.baseURL}/${imageName}`;
  }
}
