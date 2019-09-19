import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { tap, catchError, take } from 'rxjs/operators';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { LoginViewModel } from '../_models/LoginViewModel';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class AuthenticationService {

  private isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  isAuthenticated$: Observable<boolean> = this.isAuthenticated.asObservable();

  constructor(private http: HttpClient
    , private jwtHelper: JwtHelperService
    , private httpErrorHandlerService: HttpErrorHandlerService) { }

  login(viewModel: LoginViewModel): Observable<any | ErrorReportViewModel> {
    return this.http.post<any>('api/Authentication/login', viewModel, httpOptions)
      .pipe(
        take(1),
        tap(response => this.setAuthenticationToken(response)),
        catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
  }

  setAuthenticationToken(token: any): void {
    // let token = (<any>user).token;
    if (token && token.token) {
      localStorage.setItem('jwt', token.token);
      this.isAuthenticated.next(true);
    }
  }

  getAuthenticationToken(): string {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      return token;
    } else {
      return '';
    }
  }

  logout(): void {
    localStorage.removeItem('jwt');
    this.isAuthenticated.next(false);
  }

  checkIsAuthenticatedObservable(): Observable<boolean> {

    return new Observable<boolean>(isAuthenticated => {
      const token = localStorage.getItem('jwt');
      if (token && !this.jwtHelper.isTokenExpired(token)) {
        return isAuthenticated.next(true);

      } else {
        return isAuthenticated.next(false);
      }
    });
  }

  checkIsAuthenticated(): boolean {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      this.isAuthenticated.next(true);
      return true;
    } else {
      // TODO: Remove token (could be expired)
      this.isAuthenticated.next(false);
      return false;
    }
  }


}
// export interface TokenModel {
//   authToken: string;
// }
