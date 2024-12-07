import {Component, OnDestroy, OnInit} from '@angular/core';
import {CardModule} from "primeng/card";
import {ButtonDirective} from 'primeng/button';
import {TooltipModule} from 'primeng/tooltip';
import {ActivatedRoute, Router} from '@angular/router';
import {Email, EmailClient} from '../../../core/openapi/generated/openapi-client';
import {Subject, takeUntil} from 'rxjs';
import {ConfirmationService} from 'primeng/api';
import {ConfirmDialogModule} from 'primeng/confirmdialog';

@Component({
  selector: 'dmc-mail-view',
  imports: [
    CardModule,
    ButtonDirective,
    TooltipModule,
    ConfirmDialogModule
  ],
  templateUrl: './view.component.html',
  styleUrl: './view.component.scss',
  providers: [ConfirmationService]
})
export class ViewComponent implements OnInit, OnDestroy {
  email!: Email;

  private destroy$: Subject<void> = new Subject<void>();

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private confirmationService: ConfirmationService, private emailClient: EmailClient) {}

  ngOnInit() {

  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  return() {
    this.router.navigate(['../../list'], {relativeTo: this.activatedRoute});
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
              this.return();
            }
          })
      },
      key: 'positionDialog'
    });
  }
}
