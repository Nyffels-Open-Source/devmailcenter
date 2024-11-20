import {Component, OnDestroy, OnInit} from '@angular/core';
import {MailServerClient} from '../../../core/openapi/generated/openapi-client';
import {Subject, takeUntil} from 'rxjs';

@Component({
    selector: 'dmc-mailserver-list',
    imports: [],
    templateUrl: './list.component.html',
    styleUrl: './list.component.scss'
})
export class ListComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  constructor(private mailServerClient: MailServerClient) {
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.mailServerClient.listMailServers(false).pipe(takeUntil(this.destroy$)).subscribe({
      next: data => {
        console.log(data);
      },
      error: error => {
        console.error(error);
        // TODO Error handling
      }
    });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
