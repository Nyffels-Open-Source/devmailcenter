import {Component, ViewEncapsulation} from '@angular/core';

@Component({
  selector: 'form-label',
  imports: [],
  template: `
    <div class="font-bold"><ng-content></ng-content></div>`,
  styles: ``,
  encapsulation: ViewEncapsulation.None,
})
export class FormLabel {}
