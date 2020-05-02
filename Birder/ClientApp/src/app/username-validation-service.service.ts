import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay, map, catchError } from 'rxjs/operators';
import {
  AsyncValidatorFn,
  AbstractControl,
  ValidationErrors
} from '@angular/forms';
import { HttpParams, HttpClient } from '@angular/common/http';
import { HttpErrorHandlerService } from './_services/http-error-handler.service';
import { ErrorReportViewModel } from './_models/ErrorReportViewModel';

@Injectable({
  providedIn: 'root'
})
export class UsernameValidationService {
  takenUsernames = ['hello', 'world', 'username'];

  constructor(private http: HttpClient
    , private httpErrorHandlerService: HttpErrorHandlerService) {}

  // checkIfUsernameExists(username: string): Observable<boolean> {
  //   // normally, this is where you will connect to your backend for validation lookup
  //   // using http, we simulate an internet connection by delaying it by a second
  //   return of(this.takenUsernames.includes(username)).pipe(delay(1000));
  // }

  checkIfUsernameExists(username: string): Observable<boolean> {
    username = username.trim();

    const options = username ?
    { params: new HttpParams().set('username', username) } : {};

    return this.http.get<boolean>('api/Account/IsUsernameAvailable', options)
    .pipe(
      // catchError(err => this.httpErrorHandlerService.handleHttpError(err))
    );
  }

  checkValidUsername(username: string): Observable<boolean | ErrorReportViewModel> {
    username = username.trim();

    const options = username ?
    { params: new HttpParams().set('username', username) } : {};

    return this.http.get<boolean>('api/Account/IsUsernameAvailable', options)
    .pipe(
      catchError(err => this.httpErrorHandlerService.handleHttpError(err))
    );
  }

  usernameValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return this.checkIfUsernameExists(control.value).pipe(
        map(res => {
          console.log(res);
          // if res is true, username exists, return true
          return res ? null : { usernameExists: true };
          // NB: Return null if there is no error
        })
      );
    };
  }
}