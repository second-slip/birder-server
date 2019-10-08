import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { TweetDay } from '../_models/TweetDay';

@Injectable({
  providedIn: 'root'
})
export class TweetsService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getTweetDay(): Observable<TweetDay | ErrorReportViewModel> {
    return this.http.get<TweetDay>('api/Tweets/GetTweetDay')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
