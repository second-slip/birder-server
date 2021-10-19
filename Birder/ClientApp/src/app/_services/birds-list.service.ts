import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BirdsListService {

  //private cache = [];
  private items$: BehaviorSubject<BirdSummaryViewModel[]> = new BehaviorSubject<BirdSummaryViewModel[]>([]);

  constructor(private readonly _http: HttpClient) { }

  fetchList() {
    this._http.get<BirdSummaryViewModel[]>('api/Birds/BirdsList')
      .subscribe(receivedItems => this.items$.next(receivedItems));
  }

  get items(): Observable<BirdSummaryViewModel[]> {
    return this.items$.asObservable();
  }

  // getBirdsList(): Observable<BirdSummaryViewModel[]> {
  //   let response = this._http.get<BirdSummaryViewModel[]>('api/Birds/BirdsList');
  //   this.cache = response;
  //   return response;
  // }

  // ORIGINAL
  // getBirdsDdl(): Observable<BirdSummaryViewModel[]> {
  //   return this._http.get<BirdSummaryViewModel[]>('api/Birds/BirdsList');
  // }


}
