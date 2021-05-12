import { Component, Input, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-error-display',
  templateUrl: './error-display.component.html',
  styleUrls: ['./error-display.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ErrorDisplayComponent {
  @Input() error: any;

  constructor() { }

  // refresh(): void {
  //   window.location.reload();
  // }
}
