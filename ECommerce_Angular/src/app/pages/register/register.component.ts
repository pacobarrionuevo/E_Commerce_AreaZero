import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms'; 

@Component({
    selector: 'app-register',
    imports: [FormsModule],
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  email: string = ''; 
  password: string = ''; 
  nombre: string = '';
  direccion: string = ''; 
  jwt: string = '';

  constructor(private authService: AuthService, private router: Router) {}

  async submit() {
    //los datos que hacen falta para el registro
    const authData = { nombre: this.nombre, email: this.email , password: this.password, direccion: this.direccion }; 
    const result = await this.authService.register(authData).toPromise();
//navega al login para iniciar sesión
    if (result) {
      this.jwt = result.stringToken; 
      this.router.navigate(['/login']); 
    }
  }
}
