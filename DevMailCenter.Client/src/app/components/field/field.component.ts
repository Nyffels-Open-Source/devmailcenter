import {Component, ViewEncapsulation} from '@angular/core';

@Component({
  selector: 'dmc-field',
  imports: [],
  template: `<div class="flex flex-col gap-2 w-full"><ng-content></ng-content></div>`,
  styles: ``,
  encapsulation: ViewEncapsulation.None
})
export class DmcField {}
