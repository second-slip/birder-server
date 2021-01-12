import { Component, Input, ViewEncapsulation } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';

@Component({
  selector: 'app-observation-feed-item',
  templateUrl: './observation-feed-item.component.html',
  styleUrls: ['./observation-feed-item.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationFeedItemComponent {
  @Input() observation: ObservationViewModel;
  @Input() user: UserViewModel;

  constructor() { }
}
