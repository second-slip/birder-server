import { Component, OnInit } from '@angular/core';
import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';

@Component({
  selector: 'app-showcase-observations-list',
  templateUrl: './showcase-observations-list.component.html',
  styleUrls: ['./showcase-observations-list.component.scss']
})
export class ShowcaseObservationsListComponent implements OnInit {
  observations: ObservationViewModel[];

  constructor() { }

  ngOnInit(): void {
  }

}
