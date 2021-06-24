import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';
import { ObservationsPagedDto } from '@app/_models/ObservationViewDto';
import { ObservationsFetchService } from '@app/_services/observations-fetch.service';

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

  constructor(private service: ObservationsFetchService) { }

  ngOnInit(): void {
    this.getObservations();
  }

  changePage(): void {
    this.getObservations();
  }

  getObservations(): void {
    this.observations$ = this.service.getObservationsByBirdSpecies(this.birdId, this.page, this.pageSize)
      .pipe(share(),
        catchError(error => {
          this.errorObject = error;
          return throwError(error);
        })
      );
  }
}
