import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { HttpErrorHandlerService } from './http-error-handler.service';

@Injectable({
  providedIn: 'root'
})
export class PhotosService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  postPhotos(formData: File[]): Observable<any | ErrorReportViewModel> {
    return this.http.post('api/Manage/UploadAvatar', formData, { reportProgress: true, observe: 'events' })
    .pipe(
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
