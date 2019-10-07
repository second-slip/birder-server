import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ObservationViewModel } from '../../_models/ObservationViewModel';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { ObservationsFeedService } from '../observations-feed.service';

@Component({
  selector: 'app-observation-feed',
  templateUrl: './observation-feed.component.html',
  styleUrls: ['./observation-feed.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ObservationFeedComponent implements OnInit {
  observations: ObservationViewModel[];

  constructor(private observationsFeedService: ObservationsFeedService
    , private router: Router) { }

  ngOnInit() {
    this.getObservations();
  }

  getObservations(): void {
    this.observationsFeedService.getObservationsFeed()
      .subscribe(
        (response: ObservationViewModel[]) => {
          this.observations = response;
        },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);
        });
  }
}
