import {Component, OnDestroy, OnInit} from '@angular/core';
import {ButtonDirective} from "primeng/button";
import {CardModule} from "primeng/card";
import {ConfirmationService, PrimeTemplate} from "primeng/api";
import {TooltipModule} from "primeng/tooltip";
import {ActivatedRoute, Router} from '@angular/router';
import {Subject, takeUntil} from 'rxjs';
import {MailServer, MailServerClient} from '../../../core/openapi/generated/openapi-client';
import {ConfirmDialogModule} from 'primeng/confirmdialog';
import {DatePipe, Location} from '@angular/common';
import {mailServerType} from '../../../shared/models/mailservertype.enum';
import {FormRow} from '../../../components/field-row/field-row.component';
import {FormField} from '../../../components/field/field.component';
import {FormLabel} from '../../../components/label/label';
import {InputText} from 'primeng/inputtext';
import {FormsModule} from '@angular/forms';
import {Tag} from 'primeng/tag';
import {Divider} from 'primeng/divider';
import {Skeleton} from 'primeng/skeleton';

@Component({
  selector: 'dmc-mailserver-view',
  imports: [
    ButtonDirective,
    CardModule,
    PrimeTemplate,
    TooltipModule,
    ConfirmDialogModule,
    FormRow,
    FormField,
    FormLabel,
    InputText,
    FormsModule,
    Tag,
    DatePipe,
    Divider,
    Skeleton
  ],
  templateUrl: './view.component.html',
  styleUrl: './view.component.scss',
  providers: [ConfirmationService]
})
export class ViewComponent implements OnInit, OnDestroy {
  serverId!: string;
  server!: MailServer;
  serverType!: string;

  private destroy$: Subject<void> = new Subject<void>();

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private confirmationService: ConfirmationService, private mailServerClient: MailServerClient, private location: Location) {}

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      if (params['id']) {
        this.serverId = params['id'];
        this.mailServerClient.getMailServer(this.serverId, true).subscribe(data => {
          if (!data) {
            alert("No mailserver found for id: " + this.serverId);
            this.router.navigate(['/portal/emailserver/list'], {replaceUrl: true});
            return;
          }

          this.server = data;
          this.setType();
          console.log(this.server);
        })
      } else {
        alert("No mailserverID found!");
        this.router.navigate(['/portal/emailserver/list'], {replaceUrl: true});
      }
    })
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

  setType() {
    switch ((this.server.type as any) as string) {
      case mailServerType.Smtp:
        this.serverType = "SMTP";
        break;
      case mailServerType.Google:
        this.serverType = "Google / Gmail";
        break;
      case mailServerType.MicrosoftExchange:
        this.serverType = "Exchange / Outlook";
        break;
    }
  }

  protected readonly mailServerType = mailServerType;
}
