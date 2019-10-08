import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpErrorHandlerService } from '../_services/http-error-handler.service';
import { Observable } from 'rxjs';
import { ObservationViewModel } from '../_models/ObservationViewModel';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ObservationsFeedService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) {
  }

  getObservationsFeed(): Observable<ObservationViewModel[] | ErrorReportViewModel> {
    return this.http.get<ObservationViewModel[]>('api/ObservationFeed')
      .pipe(
        // (take(1)),
        // tap(observations => this.log('fetched observations')),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
