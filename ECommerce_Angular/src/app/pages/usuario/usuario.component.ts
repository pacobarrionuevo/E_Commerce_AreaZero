import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-usuario',
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
//carga los datos del usuario
  loadUserData(): void {
    this.user = this.authService.getUserDataFromToken();
    if (!this.user) {
      this.user = {
        id: 0,
        name: 'Usuario',
        email: 'correo@correo.com',
        address: '123 Alan Turing'
      };
    }
  }
//actualiza los datos del usuario
  updateUserData(): void {
    const updatedUser = {
      UsuarioId: this.user.id,
      Nombre: this.user.name,
      Email: this.user.email,
      Direccion: this.user.address
    };
    
    this.authService.updateUserData(updatedUser).subscribe(response => {
      const newToken = response.StringToken;
      localStorage.setItem('accessToken', newToken);
      alert('Datos actualizados correctamente');
    }, error => {
      console.error('Error al actualizar los datos del usuario', error);
    });
  }
}
