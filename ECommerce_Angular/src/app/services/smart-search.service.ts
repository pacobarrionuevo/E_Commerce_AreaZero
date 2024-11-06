import { Injectable } from "@angular/core";
import { ApiService } from "./api.service";
import { Result } from "../models/result";
import { environment } from '../../environments/environment';
@Injectable({
    providedIn: 'root'
})
export class SmartSearchService {

    private BASE_URL = environment.apiUrl;

    constructor(private api: ApiService) { }

  // Llama al endpoint de búsqueda en el backend
  async search(query: string): Promise<Result<string[]>> {
    // Usa el ApiService para realizar la solicitud GET, construyendo el path completo con el endpoint actual
    return this.api.get<string[]>(`ControladorSmartSearch`, { query });
  }
}