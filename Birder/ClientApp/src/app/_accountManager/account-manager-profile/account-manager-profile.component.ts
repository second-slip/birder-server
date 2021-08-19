import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { ManageProfileViewModel } from '@app/_models/ManageProfileViewModel';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// 
import { Router } from '@angular/router';
import { UsernameValidationService } from '@app/_services/username-validation-service.service';
import { ParentErrorStateMatcher, RestrictedNameValidator } from '@app/_validators';
import { AccountManagerService } from '../account-manager.service';


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
  errorReport: any;
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  requesting: boolean;
  emailChanged = false;
  updating: boolean;


  manageProfile_validation_messages = {
    'username': [
      { type: 'required', message: 'Username is required' },
      { type: 'minlength', message: 'Username must be at least 5 characters long' },
      { type: 'maxlength', message: 'Username cannot be more than 25 characters long' },
      { type: 'pattern', message: 'Your username must be alphanumeric (no special characters) and must not contain spaces' },
      { type: 'restrictedName', message: 'Username may not contain the name "birder"' },
      { type: 'usernameExists', message: 'Username is not available.  Please type another one...' },
      { type: 'serverError', message: 'Unable to connect to the server.  Please try again.' }
    ],
    'email': [
      { type: 'required', message: 'Email is required' },
      { type: 'pattern', message: 'Enter a valid email' }
    ]
  };

  constructor(private usernameService: UsernameValidationService
    , private formBuilder: FormBuilder
    , private router: Router
    , private accountManager: AccountManagerService) { }

  ngOnInit() {
    this.getUserProfile();
  }

  createForm(): FormGroup {
    return this.formBuilder.group({
      username: [
        this.user.userName,
        {
          validators: [
            Validators.required,
            Validators.minLength(5),
            Validators.maxLength(25),
            Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9]+$'), // ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$
            RestrictedNameValidator(/birder/i)],
          asyncValidators: [this.usernameService.usernameValidator()],
          updateOn: 'blur'
        }
      ],
      email: [
        // this updates on blur
        this.user.email,
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


  getUserProfile() {
    this.requesting = true;
    this.accountManager.getUserProfile()
      .subscribe(
        (data: ManageProfileViewModel) => {
          this.user = data;
          this.manageProfileForm = this.createForm();
        },
        (error => {
          // this.toast.error(error.friendlyMessage, 'An error occurred');
          this.router.navigate(['/login'], { queryParams: { returnUrl: '/account-manager-profile' } });
        }),
        () => { this.requesting = false; }
      );
  }

  onSubmit(value): void {
    this.updating = true;

    const model = <ManageProfileViewModel>{
      userName: value.username,
      email: value.email,
    };

    if (model.email !== this.user.email) {
      this.emailChanged = true;
    }

    this.accountManager.postUpdateProfile(model)
      .subscribe(
        (data: ManageProfileViewModel) => {
          this.user = data;  // why?
          // this.toast.success('Please re-login', 'Your profile was changed');
          if (this.emailChanged === true) {
            this.router.navigate(['/confirm-email']);
          } else {
            this.router.navigate(['/login'], { queryParams: { returnUrl: '/user-profile' } });
          }
        },
        (error => {
          // if (error.status === 400) { }
          this.errorReport = error;
          // this.invalidRegistration = true;
          // this.toast.error(error.friendlyMessage, 'Error');
        }),
        () => { this.updating = false; }
      );
  }
}
