import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';

// 
import { Router } from '@angular/router';

import { ParentErrorStateMatcher, PasswordValidator } from '@app/_validators';

import { ChangePasswordViewModel } from '@app/_models/ChangePasswordViewModel';
import { AccountManagerService } from '../account-manager.service';


@Component({
  selector: 'app-account-manager-password',
  templateUrl: './account-manager-password.component.html',
  styleUrls: ['./account-manager-password.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountManagerPasswordComponent implements OnInit {
  unsuccessful: boolean;
  errorReport: any;
  changePasswordForm: FormGroup;
  matching_passwords_group: FormGroup;
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  requesting: boolean;

  changePassword_validation_messages = {
    'oldPassword': [
      { type: 'required', message: 'Your current password required' },
    ],
    'newPassword': [
      { type: 'required', message: 'Your new password is required' },
      { type: 'minlength', message: 'Password must be at least 8 characters long' },
      { type: 'pattern', message: 'Your password must contain at least one number and one letter' }
    ],
    'confirmPassword': [
      { type: 'required', message: 'You must confirm your new password' },
      { type: 'areEqual', message: 'Password mismatch' }
    ]
  };

  constructor(private router: Router
    , private accountManager: AccountManagerService
    , private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.createForms();
  }

  createForms() {
    this.matching_passwords_group = new FormGroup({
      newPassword: new FormControl('', Validators.compose([
        Validators.minLength(8),
        Validators.required, // regex: accept letters, numbers and !@#$%.  Must have at least one letter and number
        Validators.pattern('^(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9!@#$%]+$') // ^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$')
      ])),
      confirmPassword: new FormControl('', Validators.required)
    }, (formGroup: FormGroup) => {
      return PasswordValidator.areEqual(formGroup);
    });

    this.changePasswordForm = this.formBuilder.group({
      oldPassword: new FormControl('', Validators.compose([
        Validators.required
      ])),
      matching_passwords: this.matching_passwords_group
    });
  }

  onSubmit(value): void {
    this.requesting = true;

    const viewModelObject = <ChangePasswordViewModel>{
      oldPassword: value.oldPassword,
      newPassword: value.matching_passwords.newPassword,
      confirmPassword: value.matching_passwords.confirmPassword
    };

    this.accountManager.postChangePassword(viewModelObject)
      .subscribe(
        (data: ChangePasswordViewModel) => {
          this.unsuccessful = false;
          this.changePasswordForm.reset();
          // this.toast.success('Your changed your password', 'Success');
          this.router.navigate(['login']);
          // this.router.navigate(['/confirm-email']);
        },
        (error => {
          // if (error.status === 400) { }
          this.errorReport = error;
          this.unsuccessful = true;
          // this.toast.error('Your password could not be changed', 'Error');
        }),
        () => { this.requesting = false; }
      );
  }
}
