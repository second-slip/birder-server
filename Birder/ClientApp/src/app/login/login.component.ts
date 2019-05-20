import { AuthenticationService } from '../authentication.service';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ParentErrorStateMatcher } from '../../validators';
import { ErrorReportViewModel } from '../../_models/ErrorReportViewModel';
import { Location } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
  invalidLogin: boolean;
  loginForm: FormGroup;
  errorReport: ErrorReportViewModel;
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  returnUrl: string;

  login_validation_messages = {
    'username': [
      { type: 'required', message: 'Email is required' },
      { type: 'pattern', message: 'Enter a valid email' }
    ],
    'password': [
      { type: 'required', message: 'Password is required' }
    ],
    'rememberMe': [
      { type: 'pattern', message: 'You must accept terms and conditions' }
    ]
  };

  constructor(private router: Router
            , private route: ActivatedRoute
            , private authenticationService: AuthenticationService
            , private formBuilder: FormBuilder
            , private location: Location) { }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    this.authenticationService.logout();
    this.createForms();
  }

  onSubmit(value): void {
    this.authenticationService.login(value)
      .pipe(first())
      .subscribe(
        (data: any) => {
          this.invalidLogin = false;
          this.router.navigate([this.returnUrl]);
        },
        (error: ErrorReportViewModel) => {
          this.invalidLogin = true;
          this.errorReport = error;
        });
  }

  createForms(): void {
    this.loginForm = this.formBuilder.group({
      username: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
      ])),
      password: new FormControl('', Validators.compose([
        Validators.required,
      ])),
      rememberMe: new FormControl(false)
    });
  }
}
