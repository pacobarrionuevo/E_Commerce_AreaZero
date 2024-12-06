import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms'; 
import { CarritoService } from '../../services/carrito.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email: string = ''; 
  password: string = ''; 
  jwt: string | null = ''; 
  usuarioId: number | null = null;

  constructor(private authService: AuthService, private carritoService: CarritoService) {}

  async submit() {
    const authData = { email: this.email, password: this.password }; 

    try {
      const result = await this.authService.login(authData).toPromise();
      console.log('Resultado de login:', result);  // Verifica la respuesta del login 

      if (result) {
        // Guarda el token y el ID del usuario en el localStorage
        localStorage.setItem('accessToken', result.stringToken); 
        localStorage.setItem('usuarioId', result.usuarioId.toString()); 

        this.jwt = result.stringToken;
        this.usuarioId = result.usuarioId;

        console.log("Inicio de sesión exitoso.");
        this.carritoService.localtoCart();
        localStorage.removeItem('cart');
      } else {
        console.error("No se recibió un token de acceso.");
      }
    } catch (error) {
      console.error("Error al iniciar sesión:", error);
    }
  }
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('usuarioId');
    this.jwt = null;
    this.usuarioId = null;
    console.log("Cierre de sesión exitoso.");
  }
  
}
