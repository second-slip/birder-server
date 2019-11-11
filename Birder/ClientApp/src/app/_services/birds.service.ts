import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BirdDetailViewModel } from '../_models/BirdDetailViewModel';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { BirdSummaryViewModel, BirdsDto } from '../_models/BirdSummaryViewModel';
import { ObservationViewModel } from '../_models/ObservationViewModel';

@Injectable({
  providedIn: 'root'
})
export class BirdsService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getBirds(pageIndex: number, pageSize: number): Observable<BirdsDto[] | ErrorReportViewModel> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<BirdsDto[]>('api/Birds', { params })
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getBirdsDdl(): Observable<BirdSummaryViewModel[] | ErrorReportViewModel> {
    return this.http.get<BirdSummaryViewModel[]>('api/Birds/BirdsList')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getBird(id: number): Observable<BirdDetailViewModel | ErrorReportViewModel> {
    const options = id ?
      { params: new HttpParams().set('id', id.toString()) } : {};

    return this.http.get<BirdDetailViewModel>('api/Birds/GetBird', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getObservations(birdId: number): Observable<ObservationViewModel[] | ErrorReportViewModel> {
    const options = birdId ?
      { params: new HttpParams().set('birdId', birdId.toString()) } : {};

    return this.http.get<ObservationViewModel[]>('api/Observation/GetObservationsBySpecies', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
