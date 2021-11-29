import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TopObservationsAnalysisViewModel } from '@app/_models/observationanalysisviewmodel';
import { BehaviorSubject, finalize, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ObservationTopService {

  private readonly _isError$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private readonly _isLoading$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  private readonly _topObservations$: BehaviorSubject<TopObservationsAnalysisViewModel | null> = new BehaviorSubject<TopObservationsAnalysisViewModel | null>(null);

  constructor(private readonly _httpClient: HttpClient) { }

  public get isError(): Observable<boolean> {
    return this._isError$.asObservable();
  }

  public get isLoading(): Observable<boolean> {
    return this._isLoading$.asObservable();
  }

  public get getTop(): Observable<TopObservationsAnalysisViewModel> {
    return this._topObservations$.asObservable();
  }

  public getData(): void {

    this._isLoading$.next(true);

    this._httpClient.get<TopObservationsAnalysisViewModel>('api/List/GetTopObservationsList')
      .pipe(finalize(() => { this._isLoading$.next(false); }))
      .subscribe({
        next: (response) => {
          this._topObservations$.next(response);
        },
        error: (e) => { this._handleError(e); }
      })
  }

  private _handleError(error: any) { // no need to send error to the component...
    //console.log(error);
    this._isError$.next(true);
  }
}

