import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { JwtHelper } from 'angular2-jwt';
import { map } from 'rxjs/operators';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { LoginViewModel } from '../_models/login-view-model';
import { Token } from '@angular/compiler';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class AuthenticationService {

  private isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  isLoggedIn$: Observable<boolean> = this.isLoggedIn.asObservable();

  constructor(private http: HttpClient
            , private jwtHelper: JwtHelper) { }

  login(viewModel: LoginViewModel): Observable<string> {
    // TODO: remove console log
    console.log('loginservice - LOGIN');

    return this.http.post<any>('api/Authentication/Login', viewModel, httpOptions)
      .pipe(map(user => {
        // login successful if there's a jwt token in the response
        if (user && user.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          // let token = (<any>user).token;
          localStorage.setItem('jwt', user.token);
          this.isLoggedIn.next(true);
        }
        // TODO: handle error: looed by global intercept
        return user;
      }));
  }

  getAuthorisationToken(): string {

    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      return token;
    } else {
      return '';
    }
  }

  logout(): void {
    // TODO: Remove console.log
    console.log('loginservice - LOGOUT');
    localStorage.removeItem('jwt');
    this.isLoggedIn.next(false);
  }


  checkLoginStatus(): boolean {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      // alert('user is logged in');
      // this.isLoggedIn = true;
      // this.loggedStatus.emit(true);
      this.isLoggedIn.next(true);
      return true;
    } else {
      // this.isLoggedIn = false;
      // alert('user is NOT logged in');
      // this.loggedStatus.emit(false);

      // TODO: Remove token (could be expired)
      this.isLoggedIn.next(false);
      return false;

    }
  }


}

