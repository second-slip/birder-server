import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ManageProfileViewModel } from '@app/_models/ManageProfileViewModel';
import { FormGroup, FormBuilder, FormControl, Validators, AbstractControl, ValidatorFn, ValidationErrors, AsyncValidatorFn } from '@angular/forms';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';
import { ParentErrorStateMatcher } from 'validators';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '@app/_services/account.service';
import { Router } from '@angular/router';
import { AccountManagerService } from '@app/_services/account-manager.service';
import { first, delay, map, debounceTime } from 'rxjs/operators';
import { RestrictedNameValidator } from 'validators/RestrictedNameValidator';
import { HttpClient } from '@angular/common/http';
import { pipe, Observable, of } from 'rxjs';
import { UsernameValidationService } from '@app/username-validation-service.service';


@Component({
  selector: 'app-account-manager-profile',
  templateUrl: './account-manager-profile.component.html',
  styleUrls: ['./account-manager-profile.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountManagerProfileComponent implements OnInit {
  user: ManageProfileViewModel;
  invalidChange: boolean; //
  manageProfileForm: FormGroup;
  errorReport: ErrorReportViewModel;
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  isUsernameAvailable = true;
  emailChanged = false;


  manageProfile_validation_messages = {
    'username': [
      { type: 'required', message: 'Username is required' },
      { type: 'minlength', message: 'Username must be at least 5 characters long' },
      { type: 'maxlength', message: 'Username cannot be more than 25 characters long' },
      { type: 'pattern', message: 'Your username must be alphanumeric (no special characters) and must not contain spaces' },
      { type: 'restrictedName', message: 'Username may not contain the name "birder"' },
      { type: 'usernameExists', message: 'XXXXXXXXXXXXX' }
    ],
    'email': [
      { type: 'required', message: 'Email is required' },
      { type: 'pattern', message: 'Enter a valid email' }
    ]
  };

  constructor(private toast: ToastrService
    , private usernameService: UsernameValidationService
    , private formBuilder: FormBuilder
    , private accountService: AccountService // validate username needs to be separate service...
    , private router: Router
    , private accountManager: AccountManagerService) { }

  ngOnInit() {
    this.getUserProfile();
  }

  createForm(): FormGroup {
    return this.formBuilder.group({
      username: [
        'chicken', {
          validators: [Validators.maxLength(25),
          Validators.minLength(5),
          Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9]+$'), // ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$
          Validators.required,
          RestrictedNameValidator(/birder/i)],
          asyncValidators: [this.usernameService.usernameValidator()],
          updateOn: 'blur'
        }
      ],
      email: [
        // this updates on blur
        'a@b.com',
        {
          validators: [Validators.required,
          Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')],
          updateOn: 'blur'
        }
      ]
    });
  }


  // createForms() {

  //   this.manageProfileForm = this.formBuilder.group({
  //     userName: new FormControl(this.user.userName, Validators.compose([
  //       Validators.maxLength(25),
  //       Validators.minLength(5),
  //       Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9]+$'), // ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$
  //       Validators.required,
  //       RestrictedNameValidator(/birder/i),
  //       this.usernameService.usernameValidator()
  //     ])),
  //     email: new FormControl(this.user.email, Validators.compose([
  //       Validators.required,
  //       Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
  //     ])),
  //   });
  // }


  validateUsernameIsAvailable(username: string) {
    return this.accountService.checkValidUsername(username)
      .subscribe(
        (data: boolean) => {
          this.isUsernameAvailable = data;
        },
        (error: ErrorReportViewModel) => {
          this.isUsernameAvailable = false;
        }
      );
  }

  checkUsernameIsAvailable(): void {
    if (this.manageProfileForm.get('username').value === this.user.userName) {
      // this.isUsernameAvailable = true;
      return;
    }
    if (this.manageProfileForm.get('username').valid) {
      this.validateUsernameIsAvailable(this.manageProfileForm.get('username').value);
    } else {
      // alert('do nothing');
    }
  }

  getUserProfile() {
    this.accountManager.getUserProfile()
      .subscribe(
        (data: ManageProfileViewModel) => {
          this.user = data;
          this.manageProfileForm = this.createForm();
          // this.createForm();
        },
        (error: ErrorReportViewModel) => {
          this.toast.error(error.friendlyMessage, 'An error occurred');
          this.router.navigate(['/login'], { queryParams: { returnUrl: '/account-manager-profile' } });
        });
  }

  onSubmit(value): void {

    if (this.isUsernameAvailable === false) {
      const unavailableUsername = this.manageProfileForm.get('userName').value;
      this.toast.error(`The username '${unavailableUsername}' is already taken.  Please choose a different username.`, 'Error');
      return;
    }

    const model = <ManageProfileViewModel>{
      userName: value.userName,
      email: value.email,
    };

    if (model.email !== this.user.email) {
      this.emailChanged = true;
    }

    this.accountManager.postUpdateProfile(model)
      .pipe(first())
      .subscribe(
        (data: ManageProfileViewModel) => {
          this.user = data;
          this.toast.success('Please re-login', 'Your profile was changed');
          if (this.emailChanged === true) {
            this.router.navigate(['/confirm-email']);
          } else {
            this.router.navigate(['/login'], { queryParams: { returnUrl: '/user-profile' } });
          }
        },
        (error: ErrorReportViewModel) => {
          // if (error.status === 400) { }
          this.errorReport = error;
          // this.invalidRegistration = true;
          this.toast.error(error.friendlyMessage, 'Error');
        });
  }
}
