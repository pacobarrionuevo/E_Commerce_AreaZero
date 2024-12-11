import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms'; 
import { CarritoService } from '../../services/carrito.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

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
  jwt: string | null = null; 
  usuarioId: number | null = null;

  constructor(private authService: AuthService, private carritoService: CarritoService, private router: Router) {}

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

      console.log('Resultado de login:', result);  

      if (result) {
        // Guarda el token y el ID del usuario en el localStorage
        localStorage.setItem('accessToken', result.stringToken); 
        localStorage.setItem('usuarioId', result.usuarioId.toString()); 

        this.jwt = result.stringToken;
        this.usuarioId = result.usuarioId;

        console.log("Inicio de sesión exitoso.");
        this.carritoService.localtoCart();
        localStorage.removeItem('cart');
        this.router.navigate(['/']);
      } else {
        console.error("No se recibió un token de acceso.");
      }
    } catch (error) {
      console.error("Error al iniciar sesión:", error);
    }
  }
}