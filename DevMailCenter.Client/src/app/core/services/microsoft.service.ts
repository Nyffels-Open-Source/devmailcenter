import { Injectable } from '@angular/core';
import {BrowserCacheLocation, LogLevel, PublicClientApplication} from '@azure/msal-browser';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MicrosoftService {
  private clientId: string = environment.microsoft.clientId;
  private redirectUri: string = environment.microsoft.redirectUri;
  private authority: string = environment.microsoft.authority;
  private publicClient!: PublicClientApplication;
  private scopes: string[] = environment.microsoft.scopes;


  constructor() {
    this.setupClient();
  }

  async setupClient() {
    this.publicClient = new PublicClientApplication({
      auth: {
        clientId: this.clientId,
        authority: this.authority,
        redirectUri: this.redirectUri,
      },
      cache: {
        cacheLocation: BrowserCacheLocation.SessionStorage,
        storeAuthStateInCookie: false, // set to true for IE 11
      },
      system: {
        loggerOptions: {
          loggerCallback: (logLevel: any, message: any) => {
            switch (logLevel) {
              case LogLevel.Info:
                console.info(message);
                break;
              case LogLevel.Error:
                console.error(message);
                break;
              case LogLevel.Trace:
                console.trace(message);
                break;
              case LogLevel.Verbose:
                console.log(message);
                break;
              case LogLevel.Warning:
                console.warn(message);
                break;
              default:
                console.log(message);
                break;
            }
          },
          logLevel: LogLevel.Error,
          piiLoggingEnabled: false,
        },
      },
    });
    await this.publicClient.initialize();
  }

  async acquireConsentAndAuthorizationTokenByRedirect(scopes: string[] = []) {
    return await this.publicClient.acquireTokenRedirect({
      prompt: "consent",
      scopes: (scopes ?? []).length <= 0 ? this.scopes : scopes
    });
  }
}
