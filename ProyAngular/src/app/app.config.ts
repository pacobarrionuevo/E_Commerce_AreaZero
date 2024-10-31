import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { IMAGE_CONFIG } from '@angular/common';
import { provideNgxStripe } from 'ngx-stripe';
import { environment } from '../environments/environment';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }), 
    provideRouter(routes),
    // Habilita el uso de HttpClient
    provideHttpClient(),
    // Configura Stripe, hay que pasarle la clave pública (se encuentra en el dashboard => desarrolladores)
    provideNgxStripe(environment.stripePublicKey),
    // Deshabilita el warning cuando las imágenes son muy grandes
    { provide: IMAGE_CONFIG, useValue: { disableImageSizeWarning: true, disableImageLazyLoadWarning: true } }, 
  ]
};
