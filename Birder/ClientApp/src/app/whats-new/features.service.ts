import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { first } from 'rxjs/operators';
import { IFeatures } from './whats-new.component';

@Injectable({
  providedIn: 'root'
})
export class FeaturesService {

  constructor(private http: HttpClient) { }

  getFeatures(): Observable<IFeatures[]> {
    return this.http.get<IFeatures[]>("./assets/new-features.json").pipe(first());
  }

  wakeyWakey(): Observable<void> {
    return this.http.get<void>("api/Home").pipe(first());
  }
}
