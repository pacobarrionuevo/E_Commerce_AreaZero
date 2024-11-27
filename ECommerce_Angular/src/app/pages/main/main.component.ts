import { Component } from '@angular/core';
import { ImageService } from '../../services/image.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent {
  sobreImgSrc: string;
  torneosImgSrc: string;
  chapaImgSrc: string;

  constructor(private imageService: ImageService) {
    this.sobreImgSrc = this.imageService.getImage('sobre.png');
    this.torneosImgSrc = this.imageService.getImage('torneos.png');
    this.chapaImgSrc = this.imageService.getImage('chapaplateada.png');
  }
}
