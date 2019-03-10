import { AbstractControl } from '@angular/forms';
import { AccountService } from '../app/account.service';

export class ValidateEmailNotTaken {
  static createValidator(signupService: AccountService) {
    return (control: AbstractControl) => {
      return signupService.checkValidUsername(control.value)
      .subscribe(
        (data) => {
          //this.isValid = true;
          console.log('valid');
        },
        (error) => {
          //this.isValid = false;
          console.log('invalid');
        }
      );
    };
  }
}