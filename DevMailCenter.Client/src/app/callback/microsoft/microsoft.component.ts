import {Component, OnDestroy, OnInit} from '@angular/core';
import { ProgressBarModule } from 'primeng/progressbar';
import {CommonModule} from '@angular/common';
import {CardModule} from 'primeng/card';
import {ActivatedRoute, Router} from '@angular/router';
import {Subject, takeUntil} from 'rxjs';
import {MailServerClient, MicrosoftMailServerCreate} from '../../core/openapi/generated/openapi-client';

@Component({
    selector: 'dmc-callback-microsoft',
    imports: [
      CommonModule,
      ProgressBarModule,
      CardModule
    ],
    templateUrl: './microsoft.component.html',
    styleUrl: './microsoft.component.scss'
})
export class MicrosoftComponent implements OnInit, OnDestroy {
  private _destroy$ = new Subject<void>();

  code: string | null = null;

  state: "process" | 'error' | 'success' = 'process';

  constructor(private route: ActivatedRoute, private router: Router, private mailServerClient: MailServerClient) {
  }

  ngOnInit() {
    this.route.fragment.pipe(takeUntil(this._destroy$)).subscribe((fragments) => {
      const urlFragments = new URLSearchParams(fragments ?? "");
      this.code = urlFragments.get("code");

      this.handleRequest();
    });
  }

  ngOnDestroy() {
    this._destroy$.next();
    this._destroy$.complete();
  }

  handleRequest() {
    this.mailServerClient.createMicrosoftMailServer(new MicrosoftMailServerCreate({code: this.code ?? ""})).pipe(takeUntil(this._destroy$)).subscribe({
      next: (guid) => {
        console.log(guid);
        // TODO Show user request is successful.
        // TODO Give options (go to dashboard - go to mailservers)
      },
      error: (err) => {
        console.log(err);
        // TODO Show user request has errors.
        // TODO Give options (go to dashboard - go to mailservers)
      }
    });
  }
}
