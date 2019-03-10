import { FormControl, AbstractControl } from '@angular/forms';
import { AccountService } from '../app/account.service';
import { first } from 'rxjs/operators';
import { ErrorReportViewModel } from '../_models/ErrorReportViewModel';
import { hasLifecycleHook } from '@angular/compiler/src/lifecycle_reflector';

export class UsernameValidator {

  constructor(private accountService: AccountService) {}

  static validUsername(fc: FormControl) {
    if (fc.value.toLowerCase() === 'abc123' || fc.value.toLowerCase() === '123abc') {
      return {
        validUsername: true
      };
    } else {
      return null;
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
}
