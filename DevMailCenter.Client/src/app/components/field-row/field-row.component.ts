import {Component, ElementRef, OnInit, ViewChild, ViewContainerRef, ViewEncapsulation} from '@angular/core';

@Component({
  selector: 'form-row',
  imports: [],
  template: `<ng-content></ng-content>`,
  styles: ``,
  encapsulation: ViewEncapsulation.None,
  host: {
    '[class.flex]': 'true',
    '[class.flex-row]': 'true',
    '[class.gap-4]': 'true',
    '[class.w-full]': 'true',
    '[class.grow]': 'true',
    '[class.row]': 'true'
  }
})
export class FormRow {}
