import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject} from 'rxjs';
import {Password} from 'primeng/password';
import {InputText} from 'primeng/inputtext';
import {FormsModule} from '@angular/forms';
import {FormField} from '../../components/field/field.component';
import {FormLabel} from '../../components/label/label';
import {Button} from 'primeng/button';

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

  constructor() {
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this._destroy$.next();
    this._destroy$.complete();
  }

  authenticate() {
    // TODO
  }
}
