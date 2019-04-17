import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AccountManagerService } from '../account-manager.service';
import { ToastrService } from 'ngx-toastr';
import { ManageProfileViewModel } from '../../_models/ManageProfileViewModel';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { AccountService } from '../account.service';
import { ParentErrorStateMatcher } from '../../validators';

@Component({
  selector: 'app-account-manage',
  templateUrl: './account-manage.component.html',
  styleUrls: ['./account-manage.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AccountManageComponent implements OnInit {
  user: ManageProfileViewModel;
  invalidRegistration: boolean; //
  manageProfileForm: FormGroup;
  errorReport: ErrorReportViewModel;
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  isUsernameAvailable: boolean = true;

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
    // matching passwords validation
    // this.matching_passwords_group = new FormGroup({
    //   password: new FormControl('', Validators.compose([
    //     Validators.minLength(8),
    //     Validators.required, // regex: accept letters, numbers and !@#$%.  Must have at least one letter and number
    //     Validators.pattern('^(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9!@#$%]+$') // ^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$')
    //   ])),
    //   confirmPassword: new FormControl('', Validators.required)
    // }, (formGroup: FormGroup) => {
    //   return PasswordValidator.areEqual(formGroup);
    // });

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
      // matching_passwords: this.matching_passwords_group
      // terms: new FormControl(false, Validators.pattern('true'))
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
        // console.log(error);
        this.toast.error(error.serverCustomMessage, 'An error occurred');
        // this.router.navigate(['/']);
      });
  }
}
