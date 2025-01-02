import {Component, OnInit} from '@angular/core';
import {Router, RouterOutlet} from '@angular/router';
import {SpeedDial, SpeedDialModule} from 'primeng/speeddial';
import {MenuItem} from 'primeng/api';
import {FormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'dmc-portal',
  imports: [
    RouterOutlet,
    SpeedDial,
    FormsModule,
    CommonModule
  ],
  templateUrl: './portal.component.html',
  styleUrl: './portal.component.scss'
})
export class PortalComponent implements OnInit {
  tooltipItems!: MenuItem[];

  constructor(private router: Router) {}

  ngOnInit() {
    this.tooltipItems = [
      {
        tooltipOptions: {
          tooltipLabel: 'Emails'
        },
        icon: 'pi pi-envelope',
        command: () => {
          this.router.navigate(['portal/email/list']);
        }
      },
      {
        tooltipOptions: {
          tooltipLabel: 'Servers'
        },
        icon: 'pi pi-server',
        command: () => {
          this.router.navigate(['portal/emailserver/list']);
        }
      }
    ]
  }
}
