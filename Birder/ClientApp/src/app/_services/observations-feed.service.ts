import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { ObservationViewModel } from '../_models/ObservationViewModel';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';
import { ObservationFeedFilter } from '@app/_models/FilterFeedOptions';

@Injectable({
  providedIn: 'root'
})
export class ObservationsFeedService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) {
  }

  getObservationsFeed(pageIndex: number, filter: ObservationFeedFilter): Observable<ObservationFeedDto | ErrorReportViewModel> {
    const params = new HttpParams()
    .set('pageIndex', pageIndex.toString())
    .set('filter', filter.toString());

    return this.http.get<ObservationFeedDto>(`api/ObservationFeed`, {params})
      .pipe(
        // (take(1)),
        // tap(observations => this.log('fetched observations')),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  // getObservationsFeed(): Observable<ObservationViewModel[] | ErrorReportViewModel> {
  //   return this.http.get<ObservationViewModel[]>('api/ObservationFeed')
  //     .pipe(
  //       // (take(1)),
  //       // tap(observations => this.log('fetched observations')),
  //       catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  // }
}
