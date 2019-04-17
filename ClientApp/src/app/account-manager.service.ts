import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { ManageProfileViewModel } from '../_models/ManageProfileViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AccountManagerService {

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  getUserProfile(): Observable<ManageProfileViewModel | ErrorReportViewModel> {
    return this.http.get<ManageProfileViewModel>('api/Manage/GetUserProfile')
      .pipe(
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postUpdateProfile(viewModel: ManageProfileViewModel): Observable<ManageProfileViewModel | ErrorReportViewModel> {
    return this.http.post<ManageProfileViewModel>('api/User/UpdateProfile', viewModel, httpOptions)
    .pipe(
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
