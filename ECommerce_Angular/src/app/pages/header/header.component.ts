import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ImageService } from '../../services/image.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  logoImgSrc: string;
  carritoImgSrc: string;
  torneosImgSrc: string;
  subscripcionImgSrc: string;
  isLoggedIn: boolean = false;

  constructor(private imageService: ImageService) {
    this.logoImgSrc = this.imageService.getImageUrl('logo.png');
    this.carritoImgSrc = this.imageService.getImageUrl('carrito.png');
    this.torneosImgSrc = this.imageService.getImageUrl('trofeo.png');
    this.subscripcionImgSrc = this.imageService.getImageUrl('ticket.png');
  }

  ngOnInit(): void {
    this.isLoggedIn = !!localStorage.getItem('accessToken');
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('usuarioId');
    this.isLoggedIn = false;
  }
}