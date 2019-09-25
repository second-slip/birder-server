import { Injectable } from '@angular/core';
import { RegisterViewModel } from '../_models/RegisterViewModel';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { UserEmailDto } from '../_models/UserEmailDto';
import { ResetPasswordViewModel } from '../_models/ResetPasswordViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient
            , private httpErrorHandlerService: HttpErrorHandlerService) { }

  register(viewModel: RegisterViewModel): Observable<void | ErrorReportViewModel> {
    return this.http.post<void>('api/Account/Register', viewModel, httpOptions)
    .pipe(
      catchError(err => this.httpErrorHandlerService.handleHttpError(err))
    );
  }

  forgotPassword(viewModel: UserEmailDto): Observable<void | ErrorReportViewModel> {
    return this.http.post<void>('api/Account/ForgotPassword', viewModel, httpOptions)
    .pipe(
      catchError(err => this.httpErrorHandlerService.handleHttpError(err))
    );
  }

  resendEmailConfirmation(viewModel: UserEmailDto): Observable<void | ErrorReportViewModel> {
    return this.http.post<void>('api/Account/ResendEmailConfirmation', viewModel, httpOptions)
    .pipe(
      catchError(err => this.httpErrorHandlerService.handleHttpError(err))
    );
  }

  resetPassword(viewModel: ResetPasswordViewModel): Observable<void | ErrorReportViewModel> {
    return this.http.post<void>('api/Account/ResetPassword', viewModel, httpOptions)
    .pipe(
      catchError(err => this.httpErrorHandlerService.handleHttpError(err))
    );
  }

  checkValidUsername(userName: string): Observable<boolean | ErrorReportViewModel> {
    userName = userName.trim();

    const options = userName ?
    { params: new HttpParams().set('userName', userName) } : {};

    return this.http.get<boolean>('api/Account/IsUsernameAvailable', options)
    .pipe(
      catchError(err => this.httpErrorHandlerService.handleHttpError(err))
    );
  }
}
