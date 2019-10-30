import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  postPhotos(model: FormData): Observable<any | ErrorReportViewModel> {
    const headers = new HttpHeaders({ 'Content-Disposition' : 'multipart/form-data' });

    return this.http.post('api/Photograph', model, { headers: headers, observe: 'events', reportProgress: true })
    .pipe(
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
