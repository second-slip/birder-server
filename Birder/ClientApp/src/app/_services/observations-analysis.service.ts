import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { LifeListViewModel } from '@app/_models/LifeListViewModels';
import { ObservationAnalysisViewModel, TopObservationsAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { HttpErrorHandlerService } from './http-error-handler.service';

@Injectable({
  providedIn: 'root'
})
export class ObservationsAnalysisService {

  constructor(private http: HttpClient
            , private httpErrorHandlerService: HttpErrorHandlerService) {
  }

  getObservationAnalysis(username: string): Observable<ObservationAnalysisViewModel | ErrorReportViewModel> {
    const options = username ?
    { params: new HttpParams().set('requestedUsername', username) } : {};

    return this.http.get<ObservationAnalysisViewModel>('api/ObservationAnalysis/GetObservationAnalysis', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getTopObservationsAnalysis(): Observable<TopObservationsAnalysisViewModel | ErrorReportViewModel> {
    return this.http.get<TopObservationsAnalysisViewModel>('api/ObservationAnalysis/GetTopObservationAnalysis')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getLifeList(): Observable<LifeListViewModel[] | ErrorReportViewModel> {
    return this.http.get<LifeListViewModel[]>('api/ObservationAnalysis/GetLifeList')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
