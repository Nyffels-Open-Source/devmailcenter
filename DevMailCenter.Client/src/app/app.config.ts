import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
import {API_BASE_URL} from './core/openapi/generated/openapi-client';
import {environment} from '../environments/environment';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import {authenticationInterceptor} from './core/Interceptors/http-authentication.interceptor';
import {DATE_PIPE_DEFAULT_OPTIONS} from '@angular/common';
import {providePrimeNG} from 'primeng/config';
import Aura from '@primeng/themes/aura';
import {AuthenticatedGuard} from './core/guards/authenticated.guard';

export function GetApiBaseUrl() {
  return environment.api.url;
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura
      }
    }),
    {
      provide: API_BASE_URL,
      useFactory: GetApiBaseUrl
    },
    provideHttpClient(withInterceptors([authenticationInterceptor])),
    {
      provide: DATE_PIPE_DEFAULT_OPTIONS,
      useValue: { dateFormat: "dd/MM/YYYY" }
    },
    AuthenticatedGuard
  ]
};
