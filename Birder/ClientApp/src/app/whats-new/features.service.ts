import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { IFeatures } from './ifeatures.data';

@Injectable({
  providedIn: 'root'
})
export class FeaturesService {

  private readonly items$: BehaviorSubject<IFeatures[]> = new BehaviorSubject<IFeatures[]>([]);

  constructor(private readonly _http: HttpClient) { }

  getFeatures(): Observable<IFeatures[]> {
    return this._http.get<IFeatures[]>("./assets/new-features.json");
  }

  fetchList() {
    this._http.get<IFeatures[]>("./assets/new-features.json")
      .subscribe(receivedItems => this.items$.next(receivedItems));
  }

  get items(): Observable<IFeatures[]> {
    return this.items$.asObservable();
  }
}