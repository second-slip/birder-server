import { Injectable } from '@angular/core';
import { IXenoCantoResponse } from './_models/IXenoCantoResponse';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { environment } from 'environments/environment';

@Injectable({
  providedIn: 'root'
})
export class XenoCantoService {
  corsAnywhereUrl = environment.corsAnywhereUrl;
  xenoCantoApiBaseUrl = environment.xenoCantoApiBaseUrl;
  recordingLength = '+len_gt:40';

  constructor(private http: HttpClient) { }

  getRecordings(searchTerm: string): Observable<IXenoCantoResponse> {
    return this.http.get<IXenoCantoResponse>(`${this.corsAnywhereUrl}${this.xenoCantoApiBaseUrl}${this.formatSearchTerm(searchTerm)}${this.recordingLength}`)
      .pipe(
        map(o => ({
          numRecordings: o.numRecordings,
          numSpecies: o.numSpecies,
          page: o.page,
          numPages: o.numPages,
          recordings: o.recordings
        }))
      );
  }

  formatSearchTerm(searchTerm: string): string {
    return searchTerm.split(' ').join('+');
  }
}
