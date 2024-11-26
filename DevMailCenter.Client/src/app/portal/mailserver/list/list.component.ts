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
import {ConfirmationService, MessageService} from 'primeng/api';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {ToastModule} from 'primeng/toast';

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
    TagModule,
    ConfirmDialogModule
  ],
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss',
  providers: [ConfirmationService]
})
export class ListComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  servers: MailServer[] = [];
  isLoading = false;

  constructor(private mailServerClient: MailServerClient, private router: Router, private activatedRoute: ActivatedRoute, private confirmationService: ConfirmationService) {
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
    this.confirmationService.confirm({
      message: `Are you sure you want to delete '${server.name}'?`,
      header: 'Are you sure?',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      acceptButtonStyleClass: "p-button-danger",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text p-button-secondary",
      closeOnEscape: false,
      accept: () => {
        this.mailServerClient.deleteMailServer(server.id)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.servers.splice(this.servers.indexOf(server), 1);
            }
          })
      },
      key: 'positionDialog'
    });
  }

  viewServer(server: MailServer) {
    // TODO
    alert("Not yet implemented");
  }
}
