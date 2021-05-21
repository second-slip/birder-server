import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { ObservationFeedPagedDto } from '@app/_models/ObservationFeedDto';

@Injectable({
  providedIn: 'root'
})
export class ObservationsFeedService {

  constructor(private http: HttpClient) {  }

  getObservationsFeed(pageIndex: number, filter: string): Observable<ObservationFeedPagedDto> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('filter', filter.toString());

    return this.http.get<ObservationFeedPagedDto>(`api/ObservationFeed`, { params })
      .pipe(first());
  }
}
