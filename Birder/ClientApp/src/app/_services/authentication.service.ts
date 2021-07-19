import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { tap, first } from 'rxjs/operators';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { AuthenticationResultDto } from '@app/_models/AuthenticationResultDto';
import { LoginViewModel } from '@app/_models/LoginViewModel';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class AuthenticationService {
  private isAuthenticated: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  isAuthenticated$: Observable<boolean> = this.isAuthenticated.asObservable();

  constructor(private readonly _http: HttpClient
    , private jwtHelper: JwtHelperService) { }

  login(viewModel: LoginViewModel): Observable<AuthenticationResultDto> {
    return this._http.post<any>('api/Authentication/login', viewModel, httpOptions)
      .pipe(
        tap(response => this.setAuthenticationToken(response)));
  }

  setAuthenticationToken(token: AuthenticationResultDto): void {
    if (token && token.authenticationToken) {
      localStorage.setItem('jwt', token.authenticationToken);
      this.isAuthenticated.next(true);
    }
  }

  getAuthenticationToken(): string {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      return token;
    } else {
      return '';
    }
  }

  logout(): void {
    localStorage.removeItem('jwt');
    this.isAuthenticated.next(false);
  }

  checkIsAuthenticated(): boolean {
    const token = localStorage.getItem('jwt');

    if (token && !this.jwtHelper.isTokenExpired(token)) {
      this.isAuthenticated.next(true);
      return true;
    } else {
      localStorage.removeItem('jwt');
      this.isAuthenticated.next(false);
      return false;
    }
  }
}
