import { Component } from '@angular/core';
import {CardModule} from "primeng/card";
import {ButtonDirective} from 'primeng/button';
import {TooltipModule} from 'primeng/tooltip';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
    selector: 'dmc-mail-view',
  imports: [
    CardModule,
    ButtonDirective,
    TooltipModule
  ],
    templateUrl: './view.component.html',
    styleUrl: './view.component.scss'
})
export class ViewComponent {
  constructor(private router: Router, private activatedRoute: ActivatedRoute) {}

  return() {
    this.router.navigate(['../../list'], {relativeTo: this.activatedRoute});
  }
}
