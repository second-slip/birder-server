import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ObservationAnalysisViewModel } from '@app/_models';
import { BehaviorSubject, finalize, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ObservationCountService {

  private readonly _isError$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private readonly _isLoading$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  private readonly _observationCount$: BehaviorSubject<ObservationAnalysisViewModel | null> = new BehaviorSubject<ObservationAnalysisViewModel | null>(null);

  constructor(private readonly _httpClient: HttpClient) { }

  public get isError(): Observable<boolean> {
    return this._isError$.asObservable();
  }

  public get isLoading(): Observable<boolean> {
    return this._isLoading$.asObservable();
  }

  public get getCount(): Observable<ObservationAnalysisViewModel> {
    return this._observationCount$.asObservable();
  }

  public getData(): void {

    this._isLoading$.next(true);

    this._httpClient.get<ObservationAnalysisViewModel>('api/ObservationAnalysis')
      .pipe(finalize(() => { this._isLoading$.next(false); }))
      .subscribe({
        next: (response) => {
          this._observationCount$.next(response);
        },
        error: (e) => { this._handleError(e); }
      })
  }

  private _handleError(error: any) { // no need to send error to the component...
    //console.log(error);
    this._isError$.next(true);
  }
}
