import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-usuario',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit {
  user: any = {};

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.loadUserData();
  }
//esto hay que cambiarlo porque no esta bien planteao
  loadUserData(): void {
    this.user = this.authService.getUserDataFromToken();
    console.log('Datos del usuario:', this.user); 
    if (!this.user) {
      this.user = {
        name: 'Usuario Ejemplo',
        email: 'ejemplo@correo.com',
        address: '123 Calle Falsa'
      };
    }
  }

  updateUserData(): void {
    alert('Datos actualizados correctamente (simulado)');
  }
}
