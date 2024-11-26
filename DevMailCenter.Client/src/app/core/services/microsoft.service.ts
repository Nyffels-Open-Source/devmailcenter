import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MicrosoftService {
  private clientId: string = environment.microsoft.clientId;
  private redirectUri: string = environment.microsoft.redirectUri;
  private authority: string = environment.microsoft.authority;
  private scopes: string[] = environment.microsoft.scopes;

  constructor() {
  }

  async acquireConsentAndAuthorizationTokenByRedirect(scopes: string[] = []) {
    let url = `${this.authority}/oauth2/v2.0/authorize?`;
    url += `client_id=${this.clientId}`;
    url += `&scope=api%3A%2F%2Fnyffels-websites-api%2Faccess_as_user%20api%3A%2F%2Fnyffels-websites-api%2Femail%20api%3A%2F%2Fnyffels-websites-api%2FMail.Send%20api%3A%2F%2Fnyffels-websites-api%2Foffline_access%20api%3A%2F%2Fnyffels-websites-api%2Fopenid%20api%3A%2F%2Fnyffels-websites-api%2Fprofile%20api%3A%2F%2Fnyffels-websites-api%2FUser.Read%20openid%20profile%20offline_access`;
    url += `&redirect_uri=${this.redirectUri}`;
    url += `&response_mode=fragment`;
    url += `&response_type=token`;
    url += `&code_challenge=so0hRl0s-92wTU8QI5Ck9grpsiv_nk93QkREi_qjuAg`;
    url += `&code_challenge_method=S256`;
    url += `&prompt=consent`;

    window.open(url, "_self");
  }
}
