import { Injectable } from '@angular/core';
import { RegisterViewModel } from '../_models/RegisterViewModel';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { first } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { UserEmailDto } from '../_models/UserEmailDto';
import { ResetPasswordViewModel } from '../_models/ResetPasswordViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  register(viewModel: RegisterViewModel): Observable<RegisterViewModel> {
    return this.http.post<RegisterViewModel>('api/Account/Register', viewModel, httpOptions)
    .pipe(first());
  }

  forgotPassword(viewModel: UserEmailDto): Observable<UserEmailDto> {
    return this.http.post<UserEmailDto>('api/Account/ForgotPassword', viewModel, httpOptions)
    .pipe(first());
  }

  resendEmailConfirmation(viewModel: UserEmailDto): Observable<UserEmailDto> {
    return this.http.post<UserEmailDto>('api/Account/ResendEmailConfirmation', viewModel, httpOptions)
    .pipe(first()
    );
  }

  resetPassword(viewModel: ResetPasswordViewModel): Observable<ResetPasswordViewModel> {
    return this.http.post<ResetPasswordViewModel>('api/Account/ResetPassword', viewModel, httpOptions)
    .pipe(first());
  }

  checkValidUsername(username: string): Observable<boolean> {
    username = username.trim();

    const options = username ?
    { params: new HttpParams().set('username', username) } : {};

    return this.http.get<boolean>('api/Account/IsUsernameAvailable', options)
    .pipe(first());
  }
}
