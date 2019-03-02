import { Injectable } from '@angular/core';
import { JwtHelper } from 'angular2-jwt';

@Injectable({
  providedIn: 'root'
})
// export class LoginService {

//   constructor() { }
// }

export class LoginService {
  // isLoggedIn: boolean;

  constructor(private jwtHelper: JwtHelper) { }

  // checkLoginStatus(): Observable<boolean> {
   checkLoginStatus(): boolean {
    var token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)){
      // console.log(this.jwtHelper.decodeToken(token));
      alert('user is logged in');
      // this.isLoggedIn = true;

      return true;
    } else {
      // this.isLoggedIn = false;
      alert('user is NOT logged in');
      return false;
    }
  }
}
