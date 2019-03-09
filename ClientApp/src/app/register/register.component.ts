import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { UsernameValidator, PasswordValidator, ParentErrorStateMatcher } from '../../validators';
import { AccountService } from '../account.service';
import { first } from 'rxjs/operators';
import { Router } from '@angular/router';
import { RegisterViewModel } from '../../_models/RegisterViewModel';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class RegisterComponent implements OnInit {

  userRegisterForm: FormGroup;

  matching_passwords_group: FormGroup;

  parentErrorStateMatcher = new ParentErrorStateMatcher();

  userRegister_validation_messages = {
    'userName': [
      { type: 'required', message: 'Username is required' },
      { type: 'minlength', message: 'Username must be at least 5 characters long' },
      { type: 'maxlength', message: 'Username cannot be more than 25 characters long' },
      // { type: 'pattern', message: 'Your username must contain at least one number and one letter' },
      { type: 'validUsername', message: 'Your username has already been taken' }
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
            , private accountService: AccountService
            , private router: Router) { }

  ngOnInit() {
    this.createForms();
  }

  createForms() {
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

    this.userRegisterForm = this.formBuilder.group({
      userName: new FormControl('', Validators.compose([
       UsernameValidator.validUsername,
       Validators.maxLength(25),
       Validators.minLength(5),
       Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9]+$'), // ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$
       Validators.required
      ])),
      email: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
      ])),
      matching_passwords: this.matching_passwords_group
      // terms: new FormControl(false, Validators.pattern('true'))
    });
  }

  onSubmit(value) {

    const viewModelObject = <RegisterViewModel> {
      UserName: value.userName,
      Email: value.email,
      Password: value.matching_passwords.password,
      ConfirmPassword: value.matching_passwords.confirmPassword
    };

    this.accountService.register(viewModelObject)
    .pipe(first())
    .subscribe(
      data => {
        console.log('successful registration');
        this.router.navigate(['/login']);
      },
      error => {
        // if (error.status === 400) { }
        console.log('error: ' + error);
        console.log('unsuccessful registration');
      });
  }

}
