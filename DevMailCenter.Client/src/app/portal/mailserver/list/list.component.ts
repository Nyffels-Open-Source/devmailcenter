import {Component, OnDestroy, OnInit} from '@angular/core';
import {MailServer, MailServerClient} from '../../../core/openapi/generated/openapi-client';
import {Subject, takeUntil} from 'rxjs';
import {TableModule} from 'primeng/table';
import {ButtonModule} from 'primeng/button';
import {TooltipModule} from 'primeng/tooltip';
import {ActivatedRoute, Router} from '@angular/router';
import {CardModule} from 'primeng/card';
import {CommonModule, DatePipe} from '@angular/common';
import {TagModule} from 'primeng/tag';
import {RippleModule} from 'primeng/ripple';

@Component({
  selector: 'dmc-mailserver-list',
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    RippleModule,
    TooltipModule,
    CardModule,
    DatePipe,
    TagModule
  ],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  servers: MailServer[] = [];
  isLoading = false;

  constructor(private mailServerClient: MailServerClient, private router: Router, private activatedRoute: ActivatedRoute) {
  }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    this.mailServerClient.listMailServers(false)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: data => {
          this.servers = data;
          this.isLoading = false;
        },
        error: error => {
          console.error(error);
          this.isLoading = false
          // TODO Error handling
        }
      });
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  addServer() {
    this.router.navigate(['../add'], {relativeTo: this.activatedRoute});
  }

  refresh() {
    this.loadData();
  }

  deleteServer(server: MailServer) {
    // TODO Are you sure you wish to delete this mailserver. All e-mails connected to this mailserver will also be deleted.
    // TODO Delete the server
    alert("Not yet implemented");
  }

  viewServer(server: MailServer) {
    // TODO
    alert("Not yet implemented");
  }
}
