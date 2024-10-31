import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { AuthRequest } from '../models/auth-request';
import { AuthResponse } from '../models/auth-response';
import { Result } from '../models/result';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private api: ApiService) {}

  async login(authData: AuthRequest): Promise<Result<AuthResponse>> {
    const result = await this.api.post<AuthResponse>('auth', authData);

    if (result.success) {
      this.api.jwt = result.data.accessToken;
    }

    return result;
  }

  getSecretMessage(): Promise<Result<string>> {
    return this.api.get<string>('auth', null, 'text');
  }
}
