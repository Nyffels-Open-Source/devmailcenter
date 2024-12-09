import {Component, OnInit} from '@angular/core';
import {CardModule} from 'primeng/card';
import {CommonModule, Location} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {ButtonModule} from 'primeng/button';
import {RippleModule} from 'primeng/ripple';
import {TooltipModule} from 'primeng/tooltip';
import {SmtpMailServerCreate} from '../../../../core/openapi/generated/openapi-client';
import {InputTextModule} from 'primeng/inputtext';
import {InputNumberModule} from 'primeng/inputnumber';
import {InputGroupModule} from 'primeng/inputgroup';
import {InputGroupAddonModule} from 'primeng/inputgroupaddon';
import {RadioButtonModule} from 'primeng/radiobutton';
import {CheckboxModule} from 'primeng/checkbox';
import {DmcField} from '../../../../components/field/field.component';
import {DmcFieldRow} from '../../../../components/field-row/field-row.component';

@Component({
  selector: 'dmc-mailserver-add-smtp',
  imports: [CardModule, CommonModule, FormsModule, ButtonModule, RippleModule, TooltipModule, InputTextModule, InputNumberModule, InputGroupModule, InputGroupAddonModule, RadioButtonModule, CheckboxModule, DmcField, DmcField, DmcField, DmcFieldRow],
  templateUrl: './smtp.component.html',
  styleUrl: './smtp.component.scss'
})
export class SmtpComponent implements OnInit {
  smtp: SmtpMailServerCreate = new SmtpMailServerCreate();
  canSave = false;

  constructor(private location: Location) {}

  ngOnInit() {}

  return() {
    this.location.back();
  }

  addServer() {
    // TODO
  }
}
