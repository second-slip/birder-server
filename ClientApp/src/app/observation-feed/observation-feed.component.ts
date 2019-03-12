import { Component, OnInit } from '@angular/core';
import { Observation } from '../../_models/Observation';
import { ObservationService } from '../observation.service';
import { Router } from '@angular/router';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-observation-feed',
  templateUrl: './observation-feed.component.html',
  styleUrls: ['./observation-feed.component.scss']
})
export class ObservationFeedComponent implements OnInit {

  observations: Observation[];

  constructor(private observationService: ObservationService
    , private router: Router) { }

  ngOnInit() {
    this.getObservations();
  }

  getObservations(): void {
    this.observationService.getObservations()
    .subscribe(
      (response: Observation[]) => { this.observations = response; },
      (error: ErrorReportViewModel) => {
        this.router.navigate(['/page-not-found']);
      });
  }
}
