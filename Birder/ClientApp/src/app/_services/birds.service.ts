import { Injectable } from '@angular/core';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { BirdSummaryViewModel, BirdsDto } from '@app/_models/BirdSummaryViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { HttpParams, HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { BirdDetailViewModel } from '@app/_models/BirdDetailViewModel';
import { BirderStatus } from '@app/_models/BirdIndexOptions';
import { RecordingViewModel } from '@app/_models/RecordingViewModel';

@Injectable({
  providedIn: 'root'
})
export class BirdsService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getBirds(pageIndex: number, pageSize: number, speciesFilter: string): Observable<BirdsDto | ErrorReportViewModel> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString())
      .set('speciesFilter', speciesFilter.toString());

    return this.http.get<BirdsDto>('api/Birds', { params })
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
}
