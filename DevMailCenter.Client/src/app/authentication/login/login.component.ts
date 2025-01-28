import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject} from 'rxjs';
import {Password} from 'primeng/password';
import {InputText} from 'primeng/inputtext';
import {FormsModule} from '@angular/forms';
import {FormField} from '../../components/field/field.component';
import {FormLabel} from '../../components/label/label';
import {Button} from 'primeng/button';
import {ConfigClient} from '../../core/openapi/generated/openapi-client';
import {Router} from '@angular/router';

@Component({
  selector: 'dmc-auth-login',
  imports: [
    InputText,
    FormsModule,
    FormField,
    FormLabel,
    FormLabel,
    Password,
    Button
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  providers: [Password, InputText]
})
export class AuthenticationLoginComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  login = "";
  password = "";

  constructor(private configClient: ConfigClient, private router: Router) {
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this._destroy$.next();
    this._destroy$.complete();
  }

  authenticate() {
    localStorage.setItem('authlgn', btoa(`${this.login}:${this.password}`));
    this.configClient.authenticate().subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        alert("Incorrect credentials");
        localStorage.removeItem('authlgn');
      }
    })
  }
}
