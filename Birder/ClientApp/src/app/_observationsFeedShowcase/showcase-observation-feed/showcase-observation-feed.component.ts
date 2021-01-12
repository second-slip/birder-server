import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ObservationsFeedService } from '@app/_services/observations-feed.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-showcase-observation-feed',
  templateUrl: './showcase-observation-feed.component.html',
  styleUrls: ['./showcase-observation-feed.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ShowcaseObservationFeedComponent implements OnInit {
  observations: ObservationViewModel[];
  itemHeight = 177

  constructor(private observationsFeedService: ObservationsFeedService) { }

  ngOnInit(): void {
    this.getObservations(10);
  }

  getObservations(quantity: number): void {
    this.observationsFeedService.getShowcaseObservationsFeed(quantity)
    .subscribe(
      (data: ObservationFeedDto) => {
        // this.totalItems = data.totalItems;
        this.observations = data.items;
      },
      (error: ErrorReportViewModel) => {
        console.log('bad request');
        // this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
      });
      // () => {
      // });
  }
}
