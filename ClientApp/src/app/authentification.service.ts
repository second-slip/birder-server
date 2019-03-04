import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { JwtHelper } from 'angular2-jwt';
import { map } from 'rxjs/operators';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { LoginViewModel } from './login/login.component';



const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class AuthentificationService {

  private isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  isLoggedIn$: Observable<boolean> = this.isLoggedIn.asObservable();
  private heroesUrl = 'api/Account/Login';

  constructor(private http: HttpClient) { }

  login(viewModel: LoginViewModel) {
    // TODO: remove console log
    console.log('loginservice - LOGIN');
    console.log(viewModel);

    return this.http.post<any>(this.heroesUrl, viewModel, httpOptions)
      .pipe(map(user => {
        // login successful if there's a jwt token in the response
        if (user && user.token) {
          // store user details and jwt token in local storage to keep user logged in between page refreshes
          localStorage.setItem('jwt', JSON.stringify(user));
          this.isLoggedIn.next(true);
          // this.currentUserSubject.next(user);
        }

        return user;
      }));
  }


  // ...
  logout() {

    console.log('loginservice - LOGOUT');

    localStorage.removeItem('jwt');

    // When Logout
    this.isLoggedIn.next(false);
  }

  /* checkLoginStatus(): boolean {
    var token = localStorage.getItem("jwt");

    (token && !this.jwtHelper.isTokenExpired(token)) ? this.isLoggedIn.next(true): this.isLoggedIn.next(false);
  } */
}

