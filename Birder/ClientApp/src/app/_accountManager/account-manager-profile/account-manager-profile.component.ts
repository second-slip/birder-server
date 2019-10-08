import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AccountManagerService } from '../../_services/account-manager.service';
import { ToastrService } from 'ngx-toastr';
import { ManageProfileViewModel } from '../../_models/ManageProfileViewModel';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { AccountService } from '../../_services/account.service';
import { ParentErrorStateMatcher } from '../../../validators';
import { first } from 'rxjs/operators';

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
    'userName': [
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

  constructor(private toast: ToastrService
            , private formBuilder: FormBuilder
            , private accountService: AccountService // validate username needs to be separate service...
            , private router: Router
            , private accountManager: AccountManagerService) { }

  ngOnInit() {
    this.getUserProfile();
  }

  createForms() {

    this.manageProfileForm = this.formBuilder.group({
      userName: new FormControl(this.user.userName, Validators.compose([
        // ValidateEmailNotTaken.createValidator(this.accountService),
        // this.validateEmailNotTaken.bind(this),
        // UsernameValidator.validUsername,
        Validators.maxLength(25),
        Validators.minLength(5),
        Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9]+$'), // ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$
        Validators.required
      ])),
      email: new FormControl(this.user.email, Validators.compose([
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
      ])),
    });
  }

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
    if (this.manageProfileForm.get('userName').value === this.user.userName) {
      // this.isUsernameAvailable = true;
      return;
    }
    if (this.manageProfileForm.get('userName').valid) {
      this.validateUsernameIsAvailable(this.manageProfileForm.get('userName').value);
    } else {
      // alert('do nothing');
    }
  }

  getUserProfile() {
    this.accountManager.getUserProfile()
      .subscribe(
        (data: ManageProfileViewModel) => {
          this.user = data;
          this.createForms();
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
            this.router.navigate(['/login'], { queryParams: { returnUrl: '/account-manager-profile' } });
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
