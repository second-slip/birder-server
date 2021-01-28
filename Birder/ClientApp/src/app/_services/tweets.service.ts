import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { TweetArchiveDto, TweetDay } from '../_models/TweetDay';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Injectable({
  providedIn: 'root'
})
export class TweetsService {

  constructor(private http: HttpClient, private httpErrorHandlerService: HttpErrorHandlerService) { }

  getTweetDay(): Observable<TweetDay | ErrorReportViewModel> {
    return this.http.get<TweetDay>('api/Tweets/GetTweetDay')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  getTweetArchive(pageIndex: number, pageSize: number): Observable<TweetArchiveDto | ErrorReportViewModel> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<TweetArchiveDto>('api/Tweets/GetTweetArchive', { params })
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
