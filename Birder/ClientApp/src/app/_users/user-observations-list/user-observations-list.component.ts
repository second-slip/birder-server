import { Component, OnInit, Input, ViewEncapsulation } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, share } from 'rxjs/operators';
import { ObservationsPagedDto } from '@app/_models/ObservationViewDto';
import { ObservationsFetchService } from '@app/_services/observations-fetch.service';

@Component({
  selector: 'app-user-observations-list',
  templateUrl: './user-observations-list.component.html',
  styleUrls: ['./user-observations-list.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UserObservationsListComponent implements OnInit {
  observations$: Observable<ObservationsPagedDto>;
  @Input() username: string;
  public errorObject = null;
  page = 1;
  pageSize = 10;

  constructor(private readonly _service: ObservationsFetchService) { }

  ngOnInit(): void {
    this.getObservations();
  }

  changePage(): void {
    this.getObservations();
  }

  private getObservations(): void {
    this.observations$ = this._service.getObservationsByUser(this.username, this.page, this.pageSize)
      .pipe(share(),
        catchError(err => {
          this.errorObject = err;
          return throwError(err);
        }));
  }
}
