import { Component, Input, OnInit } from '@angular/core';
import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';

@Component({
  selector: 'app-observations-feed-item',
  templateUrl: './observations-feed-item.component.html',
  styleUrls: ['./observations-feed-item.component.scss']
})
export class ObservationsFeedItemComponent implements OnInit {
  @Input() observation: ObservationFeedDto;

  constructor() { }

  ngOnInit(): void { }

}
