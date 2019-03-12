import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Bird } from '../_models/Bird';
import { tap, catchError } from 'rxjs/operators';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';

@Injectable({
  providedIn: 'root'
})
export class BirdsService {

  constructor(private http: HttpClient
            , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getBirds(): Observable<Bird[] | ErrorReportViewModel> {
    return this.http.get<Bird[]>('api/Birds')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getBird(id: number): Observable<Bird | ErrorReportViewModel> {
    const options = id ?
    { params: new HttpParams().set('id', id.toString()) } : {};

    return this.http.get<Bird>('api/Birds/GetBird', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    console.log(message);
    // this.messageService.add(`HeroService: ${message}`);
  }
}
