import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators, FormGroup, AbstractControl, ValidatorFn } from '@angular/forms';
import { AccountService } from '@app/_services/account.service';
import { AccountManagerService } from '@app/_services/account-manager.service';
import { ManageProfileViewModel } from '@app/_models/ManageProfileViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-testing',
  templateUrl: './testing.component.html',
  styleUrls: ['./testing.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TestingComponent implements OnInit {
  user: ManageProfileViewModel;
  invalidChange: boolean; //
  manageProfileForm: FormGroup;
  errorReport: ErrorReportViewModel;
  // parentErrorStateMatcher = new ParentErrorStateMatcher();
  isUsernameAvailable = true;
  emailChanged = false;

  manageProfile_validation_messages = {
    'username': [
      { type: 'required', message: 'Username is required' },
      { type: 'minlength', message: 'Username must be at least 5 characters long' },
      { type: 'maxlength', message: 'Username cannot be more than 25 characters long' },
      { type: 'pattern', message: 'Your username must be alphanumeric (no special characters) and must not contain spaces' },
    ],
    'email': [
      { type: 'required', message: 'Email is required' },
      { type: 'pattern', message: 'Enter a valid email' }
    ]
  };

  constructor(
    private formBuilder: FormBuilder
    , private accountService: AccountService // validate username needs to be separate service...
    , private router: Router
    , private accountManager: AccountManagerService) { }

  ngOnInit() {
    this.getUserProfile();
  }

  get name() { return this.manageProfileForm.get('username'); }

  get email() { return this.manageProfileForm.get('email'); }

  createForms() {

    this.manageProfileForm = this.formBuilder.group({
      username: new FormControl(this.user.userName, Validators.compose([
        // ValidateEmailNotTaken.createValidator(this.accountService),
        // this.validateEmailNotTaken.bind(this),
        // UsernameValidator.validUsername,
        // forbiddenNameValidator(/bob/),
        // forbiddenNameValidator1(/bob/).bind(this),
        Validators.maxLength(25),
        Validators.minLength(5),
        Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9]+$'), // ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$
        Validators.required,
        this.validateUsername.bind(this)
        // forbiddenNameValidator1.bind(this)
        
      ])),
      email: new FormControl(this.user.email, Validators.compose([
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
      ])),
    });
  }

  getUserProfile() {
    this.accountManager.getUserProfile()
      .subscribe(
        (data: ManageProfileViewModel) => {
          this.user = data;
          this.createForms();
        },
        (error: ErrorReportViewModel) => {
          // this.toast.error(error.friendlyMessage, 'An error occurred');
          this.router.navigate(['/login'], { queryParams: { returnUrl: '/account-manager-profile' } });
        });
  }

  private validateUsername(control: AbstractControl) {
    const val = control.value;
    return this.accountService.checkValidUsername(val) // httpService.validateUsername(val)
    .subscribe(
      (isAvailable: boolean) => {
      console.log(isAvailable);
      return isAvailable ? {'username': {value: control.value}} : null;
        // const convertedName = res['user_name'];
        // return convertedName === val ? { alreadyExist: true } : null;
      }),
      catchError(err => {
        console.log(err.error);
        console.log(err.message);
        // this.messagesService.openDialog('Error aef-2', 'Database not available.');
        return of({ "error": true })
      })
    // );
  }

  private forbiddenNameValidator1(): ValidatorFn {
    return (control: AbstractControl): {[key: string]: any} | null => {
      // const forbidden = nameRe.test(control.value);
      console.log(control.value)
      return this.accountService.checkValidUsername(control.value) // httpService.validateUsername(val)
      .subscribe((isAvailable: boolean) => {
          // const convertedName = res['user_name'];
          // return convertedName === val ? { alreadyExist: true } : null;
          console.log(isAvailable);
          return isAvailable ? {'username': {value: control.value}} : null;
              // return forbidden ? {'forbiddenName': {value: control.value}} : null;
      // returns either null if the control value is valid or a validation error object
        })
        // catchError(err => {
        //   console.log(err.error);
        //   console.log(err.message);
        //   // this.messagesService.openDialog('Error aef-2', 'Database not available.');
        //   return of({ 'username': true })
        // });
      // );
      // return forbidden ? {'forbiddenName': {value: control.value}} : null;
      // returns either null if the control value is valid or a validation error object
    };
  }

  /** A hero's name can't match the given regular expression */


}

export function forbiddenNameValidator1(): ValidatorFn {
  return (control: AbstractControl): {[key: string]: any} | null => {
    // const forbidden = nameRe.test(control.value);
    console.log(control.value)
    return this.accountService.checkValidUsername(control.value) // httpService.validateUsername(val)
    .subscribe((isAvailable: boolean) => {
        // const convertedName = res['user_name'];
        // return convertedName === val ? { alreadyExist: true } : null;
        console.log(isAvailable);
        return isAvailable ? {'username': {value: control.value}} : null;
            // return forbidden ? {'forbiddenName': {value: control.value}} : null;
    // returns either null if the control value is valid or a validation error object
      })
      // catchError(err => {
      //   console.log(err.error);
      //   console.log(err.message);
      //   // this.messagesService.openDialog('Error aef-2', 'Database not available.');
      //   return of({ 'username': true })
      // });
    // );
    // return forbidden ? {'forbiddenName': {value: control.value}} : null;
    // returns either null if the control value is valid or a validation error object
  };
}


export function forbiddenNameValidator(nameRe: RegExp): ValidatorFn {
  return (control: AbstractControl): {[key: string]: any} | null => {
    const forbidden = nameRe.test(control.value);
    return forbidden ? {'forbiddenName': {value: control.value}} : null;
  };
}