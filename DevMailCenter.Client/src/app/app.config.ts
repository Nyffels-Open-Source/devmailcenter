import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import {provideAnimationsAsync} from '@angular/platform-browser/animations/async';
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
    // Enable on v18 release
    // providePrimeNG({
    //   theme: Aura
    // }),
    {
      provide: API_BASE_URL,
      useFactory: GetApiBaseUrl
    },
    provideHttpClient(),
  ]
};
