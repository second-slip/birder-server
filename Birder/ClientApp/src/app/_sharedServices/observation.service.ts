import { Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { tap, catchError, first } from 'rxjs/operators';
import { HttpErrorHandlerService } from '../_services/http-error-handler.service';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { ObservationAddDto, ObservationViewModel } from '../_models/ObservationViewModel';
import { ObservationsPagedDto } from '@app/_models/ObservationViewDto';

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
      .pipe(first());
  }

  addObservation(viewModel: ObservationAddDto): Observable<ObservationViewModel | ErrorReportViewModel> {
    return this.http.post<ObservationViewModel>('api/Observation/CreateObservation', viewModel, httpOptions)
      .pipe(
        tap(() => { this.announceObservationsChanged(); }),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  updateObservation(id: number, viewModel: ObservationViewModel): Observable<ObservationViewModel | ErrorReportViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } :
      { headers: new HttpHeaders({ 'Content-Type': 'application/json' }) };

    return this.http.put<ObservationViewModel>('api/Observation/UpdateObservation', viewModel, options)
      .pipe(
        tap(() => { this.announceObservationsChanged(); }),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  deleteObservation(id: string): Observable<ObservationViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } : {};

    return this.http.delete<ObservationViewModel>('api/Observation/DeleteObservation', options)
      .pipe(first(),
        tap(() => { this.announceObservationsChanged(); }));
  }

  getObservationsByBirdSpecies(birdId: number, pageIndex: number, pageSize: number): Observable<ObservationsPagedDto> {
    const params = new HttpParams()
      .set('birdId', birdId.toString())
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<ObservationsPagedDto>('api/ObservationQuery/GetObservationsByBirdSpecies', { params })
      .pipe(first());
  }

  getObservationsByUser(username: string, pageIndex: number, pageSize: number): Observable<ObservationsPagedDto> {
    const params = new HttpParams()
      .set('username', username)
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<ObservationsPagedDto>(`api/ObservationQuery/GetObservationsByUser`, { params })
      .pipe(first());
  }

  private announceObservationsChanged(): void {
    this.observationsChanged.next();
  }
}
