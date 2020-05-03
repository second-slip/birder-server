import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { map, catchError, } from 'rxjs/operators';
import { AsyncValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { HttpParams, HttpClient } from '@angular/common/http';
import { HttpErrorHandlerService } from './_services/http-error-handler.service';
import { TokenService } from './_services/token.service';

@Injectable({
  providedIn: 'root'
})
export class UsernameValidationService {

  constructor(private http: HttpClient, private token: TokenService) { }

  checkIfUsernameExists(username: string): Observable<boolean | any> {
    const options = username ?
      { params: new HttpParams().set('username', username) } : {};

    return this.http.get<boolean>('api/Account/IsUsernameAvailable', options)
      .pipe(
        // catchError(error => this.httpErrorHandlerService.handleHttpError(error)));
        catchError(err => { return of(err); }));   
  }

  usernameValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      if (this.token.checkIsRecordOwner(control.value)) {
        return of(null);
      }
      return this.checkIfUsernameExists(control.value).pipe(
        map(isUsernameAvailable => {
          console.log(isUsernameAvailable['status']);
          // if isUsernameAvailable is true, username is available, return null
          // if isUsernameAvailable is false, username is taken, return error
          if (isUsernameAvailable['status'] != 'ok') { return { serverError: true }; } else {
            return isUsernameAvailable[status] == 'ok' ? null : { usernameExists: true };
          }
        }));
        // (error: any) => {
        //   console.log('hello');
        //   alert();
        //   return  { usernameError: true };
        // })
        // );
    }
  }
}
  // takenUsernames = ['hello', 'world', 'username'];
  // checkIfUsernameExists(username: string): Observable<boolean> {
  //   // normally, this is where you will connect to your backend for validation lookup
  //   // using http, we simulate an internet connection by delaying it by a second
  //   return of(this.takenUsernames.includes(username)).pipe(delay(1000));
  // }

  // old implementation from component:

    // validateUsernameIsAvailable(username: string) {
  //   return this.accountService.checkValidUsername(username)
  //     .subscribe(
  //       (data: boolean) => {
  //         this.isUsernameAvailable = data;
  //       },
  //       (error: ErrorReportViewModel) => {
  //         this.isUsernameAvailable = false;
  //       }
  //     );
  // }

  // checkUsernameIsAvailable(): void {
  //   if (this.manageProfileForm.get('username').value === this.user.userName) {
  //     // this.isUsernameAvailable = true;
  //     return;
  //   }
  //   if (this.manageProfileForm.get('username').valid) {
  //     this.validateUsernameIsAvailable(this.manageProfileForm.get('username').value);
  //   } else {
  //     // alert('do nothing');
  //   }
  // }