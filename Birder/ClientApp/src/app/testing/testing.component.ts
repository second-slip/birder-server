import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, Validators, FormGroup } from '@angular/forms';
import { AccountService } from '@app/_services/account.service';
import { AccountManagerService } from '@app/_services/account-manager.service';
import { ManageProfileViewModel } from '@app/_models/ManageProfileViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

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

  createForms() {

    this.manageProfileForm = this.formBuilder.group({
      username: new FormControl(this.user.userName, Validators.compose([
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

}
