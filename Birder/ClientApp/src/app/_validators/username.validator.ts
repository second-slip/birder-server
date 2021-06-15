import { FormControl } from '@angular/forms';
import { Injectable } from '@angular/core';
import { AccountService } from '@app/_account/account.service';

@Injectable()
export class UsernameValidator {
  debouncer: any;

  constructor(public accountService: AccountService) { }

  checkUsername(control: FormControl): any {

    clearTimeout(this.debouncer);

    return new Promise(resolve => {
      this.debouncer = setTimeout(() => {
        this.accountService.checkValidUsername(control.value).subscribe((res) => {
          if (res) {
            resolve(null);
          }
        }, (err) => {
          resolve({ 'usernameInUse': true });
        });
      }, 1000);
    });
  }
}

export class UsernameValidator1 {
  constructor() { }

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
  //     (error: any) => {
  //       // if (error.status === 400) { }
  //       console.log(error.modelStateErrors);
  //       return null;
  //     });
  //   return UsernameValidator.validUsername(fc);
  // }

