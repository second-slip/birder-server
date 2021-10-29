import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ObservationFeedPagedDto } from '@app/_models';


@Injectable({
  providedIn: 'root'
})
export class ObservationsFeedService {

  constructor(private readonly _http: HttpClient) {  }

  getObservationsFeed(pageIndex: number, filter: string): Observable<ObservationFeedPagedDto> {
    const params = new HttpParams()
      .set('pageIndex', pageIndex.toString())
      .set('filter', filter.toString());

    return this._http.get<ObservationFeedPagedDto>(`api/ObservationFeed`, { params });
  }
}
