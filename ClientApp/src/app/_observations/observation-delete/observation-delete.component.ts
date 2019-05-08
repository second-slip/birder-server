import { Component, OnInit } from '@angular/core';
import { ObservationViewModel } from '../../../_models/ObservationViewModel';
import { ObservationService } from '../../observation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorReportViewModel } from '../../../_models/ErrorReportViewModel';
import { Location } from '@angular/common';

@Component({
  selector: 'app-observation-delete',
  templateUrl: './observation-delete.component.html',
  styleUrls: ['./observation-delete.component.scss']
})
export class ObservationDeleteComponent implements OnInit {
  observation: ObservationViewModel;
  errorReport: ErrorReportViewModel;

  constructor(private observationService: ObservationService
    , private route: ActivatedRoute
    , private location: Location
    , private router: Router) { }

  ngOnInit(): void {
    this.getObservation();
  }

  getObservation(): void {
    const id = +this.route.snapshot.paramMap.get('id');

    this.observationService.getObservation(id)
      .subscribe(
        (observation: ObservationViewModel) => { this.observation = observation; },
        (error: ErrorReportViewModel) => {
          this.errorReport = error;
        });
  }

  deleteObservation(): void {
    this.observationService.deleteObservation(this.observation.observationId)
      .subscribe(_ => {
          this.router.navigate(['/observation-feed']);
        },
        (error: ErrorReportViewModel) => {
          this.errorReport = error;
        });
  }

  goBack(): void {
    this.location.back();
  }
}
