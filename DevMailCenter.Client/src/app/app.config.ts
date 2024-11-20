import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {providePrimeNG} from 'primeng/config';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import Aura from '@primeng/themes/aura';
import {API_BASE_URL} from './core/openapi/generated/openapi-client';
import {environment} from '../environments/environment';
import {provideHttpClient} from '@angular/common/http';

export function GetApiBaseUrl() {
  return environment.api.url;
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: Aura
    }),
    {
      provide: API_BASE_URL,
      useFactory: GetApiBaseUrl
    },
    provideHttpClient(),
  ]
};
