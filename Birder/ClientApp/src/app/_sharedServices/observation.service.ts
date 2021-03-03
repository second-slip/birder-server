import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { tap, catchError } from 'rxjs/operators';
import { HttpErrorHandlerService } from '../_services/http-error-handler.service';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { ObservationViewModel } from '../_models/ObservationViewModel';
import { ObservationDto } from '@app/_models/ObservationFeedDto';

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

  getObservation(id: string): Observable<ObservationViewModel> {
    const options = id ?
      { params: new HttpParams().append('id', id.toString()) } : {};

    return this.http.get<ObservationViewModel>('api/Observation/GetObservationDetail', options)
      .pipe();
        // tap(observation => this.log(`fetched observation with id: ${observation.observationId}`)),
        //catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  addObservation(viewModel: ObservationViewModel): Observable<ObservationViewModel | ErrorReportViewModel> {
    return this.http.post<ObservationViewModel>('api/Observation/CreateObservation', viewModel, httpOptions)
      .pipe(
        tap(observations => { this.announceObservationsChanged(); }),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  updateObservation(id: number, viewModel: ObservationViewModel): Observable<ObservationViewModel | ErrorReportViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } :
      { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

    return this.http.put<ObservationViewModel>('api/Observation/UpdateObservation', viewModel, options)
      .pipe(
        tap(_ => { this.announceObservationsChanged(); }),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  deleteObservation(id: number): Observable<ObservationViewModel | ErrorReportViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } : {};

    return this.http.delete<ObservationViewModel>('api/Observation/DeleteObservation', options)
      .pipe(
        tap(_ => { this.announceObservationsChanged(); }),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getObservationsByBirdSpecies(birdId: number, pageIndex: number, pageSize: number): Observable<ObservationDto | ErrorReportViewModel> {
    const params = new HttpParams()
      .set('birdId', birdId.toString())
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<ObservationDto>('api/Observation/GetObservationsByBirdSpecies', { params })
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getObservationsByUser(username: string, pageIndex: number, pageSize: number): Observable<ObservationDto | ErrorReportViewModel> {
    const params = new HttpParams()
      .set('username', username)
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<ObservationDto>(`api/Observation/GetObservationsByUser`, { params })
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getObservationsShowcase(): Observable<ObservationDto | ErrorReportViewModel> {
    return this.http.get<ObservationDto>(`api/Observation/GetObservationsByUser`, httpOptions)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  private announceObservationsChanged(): void {
    this.observationsChanged.next();
  }
}
