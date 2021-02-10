import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { RecordingViewModel } from '@app/_models/RecordingViewModel';
import { HttpErrorHandlerService } from '@app/_services/http-error-handler.service';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RecordingsService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getRecordings(species: string): Observable<RecordingViewModel[] | ErrorReportViewModel> {
    const params = new HttpParams()
      .set('species', species);

    return this.http.get<RecordingViewModel[]>('api/Recording', {params})
    .pipe(
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
