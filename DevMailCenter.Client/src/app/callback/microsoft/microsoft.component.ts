import {Component, OnDestroy, OnInit} from '@angular/core';
import {ProgressBar, ProgressBarModule} from 'primeng/progressbar';
import {CommonModule} from '@angular/common';
import {Card, CardModule} from 'primeng/card';
import {ActivatedRoute, Router} from '@angular/router';
import {Subject, takeUntil} from 'rxjs';
import {MailServerClient, MicrosoftMailServerCreate} from '../../core/openapi/generated/openapi-client';
import {Button, ButtonModule} from 'primeng/button';
import {InputText, InputTextModule} from 'primeng/inputtext';
import {InputGroup, InputGroupModule} from 'primeng/inputgroup';

@Component({
  selector: 'dmc-callback-microsoft',
  imports: [
    CommonModule,
    ProgressBar,
    Card,
    Button,
    InputText,
    InputGroup
  ],
  templateUrl: './microsoft.component.html',
  styleUrl: './microsoft.component.scss'
})
export class MicrosoftComponent implements OnInit, OnDestroy {
  private _destroy$ = new Subject<void>();

  accessToken: string | null = null;

  state: "process" | 'error' | 'success' = 'process';
  message: string = "";
  showNameChange = true;

  constructor(private route: ActivatedRoute, private router: Router, private mailServerClient: MailServerClient) {
  }

  ngOnInit() {
    this.route.fragment.pipe(takeUntil(this._destroy$))
      .subscribe((fragments) => {
        const urlFragments = new URLSearchParams(fragments ?? "");
        this.accessToken = urlFragments.get("access_token");

        this.handleRequest();
      });
  }

  ngOnDestroy() {
    this._destroy$.next();
    this._destroy$.complete();
  }

  handleRequest() {
    this.mailServerClient.createMicrosoftMailServer(new MicrosoftMailServerCreate({code: this.accessToken ?? ""}))
      .pipe(takeUntil(this._destroy$))
      .subscribe({
        next: (guid) => {
          this.state = 'success';
          this.message = `Emailserver created with ID ${guid}.`;
        },
        error: (err) => {
          this.state = 'error';
          this.message = `Adding mailserver failed with error: ${err.message}.`;
        }
      });
  }

  setServerName(name: string) {
    this.showNameChange = false;
    this.message = `Emailserver created with name '${name}'`;

    // TODO Save name in database.
  }

  navigateToMails() {
    this.router.navigate(['/portal/mail/list']);
  }

  navigateToMailServers() {
    this.router.navigate(['/portal/emailserver/list']);
  }
}
