import { Injectable } from '@angular/core';
import { UserViewModel } from '../_models/UserViewModel';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getUser(username: string): Observable<UserViewModel | ErrorReportViewModel> {
    const options = username ?
      { params: new HttpParams().set('username', username) } : {};

    return this.http.get<UserViewModel>('api/User/GetUser', options)
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
