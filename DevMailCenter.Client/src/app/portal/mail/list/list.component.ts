import {Component, OnDestroy, OnInit} from '@angular/core';
import {Card} from "primeng/card";
import {ConfirmDialog} from "primeng/confirmdialog";
import {CommonModule, DatePipe} from "@angular/common";
import {ConfirmationService} from "primeng/api";
import {TableModule} from "primeng/table";
import {Tag} from "primeng/tag";
import {Subject, takeUntil} from 'rxjs';
import {Email, EmailClient} from '../../../core/openapi/generated/openapi-client';
import {ActivatedRoute, Router} from '@angular/router';
import {Button} from 'primeng/button';
import {Tooltip} from 'primeng/tooltip';

@Component({
  selector: 'dmc-mail-list',
  imports: [
    CommonModule,
    Card,
    DatePipe,
    Tag,
    ConfirmDialog,
    TableModule,
    Button,
    Tooltip
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
    this.router.navigate(['../view', email.id], {relativeTo: this.activatedRoute});
  }

  getEmailReceivers(email: Email) {
    return (email.receivers ?? []).map((receiver) => receiver.receiverEmail)
      .join(", ");
  }
}
