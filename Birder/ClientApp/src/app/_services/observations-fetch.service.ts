import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ObservationsPagedDto } from '@app/_models/ObservationViewDto';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ObservationsFetchService {

  constructor(private http: HttpClient) { }

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
}
