import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { AuthRequest } from '../../models/auth-request';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  name: string = '';
  password: string = '';
  role: string = 'admin';
  jwt: string = '';

  constructor(private authService: AuthService) {}

  async submit() {
    const authData : AuthRequest = { username: this.name, password: this.password, role: this.role };
    const result = await this.authService.login(authData);

    if (result.success) {
      this.jwt = result.data.accessToken;
    }
  }

  async showSecretMessage() {
    const result = await this.authService.getSecretMessage();

    if (result.success) {
      alert(result.data);
    } else {
      alert('No estás autorizado');
    }
  }
}
