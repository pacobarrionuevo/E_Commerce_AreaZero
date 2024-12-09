import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ImageService } from '../../services/image.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

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
  torneosImgSrc: string;
  subscripcionImgSrc: string;

  constructor(private imageService: ImageService, private authService: AuthService) {
    this.logoImgSrc = this.imageService.getImageUrl('logo.png');
    this.carritoImgSrc = this.imageService.getImageUrl('carrito.png');
    this.torneosImgSrc = this.imageService.getImageUrl('trofeo.png');
    this.subscripcionImgSrc = this.imageService.getImageUrl('ticket.png');
  }

  ngOnInit(): void {
    this.authService.isLoggedIn.subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
