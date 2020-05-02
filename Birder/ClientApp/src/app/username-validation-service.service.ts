import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, } from 'rxjs/operators';
import { AsyncValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { HttpParams, HttpClient } from '@angular/common/http';
import { HttpErrorHandlerService } from './_services/http-error-handler.service';
import { TokenService } from './_services/token.service';

@Injectable({
  providedIn: 'root'
})
export class UsernameValidationService {

  constructor(private http: HttpClient, private token: TokenService) { }

  checkIfUsernameExists(username: string): Observable<boolean> {
    const options = username ?
      { params: new HttpParams().set('username', username.trim()) } : {};

    return this.http.get<boolean>('api/Account/IsUsernameAvailable', options)
      .pipe(
        // catchError(err => this.httpErrorHandlerService.handleHttpError(err))
      );
  }

  isOwnUsername(username: string): boolean {
    if (username === '') {
      return true;
    } else {
      return false;
    }
      //         if (this.manageProfileForm.get('username').value === this.user.userName) {
      // // this.isUsernameAvailable = true;
      // return null;
  }

  usernameValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (this.token.checkIsRecordOwner(control.value)) {
        return of(null);
      }
      return this.checkIfUsernameExists(control.value).pipe(
        map(isUsernameAvailable => {
          // if isUsernameAvailable is true, username is available, return null
          return isUsernameAvailable ? null : { usernameExists: true };
          // if isUsernameAvailable is false, username is taken, return error
        })
      );
    };
  }
}
  // takenUsernames = ['hello', 'world', 'username'];
  // checkIfUsernameExists(username: string): Observable<boolean> {
  //   // normally, this is where you will connect to your backend for validation lookup
  //   // using http, we simulate an internet connection by delaying it by a second
  //   return of(this.takenUsernames.includes(username)).pipe(delay(1000));
  // }