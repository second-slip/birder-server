import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay, map } from 'rxjs/operators';
import { AsyncValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-testing2',
  templateUrl: './testing2.component.html',
  styleUrls: ['./testing2.component.scss']
})
export class Testing2Component implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  takenUsernames = [
    'hello',
    'world',
    'username'
    // ...
  ];



  checkIfUsernameExists(username: string): Observable<boolean> {
    return of(this.takenUsernames.includes(username)).pipe(delay(1000));
  }

  usernameValidator(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return this.checkIfUsernameExists(control.value).pipe(
        map(res => {
          // if res is true, username exists, return true
          return res ? { usernameExists: true } : { usernameExists: true };
          // NB: Return null if there is no error
        })
      );
    };
  }

}
