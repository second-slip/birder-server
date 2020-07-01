import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';
import { ObservationFeedFilter } from '@app/_models/ObservationFeedFilter';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

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

    return this.http.get<ObservationFeedDto>(`api/ObservationFeed`, { params })
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getShowcaseObservationsFeed(pageIndex: number): Observable<ObservationFeedDto | ErrorReportViewModel> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString());
      // .set('filter', filter.toString());

    return this.http.get<ObservationFeedDto>(`api/GetShowcaseObservationsFeed`, { params })
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
