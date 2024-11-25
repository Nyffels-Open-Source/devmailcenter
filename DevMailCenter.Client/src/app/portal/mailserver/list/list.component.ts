import {Component, OnDestroy, OnInit, ViewChild} from '@angular/core';
import {ConfigClient, MailServer, MailServerClient} from '../../../core/openapi/generated/openapi-client';
import {Subject, takeUntil} from 'rxjs';
import {Table, TableModule} from 'primeng/table';
import {ButtonModule} from 'primeng/button';
import {TooltipModule} from 'primeng/tooltip';
import {ActivatedRoute, Router} from '@angular/router';
import {CardModule} from 'primeng/card';
import {DatePipe} from '@angular/common';
import {TagModule} from 'primeng/tag';

@Component({
  selector: 'dmc-mailserver-list',
  imports: [
    TableModule,
    ButtonModule,
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
    // TODO
    alert("Not yet implemented");
  }

  viewServer(server: MailServer) {
    // TODO
    alert("Not yet implemented");
  }
}
