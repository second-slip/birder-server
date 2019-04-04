import { Injectable } from '@angular/core';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { ObservationAnalysisViewModel, TopObservationsAnalysisViewModel } from '../_models/ObservationAnalysisViewModel';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { LifeListViewModel } from '../_models/LifeListViewModels';

@Injectable({
  providedIn: 'root'
})
export class ObservationsAnalysisService {

  constructor(private http: HttpClient
            , private httpErrorHandlerService: HttpErrorHandlerService) {
  }

  getObservationAnalysis(): Observable<ObservationAnalysisViewModel | ErrorReportViewModel> {
    return this.http.get<ObservationAnalysisViewModel>('api/ObservationAnalysis/GetObservationAnalysis')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getTopObservationsAnalysis(): Observable<TopObservationsAnalysisViewModel | ErrorReportViewModel> {
    return this.http.get<TopObservationsAnalysisViewModel>('api/ObservationAnalysis/GetTopObservationAnalysis')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getLifeList(): Observable<LifeListViewModel | ErrorReportViewModel> {
    return this.http.get<LifeListViewModel>('api/ObservationAnalysis/GetLifeList')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
