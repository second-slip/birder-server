import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RecordingViewModel } from '@app/_models/RecordingViewModel';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecordingsService {

  constructor(private readonly _http: HttpClient) { }

  getRecordings(species: string): Observable<RecordingViewModel[]> {
    const params = new HttpParams()
      .set('species', species);

    return this._http.get<RecordingViewModel[]>('api/Recording', {params});
  }
}
