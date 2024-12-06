import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms'; 
<<<<<<< HEAD
import { CarritoService } from '../../services/carrito.service';
import { CommonModule } from '@angular/common';
=======
import { CommonModule } from '@angular/common'; 
>>>>>>> origin/salperro2

@Component({
  selector: 'app-login',
  standalone: true,
<<<<<<< HEAD
  imports: [FormsModule, CommonModule],
=======
  imports: [FormsModule, CommonModule], 
>>>>>>> origin/salperro2
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  email: string = ''; 
  password: string = ''; 
  jwt: string | null = ''; 
  usuarioId: number | null = null;

  constructor(private authService: AuthService, private carritoService: CarritoService) {}

  ngOnInit(): void {
    // Obtenemos el token desde el almacenamiento local en OnInit
    this.jwt = localStorage.getItem('accessToken'); 
    // Verificamos la existencia del token
    console.log('Token JWT en login:', this.jwt); 
  }

  async submit() {
    const authData = { email: this.email, password: this.password }; 

    try {
      const result = await this.authService.login(authData).toPromise();
<<<<<<< HEAD
      console.log('Resultado de login:', result);  // Verifica la respuesta del login 
=======
      // Verificamos la respuesta del login
      console.log('Resultado de login:', result);  
>>>>>>> origin/salperro2

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
<<<<<<< HEAD
  logout() {
    localStorage.removeItem('token');
=======

  logout() {
    localStorage.removeItem('accessToken');
>>>>>>> origin/salperro2
    localStorage.removeItem('usuarioId');
    this.jwt = null;
    this.usuarioId = null;
    console.log("Cierre de sesión exitoso.");
  }
<<<<<<< HEAD
  
=======
>>>>>>> origin/salperro2
}
