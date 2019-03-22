import { Injectable } from '@angular/core';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { ObservationAnalysisViewModel } from '../_models/ObservationAnalysisViewModel';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ObservationsAnalysisService {

  constructor(private http: HttpClient
            , private httpErrorHandlerService: HttpErrorHandlerService) {
  }

  getObservationAnalysis(): Observable<ObservationAnalysisViewModel | ErrorReportViewModel> {
    return this.http.get<ObservationAnalysisViewModel>('api/ObservationAnalysis/GetObservationsAnalysis')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
