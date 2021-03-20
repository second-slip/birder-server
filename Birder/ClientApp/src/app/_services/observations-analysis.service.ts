import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LifeListViewModel } from '@app/_models/LifeListViewModels';
import { ObservationAnalysisViewModel, TopObservationsAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';
import { first } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ObservationsAnalysisService {

  constructor(private http: HttpClient) { }

  getObservationAnalysis(username: string): Observable<ObservationAnalysisViewModel> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this.http.get<ObservationAnalysisViewModel>('api/ObservationAnalysis/GetObservationAnalysis', options)
      .pipe(first());
  }

  getTopObservationsAnalysis(): Observable<TopObservationsAnalysisViewModel> {
    return this.http.get<TopObservationsAnalysisViewModel>('api/List/GetTopObservationsList')
      .pipe(first());
  }

  getLifeList(): Observable<LifeListViewModel[]> {
    return this.http.get<LifeListViewModel[]>('api/List/GetLifeList')
      .pipe(first());
  }
}
