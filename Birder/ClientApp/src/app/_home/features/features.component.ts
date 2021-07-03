import { Component, Input, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-features',
  templateUrl: './features.component.html',
  styleUrls: ['./features.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FeaturesComponent {
  @Input()
  public showBlurb: boolean;
}
