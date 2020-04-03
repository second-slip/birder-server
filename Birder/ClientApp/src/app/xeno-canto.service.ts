import { Injectable } from '@angular/core';
import { IXenoCantoResponse, IRecording } from './_models/IXenoCantoResponse';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
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
          recordings: o.recordings.map((element: IRecording, index) => ({
            id: index,
            url: `${element['sono']['full'].substr(0, this.getPosition(element['sono']['full'], '\/', 6))}${element['file-name']}`
          }))
        }))
      );
  }

  getPosition(stringa, subString, index) {
    return stringa.split(subString, index).join(subString).length + 1;
  }

  formatSearchTerm(searchTerm: string): string {
    return searchTerm.split(' ').join('+');
  }
}

// This was previously used to map the recordings array to IMappedRecordings:
  // poop(data) {
  //   const urls: IVoice[] = [];
  //   data.length = 10;

  //   data.forEach((element, index) => {
  //     let sub = element.sono['full'].substr(0, this.getPosition(element.sono['full'], '\/', 6));
  //     urls.push({
  //       id: index + 1,
  //       url: `${sub}${element['file-name']}`
  //     });
  //   });
  //   return urls;
  // }
