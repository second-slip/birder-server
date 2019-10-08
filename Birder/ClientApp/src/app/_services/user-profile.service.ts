import { Injectable } from '@angular/core';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UserProfileViewModel } from '../_models/UserProfileViewModel';
import { Observable } from 'rxjs';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';

// const httpOptions = {
//   headers: new HttpHeaders({ 'Content-Type': 'application/json' })
// };

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
