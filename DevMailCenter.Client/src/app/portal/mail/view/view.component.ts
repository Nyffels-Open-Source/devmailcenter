import {Component, OnDestroy, OnInit} from '@angular/core';
import {CardModule} from "primeng/card";
import {ButtonDirective} from 'primeng/button';
import {TooltipModule} from 'primeng/tooltip';
import {ActivatedRoute, Router} from '@angular/router';
import {Email, EmailClient} from '../../../core/openapi/generated/openapi-client';
import {Subject, switchMap, takeUntil, tap} from 'rxjs';
import {ConfirmationService} from 'primeng/api';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {InputTextModule} from 'primeng/inputtext';
import {FormsModule} from '@angular/forms';
import {SkeletonModule} from 'primeng/skeleton';
import {InputGroupModule} from 'primeng/inputgroup';
import {DatePipe, Location} from '@angular/common';
import {InputTextareaModule} from 'primeng/inputtextarea';

@Component({
  selector: 'dmc-mail-view',
  imports: [
    CardModule,
    ButtonDirective,
    TooltipModule,
    ConfirmDialogModule,
    InputTextModule,
    FormsModule,
    SkeletonModule,
    InputGroupModule,
    InputTextareaModule,
    DatePipe
  ],
  templateUrl: './view.component.html',
  styleUrl: './view.component.scss',
  providers: [ConfirmationService]
})
export class ViewComponent implements OnInit, OnDestroy {
  emailId!: string;
  email!: Email;
  loaded = false;

  private destroy$: Subject<void> = new Subject<void>();

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private confirmationService: ConfirmationService, private emailClient: EmailClient, private location: Location) {}

  ngOnInit() {
    this.activatedRoute.params.pipe(
      tap(params => this.emailId = params['id']),
      switchMap(() => this.emailClient.getEmail(this.emailId, true)),
      takeUntil(this.destroy$)
    )
      .subscribe({
        next: (email) => {
          if (email) {
            this.email = email;
            this.loaded = true;
          }
        },
        error: (error) => {
          // TODO Error handling
          console.error(error);
        }
      });
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
      message: `Are you sure you want to delete this e-mail?`,
      header: 'Are you sure?',
      icon: 'pi pi-exclamation-triangle',
      acceptIcon: "none",
      acceptButtonStyleClass: "p-button-danger",
      rejectIcon: "none",
      rejectButtonStyleClass: "p-button-text p-button-secondary",
      closeOnEscape: false,
      accept: () => {
        this.emailClient.deleteEmail(this.email.id)
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

  toServer() {
    this.router.navigate(['/portal/emailserver/view', this.email.serverId]);
  }

  getSeverityColor() {
    return { 'Normal': 'warning', 'Low': 'secondary', 'High': 'danger' }[this.email.priority.toString()] as any;
  }
}
