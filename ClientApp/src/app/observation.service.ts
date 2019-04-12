import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { tap, catchError } from 'rxjs/operators';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { ObservationViewModel } from '../_models/ObservationViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class ObservationService {

  private observationsChanged = new Subject<any>();

  observationsChanged$ = this.observationsChanged.asObservable();

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) {

  }

  getObservations(): Observable<ObservationViewModel[] | ErrorReportViewModel> {
    return this.http.get<ObservationViewModel[]>('api/Observation')
      .pipe(
        // tap(observations => this.log('fetched observations')),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getObservation(id: number): Observable<ObservationViewModel | ErrorReportViewModel> {
    const options = id ?
      { params: new HttpParams().append('id', id.toString()) } : {};

    return this.http.get<ObservationViewModel>('api/Observation/GetObservation', options)
      .pipe(
        // tap(observation => this.log(`fetched observation with id: ${observation.observationId}`)),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  addObservation(viewModel: ObservationViewModel): Observable<ObservationViewModel | ErrorReportViewModel> {
    return this.http.post<ObservationViewModel>('api/Observation/PostObservation', viewModel, httpOptions)
      .pipe(
        tap(observations => { this.announceObservationsChanged(); }),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  updateObservation(id: number, viewModel: ObservationViewModel): Observable<ObservationViewModel | ErrorReportViewModel> {
    // alert(id);
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } :
      { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

    return this.http.put<ObservationViewModel>('api/Observation/UpdateObservation', viewModel, options)
      .pipe(
        tap(observations => { this.announceObservationsChanged(); }),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  deleteObservation(id: number): Observable<ObservationViewModel | ErrorReportViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } : {};

    return this.http.delete<ObservationViewModel>('api/Observation/DeleteObservation', options)
      .pipe(
        tap(observations => { this.announceObservationsChanged(); }),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  announceObservationsChanged(): void {
    this.observationsChanged.next();
  }
}
