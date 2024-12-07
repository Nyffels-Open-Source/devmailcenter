import {Component, OnDestroy, OnInit} from '@angular/core';
import {ButtonDirective} from "primeng/button";
import {CardModule} from "primeng/card";
import {ConfirmationService, PrimeTemplate} from "primeng/api";
import {TooltipModule} from "primeng/tooltip";
import {ActivatedRoute, Router} from '@angular/router';
import {Subject, takeUntil} from 'rxjs';
import {MailServer, MailServerClient} from '../../../core/openapi/generated/openapi-client';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {Location} from '@angular/common';

@Component({
  selector: 'dmc-mailserver-view',
  imports: [
    ButtonDirective,
    CardModule,
    PrimeTemplate,
    TooltipModule,
    ConfirmDialogModule
  ],
  templateUrl: './view.component.html',
  styleUrl: './view.component.scss',
  providers: [ConfirmationService]
})
export class ViewComponent implements OnInit, OnDestroy {
  server!: MailServer;

  private destroy$: Subject<void> = new Subject<void>();

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private confirmationService: ConfirmationService, private mailServerClient: MailServerClient, private location: Location) {}

  ngOnInit() {

  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  return() {
    this.location.back();
  }

  delete() {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete '${this.server.name}'?`,
      header: 'Are you sure?',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      acceptButtonStyleClass: "p-button-danger",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text p-button-secondary",
      closeOnEscape: false,
      accept: () => {
        this.mailServerClient.deleteMailServer(this.server.id)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.router.navigate(['../../list'], {relativeTo: this.activatedRoute});
            }
          })
      },
      key: 'positionDialog'
    });
  }
}
