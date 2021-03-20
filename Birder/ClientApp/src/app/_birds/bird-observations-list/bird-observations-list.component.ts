import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { ObservationService } from '@app/_sharedServices/observation.service';
import { ObservationDto } from '@app/_models/ObservationFeedDto';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';
import { ObservationsPagedDto } from '@app/_models/ObservationViewDto';

@Component({
  selector: 'app-bird-observations-list',
  templateUrl: './bird-observations-list.component.html',
  styleUrls: ['./bird-observations-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BirdObservationsListComponent implements OnInit {
  @Input() birdId: number;

  observations$: Observable<ObservationsPagedDto>;
  public errorObject = null;
  page = 1;
  pageSize = 10;

  constructor(private observationsService: ObservationService) { }

  ngOnInit(): void {
    this.getObservations();
  }

  changePage(): void {
    this.getObservations();
  }

  getObservations(): void {
    this.observations$ = this.observationsService.getObservationsByBirdSpecies(this.birdId, this.page, this.pageSize)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        }));
  }
}
