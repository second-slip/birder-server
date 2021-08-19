import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
//
import { AccountService } from '@app/_account/account.service';
import { ParentErrorStateMatcher } from '@app/_validators';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ForgotPasswordComponent implements OnInit {
  forgotEmailForm: FormGroup;
  parentErrorStateMatcher = new ParentErrorStateMatcher();

  forgot_password_validation_messages = {
    'email': [
      { type: 'required', message: 'Email is required' },
      { type: 'pattern', message: 'Enter a valid email' }
    ],
  };

  constructor(private formBuilder: FormBuilder
            , private accountService: AccountService

            , private router: Router) { }

  ngOnInit() {
    this.createForms();
  }

  createForms(): void {
    this.forgotEmailForm = this.formBuilder.group({
      email: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
      ])),
    });
  }

  onSubmit(value): void {
    this.accountService.forgotPassword(value)
      .subscribe(_ => {
          this.router.navigate(['/forgot-password-confirmation']);
        },
        (_ => {
         // this.toast.error('A network error occurred.  Please try again later', 'Error');
        }));
  }
}
