import {Component, OnDestroy, OnInit} from '@angular/core';
import {ButtonDirective, ButtonModule} from "primeng/button";
import {CardModule} from "primeng/card";
import {ConfirmDialogModule} from "primeng/confirmdialog";
import {CommonModule, DatePipe} from "@angular/common";
import {ConfirmationService, PrimeTemplate} from "primeng/api";
import {Ripple, RippleModule} from "primeng/ripple";
import {TableModule} from "primeng/table";
import {TagModule} from "primeng/tag";
import {TooltipModule} from "primeng/tooltip";
import {Subject, takeUntil} from 'rxjs';
import {Email, EmailClient, MailServer} from '../../../core/openapi/generated/openapi-client';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'dmc-mail-list',
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

  emails: Email[] = [];
  isLoading = false;

  constructor(private emailClient: EmailClient, private router: Router, private activatedRoute: ActivatedRoute, private confirmationService: ConfirmationService) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    this.emailClient.listEmails(true)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: data => {
          this.emails = data;
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

  refresh() {
    this.loadData();
  }

  deleteEmail(email: Email) {
    this.confirmationService.confirm({
      message: `Are you sure you want to delete this e-mail?`,
      header: 'Are you sure?',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      acceptButtonStyleClass: "p-button-danger",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text p-button-secondary",
      closeOnEscape: false,
      accept: () => {
        this.emailClient.deleteEmail(email.id)
          .pipe(takeUntil(this.destroy$))
          .subscribe({
            next: () => {
              this.emails.splice(this.emails.indexOf(email), 1);
            }
          })
      },
      key: 'positionDialog'
    });
  }

  viewEmail(email: Email) {
    // TODO
    alert("Not yet implemented");
  }

  getEmailReceivers(email: Email) {
    return (email.receivers ?? []).map((receiver) => receiver.receiverEmail)
      .join(", ");
  }
}
