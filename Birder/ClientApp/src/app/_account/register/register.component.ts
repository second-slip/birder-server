import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router } from '@angular/router';
import { UsernameValidationService } from '@app/_services/username-validation-service.service';
import { RegisterViewModel } from '@app/_models/RegisterViewModel';
import { AccountService } from '@app/_account/account.service';
import { ParentErrorStateMatcher, PasswordValidator, RestrictedNameValidator } from '@app/_validators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RegisterComponent implements OnInit {
  requesting: boolean;
  invalidRegistration: boolean;
  errorReport: any;
  userRegisterForm: FormGroup;
  matching_passwords_group: FormGroup;
  parentErrorStateMatcher = new ParentErrorStateMatcher();

  userRegister_validation_messages = {
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
    ],
    'confirmPassword': [
      { type: 'required', message: 'Confirm password is required' },
      { type: 'areEqual', message: 'Password mismatch' }
    ],
    'password': [
      { type: 'required', message: 'Password is required' },
      { type: 'minlength', message: 'Password must be at least 8 characters long' },
      { type: 'pattern', message: 'Your password must contain at least one number and one letter' }
      // { type: 'pattern', message: 'Your password must contain at least one uppercase, one lowercase, and one number' }
    ]
  };

  constructor(private formBuilder: FormBuilder
    , private usernameService: UsernameValidationService
    , private accountService: AccountService
    , private router: Router) { }

  ngOnInit() {
    this.userRegisterForm = this.createForm();  //this.createForms();
  }

  createForm(): FormGroup {
    // matching passwords validation
    this.matching_passwords_group = new FormGroup({
      password: new FormControl('', Validators.compose([
        Validators.minLength(8),
        Validators.required, // regex: accept letters, numbers and !@#$%.  Must have at least one letter and number
        Validators.pattern('^(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9!@#$%]+$') // ^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$')
      ])),
      confirmPassword: new FormControl('', Validators.required)
    }, (formGroup: FormGroup) => {
      return PasswordValidator.areEqual(formGroup);
    });

    return this.userRegisterForm = this.formBuilder.group({
      username: [
        '',
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
        '',
        {
          validators: [Validators.required,
          Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')],
          // updateOn: 'blur'
        }
      ],
      matching_passwords: this.matching_passwords_group
    });
  }
  // createForms() {
  //   // matching passwords validation
  //   this.matching_passwords_group = new FormGroup({
  //     password: new FormControl('', Validators.compose([
  //       Validators.minLength(8),
  //       Validators.required, // regex: accept letters, numbers and !@#$%.  Must have at least one letter and number
  //       Validators.pattern('^(?=.*[a-z])(?=.*[0-9])[a-zA-Z0-9!@#$%]+$') // ^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])[a-zA-Z0-9]+$')
  //     ])),
  //     confirmPassword: new FormControl('', Validators.required)
  //   }, (formGroup: FormGroup) => {
  //     return PasswordValidator.areEqual(formGroup);
  //   });

  //   this.userRegisterForm = this.formBuilder.group({
  //     userName: new FormControl('', Validators.compose([
  //      Validators.maxLength(25),
  //      Validators.minLength(5),
  //      Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9]+$'), // ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$
  //      Validators.required,
  //      RestrictedNameValidator(/birder/i)
  //     ])),
  //     email: new FormControl('', Validators.compose([
  //       Validators.required,
  //       Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
  //     ])),
  //     matching_passwords: this.matching_passwords_group
  //     // terms: new FormControl(false, Validators.pattern('true'))
  //   });
  // }

  onSubmit(value): void {

    // if (this.isUsernameAvailable === false) {
    //   const unavailableUsername = this.userRegisterForm.get('userName').value;
    //   alert(`The username '${unavailableUsername}' is already taken.  Please choose a different username.`);
    //   return;
    // }

    this.requesting = true;

    const viewModelObject = <RegisterViewModel>{
      userName: value.username,
      email: value.email,
      password: value.matching_passwords.password,
      confirmPassword: value.matching_passwords.confirmPassword
    };

    this.accountService.register(viewModelObject)
      .subscribe(_ => {
        this.router.navigate(['/confirm-email']);
      },
        (error: any) => {
          this.requesting = false;
          this.errorReport = error;
          this.invalidRegistration = true;
        });
  }
}
