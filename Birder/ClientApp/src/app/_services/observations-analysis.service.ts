import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LifeListViewModel } from '@app/_models/LifeListViewModels';
import { ObservationAnalysisViewModel, TopObservationsAnalysisViewModel } from '@app/_models/ObservationAnalysisViewModel';

@Injectable({
  providedIn: 'root'
})
export class ObservationsAnalysisService {

  constructor(private http: HttpClient) {
  }

  getObservationAnalysis(username: string): Observable<ObservationAnalysisViewModel> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this.http.get<ObservationAnalysisViewModel>('api/ObservationAnalysis/GetObservationAnalysis', options)
      .pipe();
  }

  getTopObservationsAnalysis(): Observable<TopObservationsAnalysisViewModel> {
    return this.http.get<TopObservationsAnalysisViewModel>('api/ObservationAnalysis/GetTopObservationAnalysis')
      .pipe();
  }

  getLifeList(): Observable<LifeListViewModel[]> {
    return this.http.get<LifeListViewModel[]>('api/ObservationAnalysis/GetLifeList')
      .pipe();
  }
}
