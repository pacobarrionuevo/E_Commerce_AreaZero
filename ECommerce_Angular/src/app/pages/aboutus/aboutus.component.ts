import { Component } from '@angular/core';
import { ImageService } from '../../services/image.service';

@Component({
  selector: 'app-aboutus',
  standalone: true,
  templateUrl: './aboutus.component.html',
  styleUrls: ['./aboutus.component.css']
})
export class AboutusComponent {
  twitterImgSrc: string;
  instagramImgSrc: string;
  tiktokImgSrc: string;
  discordImgSrc: string;

  constructor(private imageService: ImageService) {
    this.twitterImgSrc = this.imageService.getImageUrl('twitter.png');
    this.instagramImgSrc = this.imageService.getImageUrl('instagram.png');
    this.tiktokImgSrc = this.imageService.getImageUrl('tiktok.png');
    this.discordImgSrc = this.imageService.getImageUrl('discord.png');
  }
}
