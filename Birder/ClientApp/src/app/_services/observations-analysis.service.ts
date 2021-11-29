import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LifeListViewModel } from '@app/_models/LifeListViewModels';
import { ObservationAnalysisViewModel, TopObservationsAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';

@Injectable({
  providedIn: 'root'
})
export class ObservationsAnalysisService {

  constructor(private readonly _http: HttpClient) { }

  getObservationAnalysis(username: string): Observable<ObservationAnalysisViewModel> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this._http.get<ObservationAnalysisViewModel>('api/ObservationAnalysis/GetObservationAnalysis', options);
  }

  // getTopObservationsAnalysis(): Observable<TopObservationsAnalysisViewModel> {
  //   return this._http.get<TopObservationsAnalysisViewModel>('api/List/GetTopObservationsList');
  // }

  getLifeList(): Observable<LifeListViewModel[]> {
    return this._http.get<LifeListViewModel[]>('api/List/GetLifeList');
  }
}
