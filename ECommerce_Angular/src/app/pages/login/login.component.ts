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

  constructor(private authService: AuthService) {}

  async submit() {
    // Solo enviar email y password para el login
    const authData = { email: this.email, password: this.password }; 

    try {
      const result = await this.authService.login(authData).toPromise();

      if (result) {
        this.jwt = result.accessToken; // Guardar el token JWT recibido del backend
        console.log("Inicio de sesión exitoso.");
      }
    } catch (error) {
      console.error("Error al iniciar sesión:", error);
    }
  }
}
