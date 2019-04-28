import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BirdDetailViewModel } from '../_models/BirdDetailViewModel';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { BirdSummaryViewModel } from '../_models/BirdSummaryViewModel';
import { BirderStatus } from '../_models/BirdIndexOptions';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class BirdsService {

  constructor(private http: HttpClient
            , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getBirds(filter: BirderStatus): Observable<BirdSummaryViewModel[] | ErrorReportViewModel> {
    const options = filter ?
    { params: new HttpParams().set('filter', filter.toString()) } : {};

    return this.http.get<BirdSummaryViewModel[]>('api/Birds', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  // getPagedBirds(pageIndex: number, pageSize: number): Observable<PagedResult<BirdSummaryViewModel> | ErrorReportViewModel> {
  //   const options = 
  //   { params:  new HttpParams().set('pageIndex', pageIndex.toString()).set('pageSize', pageSize.toString()) };

  //   return this.http.get<PagedResult<BirdSummaryViewModel>>('api/Birds', options)
  //     .pipe(
  //       catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  // }

  getBird(id: number): Observable<BirdDetailViewModel | ErrorReportViewModel> {
    const options = id ?
    { params: new HttpParams().set('id', id.toString()) } : {};

    return this.http.get<BirdDetailViewModel>('api/Birds/GetBird', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
