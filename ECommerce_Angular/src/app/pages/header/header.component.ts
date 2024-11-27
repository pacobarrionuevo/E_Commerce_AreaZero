import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ImageService } from '../../services/image.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  logoImgSrc: string;
  carritoImgSrc: string;
  trofeosImgSrc: string;
  subscripcionImgSrc: string;

  constructor(private imageService: ImageService) {
    this.logoImgSrc = this.imageService.getImageUrl('logo.png');
    this.carritoImgSrc = this.imageService.getImageUrl('carrito.png');
    this.trofeosImgSrc = this.imageService.getImageUrl('trofeo.png');
    this.subscripcionImgSrc = this.imageService.getImageUrl('ticket.png');
  }
}