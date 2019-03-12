import { Component, OnInit } from '@angular/core';
import { ObservationViewModel } from '../../_models/ObservationViewModel';
import { ObservationService } from '../observation.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-observation-feed',
  templateUrl: './observation-feed.component.html',
  styleUrls: ['./observation-feed.component.scss']
})
export class ObservationFeedComponent implements OnInit {

  observations: ObservationViewModel[];

  constructor(private observationService: ObservationService
    , private router: Router) { }

  ngOnInit() {
    this.getObservations();
  }

  getObservations(): void {
    this.observationService.getObservations()
    .subscribe(
      (response: ObservationViewModel[]) => { this.observations = response; },
      (error: ErrorReportViewModel) => {
        this.router.navigate(['/page-not-found']);
      });
  }
}
