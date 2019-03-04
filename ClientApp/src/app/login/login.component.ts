import { AuthentificationService } from '../authentification.service';
import { Router } from '@angular/router';
// import { NgForm } from '@angular/forms';
import { first } from 'rxjs/operators';

import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ParentErrorStateMatcher } from '../../validators';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
  invalidLogin: boolean;
  // loginViewModel: LoginViewModel;
  loginForm: FormGroup;

  parentErrorStateMatcher = new ParentErrorStateMatcher();

  login_validation_messages = {
    // 'username': [
    //   { type: 'required', message: 'Username is required' },
    //   { type: 'minlength', message: 'Username must be at least 5 characters long' },
    //   { type: 'maxlength', message: 'Username cannot be more than 25 characters long' },
    //   { type: 'pattern', message: 'Your username must contain only numbers and letters' },
    //   { type: 'validUsername', message: 'Your username has already been taken' }
    // ],
    'username': [
      { type: 'required', message: 'Email is required' },
      { type: 'pattern', message: 'Enter a valid email' }
    ],
    // 'confirm_password': [
    //   { type: 'required', message: 'Confirm password is required' },
    //   { type: 'areEqual', message: 'Password mismatch' }
    // ],
    'password': [
      { type: 'required', message: 'Password is required' },
      { type: 'minlength', message: 'Password must be at least 5 characters long' }
      // { type: 'pattern', message: 'Your password must contain at least one uppercase, one lowercase, and one number' }
    ],
    'terms': [
      { type: 'pattern', message: 'You must accept terms and conditions' }
    ]
  };

  constructor(private router: Router
            , private authentificationService: AuthentificationService
            , private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.authentificationService.logout();
    this.createForms();
    // this.loginViewModel = new LoginViewModel();
    // alert(this.loginViewModel.password);
    // TODO: get return url from route parameters or default to '/'
  }

  onSubmit(value) {
    // let credentials = JSON.stringify(form.value);
    // this.loginViewModel = form.value;
    // console.log(this.eg);
    // var model: LoginViewModel;
    // model = value;
    // console.log('model data: ' + value);

    this.authentificationService.login(value)
      .pipe(first())
      .subscribe(
        data => {
          // TODO: remove console log
          console.log('successful login');
          this.router.navigate(['/']);
        },
        error => {
          this.invalidLogin = true;
          // TODO: remove console log
          console.log('UN-successful login');
        });
  }

  createForms() {
    this.loginForm = this.formBuilder.group({
      // username: new FormControl('', Validators.compose([
      //  UsernameValidator.validUsername,
      //  Validators.maxLength(25),
      //  Validators.minLength(5),
      //  Validators.pattern('^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$'),
      //  Validators.required
      // ])),
      username: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
      ])),
      password: new FormControl('', Validators.compose([
        Validators.required,
      ])),
      terms: new FormControl(false, Validators.pattern('true'))
    });


    //   matching_passwords: this.matching_passwords_group,
    //   terms: new FormControl(false, Validators.pattern('true'))
    // });
  }
}

export interface LoginViewModel {
  username: string;
  password: string;
  remember: boolean;
}

