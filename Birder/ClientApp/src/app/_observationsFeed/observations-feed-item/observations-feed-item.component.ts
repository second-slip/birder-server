import { Component, Input, ViewEncapsulation } from '@angular/core';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';

@Component({
  selector: 'app-observations-feed-item',
  templateUrl: './observations-feed-item.component.html',
  styleUrls: ['./observations-feed-item.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationsFeedItemComponent {
  @Input() observation: ObservationViewModel;
  @Input() user: UserViewModel;

  constructor() { }
}
