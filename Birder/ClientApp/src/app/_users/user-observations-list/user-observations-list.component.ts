import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { ObservationService } from '@app/_sharedServices/observation.service';
import { ObservationDto } from '@app/_models/ObservationFeedDto';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';

@Component({
  selector: 'app-user-observations-list',
  templateUrl: './user-observations-list.component.html',
  styleUrls: ['./user-observations-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserObservationsListComponent implements OnInit {
  observations$: Observable<ObservationDto>;
  @Input() username: string;
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
    this.observations$ = this.observationsService.getObservationsByUser(this.username, this.page, this.pageSize)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        }));
  }
}
