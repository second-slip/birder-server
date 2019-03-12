import { Component, OnInit } from '@angular/core';
import { Observation } from '../../_models/Observation';
import { ObservationService } from '../observation.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';

@Component({
  selector: 'app-observation-detail',
  templateUrl: './observation-detail.component.html',
  styleUrls: ['./observation-detail.component.scss']
})
export class ObservationDetailComponent implements OnInit {
  observation: Observation;

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
        (observation: Observation) => { this.observation = observation; },
        (error: ErrorReportViewModel) => {
          this.router.navigate(['/page-not-found']);  // TODO: this is right for typing bad param, but what about server error?
        });
    // ,() => {
    //   // alert('');
    //   // 'onCompleted' callback.
    //   // No errors, route to new page here
    // }
  }

  goBack(): void {
    this.location.back();
  }

}
