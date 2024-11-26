import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms'; 

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = ''; 
  password: string = ''; 
  jwt: string = ''; 
  usuarioId: number | null = null;

  constructor(private authService: AuthService) {}

  async submit() {
    const authData = { email: this.email, password: this.password }; 

    try {
      const result = await this.authService.login(authData).toPromise();
      console.log('Resultado de login:', result);  // Verifica la respuesta del login

      if (result) {
        // Guarda el token y el ID del usuario en el localStorage
        localStorage.setItem('token', result.stringToken); // Guarda el token JWT
        localStorage.setItem('usuarioId', result.usuarioId.toString()); // Guarda el ID del usuario

        // Asigna el token y el ID a las variables locales
        this.jwt = result.stringToken;
        this.usuarioId = result.usuarioId;

        console.log("Inicio de sesión exitoso.");
      } else {
        console.error("No se recibió un token de acceso.");
      }
    } catch (error) {
      console.error("Error al iniciar sesión:", error);
    }
  }
  
}