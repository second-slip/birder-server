import { FormControl, AbstractControl } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { first } from 'rxjs/operators';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { hasLifecycleHook } from '@angular/compiler/src/lifecycle_reflector';

import { Injectable } from '@angular/core';

// import { AuthProvider } from '../providers/auth/auth';

@Injectable()
export class UsernameValidator {

  debouncer: any;

  constructor(public accountService: AccountService){

  }

  checkUsername(control: FormControl): any {

    clearTimeout(this.debouncer);

    return new Promise(resolve => {

      this.debouncer = setTimeout(() => {

        // this.authProvider.validateUsername(control.value).subscribe((res) => {
          this.accountService.checkValidUsername(control.value).subscribe((res) => {
            if(res){
              resolve(null);
            }
          }, (err) => {
            resolve({'usernameInUse': true});
          });

      }, 1000);      

    });
  }

}

export class UsernameValidator1 {

  constructor(private accountService: AccountService) { }

  static validUsername(fc: FormControl) {
    if (fc.value.toLowerCase() === 'abc123' || fc.value.toLowerCase() === '123abc') {
      return {
        validUsername: true
      };
    } else {
      return null;
    }
  }
}


  // hello(fc: FormControl) {
  //       this.accountService.checkValidUsername(fc.value)
  //   .pipe(first())
  //   .subscribe(
  //      (data: void) => {
  //        // console.log('successful registration');
  //        // this.router.navigate(['/login']);
  //        console.log('hello');
  //        return {
  //         validUsername: true
  //       };
  //      },
  //     (error: ErrorReportViewModel) => {
  //       // if (error.status === 400) { }
  //       console.log(error.modelStateErrors);
  //       return null;
  //     });
  //   return UsernameValidator.validUsername(fc);
  // }

