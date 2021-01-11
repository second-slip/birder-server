import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';

@Component({
  selector: 'app-showcase-observation-feed-item',
  templateUrl: './showcase-observation-feed-item.component.html',
  styleUrls: ['./showcase-observation-feed-item.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowcaseObservationFeedItemComponent {
  @Input() observation: ObservationViewModel;
  
  constructor() { }

}
