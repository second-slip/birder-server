import { Directive, forwardRef, Injectable } from '@angular/core';
import {
  AsyncValidator,
  AbstractControl,
  NG_ASYNC_VALIDATORS,
  ValidationErrors,
  FormControl
} from '@angular/forms';
import { catchError, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AccountService } from '../app/account.service';

@Injectable({ providedIn: 'root' })
export class UniqueAlterEgoValidator implements AsyncValidator {
  constructor(private accountService: AccountService) {}

  validate(
    // ctrl: AbstractControl
    fc: FormControl
  ): Promise<any | null> | Observable<any | null> {
    return this.accountService.checkValidUsername(fc.value)
    .pipe(
      map(isTaken => (isTaken ? { validUsername: true } : null)),
      catchError(() => null)
    );
  }
}

@Directive({
  selector: '[appUniqueAlterEgo]',
  providers: [
    {
      provide: NG_ASYNC_VALIDATORS,
      useExisting: forwardRef(() => UniqueAlterEgoValidator),
      multi: true
    }
  ]
})
export class UniqueAlterEgoValidatorDirective {
  constructor(private validator: UniqueAlterEgoValidator) {}

  validate(fc: FormControl) {
    this.validator.validate(fc);
  }
}
