import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { UserProfileViewModel } from '@app/_models/UserProfileViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

@Injectable({
  providedIn: 'root'
})

export class UserProfileService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getUserProfile(username: string): Observable<UserProfileViewModel | ErrorReportViewModel> {
    const options = username ?
      { params: new HttpParams().set('requestedUsername', username) } : {};

    return this.http.get<UserProfileViewModel>('api/UserProfile', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
