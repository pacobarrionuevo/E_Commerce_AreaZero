import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule} from '@angular/forms'; 
@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = ''; 
  password: string = ''; 
  nombre: string = '';
  direccion: string = ''; 
  jwt: string = '';
  constructor(private authService: AuthService) {}
  async submit() {
    const authData = { nombre: this.nombre, email: this.email , password: this.password, direccion: this.direccion   }; 
    const result = await this.authService.register(authData).toPromise();
    if (result) {
      this.jwt = result.accessToken; 
    }
  }
}