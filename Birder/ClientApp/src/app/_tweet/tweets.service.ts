import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TweetArchiveDto, TweetDay } from '../_models/TweetDay';
import { first } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class TweetsService {

  constructor(private http: HttpClient) { }

  getTweetDay(): Observable<TweetDay> {
    return this.http.get<TweetDay>('api/Tweets/GetTweetDay')
      .pipe(first());
  }

  getTweetArchive(pageIndex: number, pageSize: number): Observable<TweetArchiveDto> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<TweetArchiveDto>('api/Tweets/GetTweetArchive', { params })
      .pipe(first());
  }
}
