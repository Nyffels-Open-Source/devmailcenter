import {Component, OnDestroy, OnInit} from '@angular/core';
import {Subject} from 'rxjs';

@Component({
  selector: 'dmc-auth-login',
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  providers: []
})
export class AuthenticationLoginComponent implements OnInit, OnDestroy {
  private _destroy$: Subject<void> = new Subject<void>();

  constructor() {
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    this._destroy$.next();
    this._destroy$.complete();
  }
}
