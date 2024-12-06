import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ImageService } from '../../services/image.service';

@Component({
  selector: 'app-main',
  standalone: true,
  imports: [RouterModule], 
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent {
  sobreImgSrc: string;
  torneosImgSrc: string;
  chapaImgSrc: string;

  constructor(private imageService: ImageService) {
    this.sobreImgSrc = this.imageService.getImageUrl('sobre.png');
    this.torneosImgSrc = this.imageService.getImageUrl('torneos.png');
    this.chapaImgSrc = this.imageService.getImageUrl('chapaplateada.png');
  }
}
