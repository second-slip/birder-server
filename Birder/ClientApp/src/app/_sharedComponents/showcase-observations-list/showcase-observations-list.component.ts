import { Component, OnInit } from '@angular/core';
import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { ObservationsFeedService } from '@app/_services/observations-feed.service';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Component({
  selector: 'app-showcase-observations-list',
  templateUrl: './showcase-observations-list.component.html',
  styleUrls: ['./showcase-observations-list.component.scss']
})
export class ShowcaseObservationsListComponent implements OnInit {
  observations: ObservationViewModel[];

  constructor(private observationsFeedService: ObservationsFeedService) { }

  ngOnInit(): void {
    this.getObservations(1);
  }

  getObservations(page: number): void {
    this.observationsFeedService.getShowcaseObservationsFeed(page)
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
