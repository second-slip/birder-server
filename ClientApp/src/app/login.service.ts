import { BehaviorSubject, Observable } from 'rxjs';
import { JwtHelper } from 'angular2-jwt';
import { HttpClient } from 'selenium-webdriver/http';

export class LoginService {

  private isLoggedIn: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  isLoggedIn$: Observable<boolean> = this.isLoggedIn.asObservable();

  constructor(private jwtHelper: JwtHelper
            , private http: HttpClient) { }



   // ...

  login() {



    // When Login
    this.isLoggedIn.next(true);
  }

  // ...
  logout() {

    localStorage.removeItem('jwt');

    // When Logout
    this.isLoggedIn.next(false);
  }

  /* checkLoginStatus(): boolean {
    var token = localStorage.getItem("jwt");

    (token && !this.jwtHelper.isTokenExpired(token)) ? this.isLoggedIn.next(true): this.isLoggedIn.next(false);
  } */
}
