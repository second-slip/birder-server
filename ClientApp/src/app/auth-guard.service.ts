import { Injectable } from '@angular/core';
import { CanActivate, Router, CanActivateChild } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthGuard implements CanActivate, CanActivateChild {
  constructor(private jwtHelper: JwtHelperService
            , private router: Router) { }

  canActivate() {

    // TODO: use authenticationService?
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      // console.log('valid token');
      return true;
    }
    // if (token && this.jwtHelper.isTokenExpired(token)) {
    //   // console.log(this.jwtHelper.decodeToken(token));
    //   console.log('expired token');
    //   this.router.navigate(['/login']);
    //   return false;
    // }
    // console.log('NO token');
    this.router.navigate(['/login']);
    return false;
  }

  canActivateChild() {

    // TODO: use authenticationService?
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      console.log('valid token');
      return true;
    }
    if (token && this.jwtHelper.isTokenExpired(token)) {
      // console.log(this.jwtHelper.decodeToken(token));
      console.log('expired token');
      this.router.navigate(['login']);
      return false;
    }
    console.log('NO token');
    this.router.navigate(['login']);
    return false;
  }
}
