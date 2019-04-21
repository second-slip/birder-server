import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { catchError } from 'rxjs/operators';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { Observable } from 'rxjs';
import { ManageProfileViewModel } from '../_models/ManageProfileViewModel';
import { ChangePasswordViewModel } from '../_models/ChangePasswordViewModel';
import { SetLocationViewModel } from 'src/_models/SetLocationViewModel';

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
    return this.http.post<ManageProfileViewModel>('api/Manage/UpdateProfile', viewModel, httpOptions)
    .pipe(
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postChangePassword(viewModel: ChangePasswordViewModel): Observable<ChangePasswordViewModel | ErrorReportViewModel> {
    return this.http.post<ChangePasswordViewModel>('api/Manage/ChangePassword', viewModel, httpOptions)
    .pipe(
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  postSetLocation(viewModel: SetLocationViewModel): Observable<SetLocationViewModel | ErrorReportViewModel> {
    return this.http.post<SetLocationViewModel>('api/Manage/ChangePassword', viewModel, httpOptions)
    .pipe(
      catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }
}
