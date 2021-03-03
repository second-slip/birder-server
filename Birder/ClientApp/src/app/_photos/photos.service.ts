import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { HttpErrorHandlerService } from '../_services/http-error-handler.service';
import { PhotographDto } from '@app/_models/PhotographDto';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getPhotos(observationId: number): Observable<PhotographDto[] | ErrorReportViewModel> {
    const options = observationId ? { params: new HttpParams().append('observationId', observationId.toString()) } : {};

    return this.http.get<any>('api/Photograph/GetPhotographs', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postDeletePhoto(model: FormData): Observable<any | ErrorReportViewModel> {
    const headers = new HttpHeaders({ 'Content-Disposition': 'multipart/form-data' });

    return this.http.post('api/Photograph/DeletePhotograph', model, { headers: headers })
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postPhotos(model: FormData): Observable<any | ErrorReportViewModel> {
    const headers = new HttpHeaders({ 'Content-Disposition': 'multipart/form-data' });

    return this.http.post('api/Photograph', model, { headers: headers, observe: 'events', reportProgress: true })
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
