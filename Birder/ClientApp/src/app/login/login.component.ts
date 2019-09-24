import { AuthenticationService } from '../authentication.service';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { ParentErrorStateMatcher } from '../../validators';
import { ErrorReportViewModel, AuthErrorViewModel } from '../../_models/ErrorReportViewModel';
import { Location } from '@angular/common';
import { AuthenticationFailureReason } from '../../_models/AuthenticationResultDto';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage: string;
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
    , private toast: ToastrService) { }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    this.authenticationService.logout();
    this.createForms();
  }

  onSubmit(value): void {
    this.errorMessage = null;
    this.authenticationService.login(value)
      .pipe(first())
      .subscribe(_ => {
        this.router.navigate([this.returnUrl]);
      },
        (error: AuthErrorViewModel) => {
          switch (error.failureReason) {
            case AuthenticationFailureReason.EmailConfirmationRequired: {
              this.toast.info('You must confirm your email address before you can login.', 'Confirm your email', {
                timeOut: 8000
              });
              this.router.navigate(['/confirm-email']);
              break;
            }
            case AuthenticationFailureReason.LockedOut: {
              break;
            }
            default: {
              this.errorMessage = 'There was an error logging in.  Make sure your email and password are correct.' +
              'If you need to reset your password please use the link below.';
              break;
            }
          }
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
