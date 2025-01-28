import {Component} from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
    selector: 'dmc-root',
    imports: [RouterOutlet],
    templateUrl: './app.component.html',
    styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'DevMailCenter';

  constructor() {}
}
