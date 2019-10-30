import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { UploadPhotosDto } from '@app/_models/UploadPhotosDto';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Disposition' : 'multipart/form-data' })
  // reportProgress: true , observe: 'events'
};

// reportProgress: true, observe: 'events'

@Injectable({
  providedIn: 'root'
})
export class PhotosService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }


    // const httpOptions = {
    //   headers: new HttpHeaders({ 'Content-Disposition' : 'multipart/form-data' }),
    // };

  postPhotos(model: FormData): Observable<any | ErrorReportViewModel> {
    return this.http.post('api/Photograph', model, httpOptions)
    .pipe(
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
