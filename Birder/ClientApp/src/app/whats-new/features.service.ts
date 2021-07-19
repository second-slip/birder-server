import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IFeatures } from './whats-new.component';

@Injectable({
  providedIn: 'root'
})
export class FeaturesService {

  constructor(private readonly _http: HttpClient) { }

  getFeatures(): Observable<IFeatures[]> {
    return this._http.get<IFeatures[]>("./assets/new-features.json");
  }
}
