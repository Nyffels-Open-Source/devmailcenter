import {Component, OnInit} from '@angular/core';
import { ProgressBarModule } from 'primeng/progressbar';
import {CommonModule} from '@angular/common';
import {CardModule} from 'primeng/card';

@Component({
    selector: 'dmc-callback-microsoft',
    imports: [
      CommonModule,
      ProgressBarModule,
      CardModule
    ],
    templateUrl: './microsoft.component.html',
    styleUrl: './microsoft.component.scss'
})
export class MicrosoftComponent implements OnInit {
  state: "process" | 'error' | 'success' = 'process';

  constructor() {
  }

  ngOnInit() {}
}
