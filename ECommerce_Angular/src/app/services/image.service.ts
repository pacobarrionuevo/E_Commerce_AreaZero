import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  private baseURL = 'https://localhost:7133/images';

  constructor() {}

  getImageUrl(imageName: string): string {
    return `${this.baseURL}/${imageName}`;
  }
}
