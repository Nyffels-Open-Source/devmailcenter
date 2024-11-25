import { Component } from '@angular/core';
import { ProgressBarModule } from 'primeng/progressbar';
import {CommonModule} from '@angular/common';

@Component({
    selector: 'dmc-callback-microsoft',
    imports: [
      CommonModule,
      ProgressBarModule
    ],
    templateUrl: './microsoft.component.html',
    styleUrl: './microsoft.component.scss'
})
export class MicrosoftComponent {

}
