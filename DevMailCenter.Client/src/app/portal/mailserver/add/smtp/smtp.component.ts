import {Component, OnInit} from '@angular/core';
import {CardModule} from 'primeng/card';
import {CommonModule, Location} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {ButtonModule} from 'primeng/button';
import {RippleModule} from 'primeng/ripple';
import {TooltipModule} from 'primeng/tooltip';

@Component({
  selector: 'dmc-mailserver-add-smtp',
  imports: [CardModule, CommonModule, FormsModule, ButtonModule, RippleModule, TooltipModule],
  templateUrl: './smtp.component.html',
  styleUrl: './smtp.component.scss'
})
export class SmtpComponent implements OnInit {
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
