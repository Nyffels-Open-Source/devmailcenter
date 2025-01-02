import {Component, ViewEncapsulation} from '@angular/core';

@Component({
  selector: 'form-field',
  imports: [],
  template: `
    <ng-content></ng-content>`,
  styles: ``,
  encapsulation: ViewEncapsulation.None,
  host: {
    '[class.flex]': 'true',
    '[class.flex-col]': 'true',
    '[class.gap-2]': 'true',
    '[class.w-full]': 'true'
  }
})
export class FormField {}
