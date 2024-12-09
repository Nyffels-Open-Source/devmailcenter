import {Component, OnInit} from '@angular/core';
import {CardModule} from 'primeng/card';
import {CommonModule, Location} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {ButtonModule} from 'primeng/button';
import {RippleModule} from 'primeng/ripple';
import {TooltipModule} from 'primeng/tooltip';
import {MailServerClient, SmtpMailServerCreate} from '../../../../core/openapi/generated/openapi-client';
import {InputTextModule} from 'primeng/inputtext';
import {InputNumberModule} from 'primeng/inputnumber';
import {InputGroupModule} from 'primeng/inputgroup';
import {InputGroupAddonModule} from 'primeng/inputgroupaddon';
import {RadioButtonModule} from 'primeng/radiobutton';
import {CheckboxModule} from 'primeng/checkbox';
import {FormRow} from '../../../../components/field-row/field-row.component';
import {FormField} from '../../../../components/field/field.component';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'dmc-mailserver-add-smtp',
  imports: [CardModule, CommonModule, FormsModule, ButtonModule, RippleModule, TooltipModule, InputTextModule, InputNumberModule, InputGroupModule, InputGroupAddonModule, RadioButtonModule, CheckboxModule, FormRow, FormField],
  templateUrl: './smtp.component.html',
  styleUrl: './smtp.component.scss'
})
export class SmtpComponent implements OnInit {
  smtp: SmtpMailServerCreate = new SmtpMailServerCreate();
  canSave = false;

  constructor(private location: Location, private mailServerClient: MailServerClient, private router: Router, private activatedRoute: ActivatedRoute) {}

  ngOnInit() {
    this.smtp.ssl = true;
    this.smtp.port = 465;
  }

  return() {
    this.location.back();
  }

  addServer() {
    this.mailServerClient.createSmtpMailServer(this.smtp).subscribe({
      next: () => {
        this.router.navigate(['../../list'], {relativeTo: this.activatedRoute});
      },
      error: (err) => {
        // TODO Better error throwing
        console.error(err);
      }
    })
  }

  checkChanges() {
    this.canSave = (this.smtp.name ?? '').trim().length > 0 &&
      (this.smtp.host ?? '').trim().length > 0 &&
      !Number.isNaN(+(this.smtp.port)) &&
      (this.smtp.email ?? '').trim().length > 0 &&
      (this.smtp.user ?? '').trim().length > 0 &&
      (this.smtp.password ?? '').trim().length > 0 &&
      (this.smtp.username ?? '').trim().length > 0
  }
}
