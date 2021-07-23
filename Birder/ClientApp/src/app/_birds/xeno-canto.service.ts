import { Injectable } from '@angular/core';
import { IXenoCantoResponse, IRecording } from '../_models/IXenoCantoResponse';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

// currently not used as the xeno-canto API blocks CORS
// Public demo server (cors-anywhere.herokuapp.com) will be very limited by January 2021, 31st
// see: https://github.com/Rob--W/cors-anywhere/issues/301
// recordings are got through RecordingsService from server


@Injectable({
  providedIn: 'root'
})
export class XenoCantoService {
  private readonly corsAnywhereUrl = 'https://cors-anywhere.herokuapp.com/';
  private readonly xenoCantoApiBaseUrl = 'https://www.xeno-canto.org/api/2/recordings?query=';
  private readonly recordingLength = '+len_gt:40';

  constructor(private readonly _http: HttpClient) { }

  getRecordings(searchTerm: string): Observable<IXenoCantoResponse> {
    return this._http.get<IXenoCantoResponse>(`${this.corsAnywhereUrl}${this.xenoCantoApiBaseUrl}${this.formatSearchTerm(searchTerm)}${this.recordingLength}`)
      .pipe(
        map(o => ({
          numRecordings: o.numRecordings,
          numSpecies: o.numSpecies,
          page: o.page,
          numPages: o.numPages,
          recordings: o.recordings.map((element: IRecording, index) => ({
            id: index,
            url: element.url
            //url: `${element['sono']['full'].substr(0, this.getSubStringStartPosition(element['sono']['full'], '\/', 6))}${element['file-name']}`
          }))
        }))
      );
  }

  private getSubStringStartPosition(inputString, subString, index): number {
    return (inputString.split(subString, index).join(subString).length) + 1;
  }

  private formatSearchTerm(searchTerm: string): string {
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
