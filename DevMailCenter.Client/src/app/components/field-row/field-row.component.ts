import {Component, ElementRef, OnInit, ViewChild, ViewContainerRef, ViewEncapsulation} from '@angular/core';

@Component({
  selector: 'form-row',
  imports: [],
  template: `<ng-template #template><ng-content></ng-content></ng-template>`,
  styles: ``,
  encapsulation: ViewEncapsulation.None
})
export class DmcFieldRow implements OnInit {
  @ViewChild('template', { static: true }) template: any;

  constructor(private viewContainerRef: ViewContainerRef) {}

  ngOnInit() {
    console.log((this.template.elementRef as ElementRef).nativeElement.classList);
    // (this.template.elementRef as ElementRef).nativeElement.classList.add('flex flex-row gap-4 w-full');
    console.log(this.template);
    this.viewContainerRef.createEmbeddedView(this.template);
  }
}
