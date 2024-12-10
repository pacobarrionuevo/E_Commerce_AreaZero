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

  loadUserData(): void {
    this.user = this.authService.getUserDataFromToken();
    //  Aquí Verificamos los datos del usuario obtenidos del token
    console.log('Datos del usuario:', this.user); 
    if (!this.user) {
      this.user = {
        id: 0,
        name: 'Usuario Ejemplo',
        email: 'ejemplo@correo.com',
        address: '123 Calle Falsa'
      };
    }
  }

  updateUserData(): void {
    const updatedUser = {
      UsuarioId: this.user.id,
      Nombre: this.user.name,
      Email: this.user.email,
      Direccion: this.user.address
    };
    //  Aquí Verificamos los datos que se envían al backend
    console.log('Datos enviados al backend:', updatedUser); 
    this.authService.updateUserData(updatedUser).subscribe(response => {
      const newToken = response.StringToken;
      console.log('Nuevo Token:', newToken);
      localStorage.setItem('accessToken', newToken);
      alert('Datos actualizados correctamente');
    }, error => {
      console.error('Error al actualizar los datos del usuario', error);
    });
  }
}