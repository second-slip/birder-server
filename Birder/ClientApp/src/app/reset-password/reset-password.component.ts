import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { AccountService } from '../account.service';
import { ToastrService } from 'ngx-toastr';
import { ParentErrorStateMatcher, PasswordValidator } from 'src/validators';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  // code: string;
  resetPasswordForm: FormGroup;
  matching_passwords_group: FormGroup;
  // errorReport: ErrorReportViewModel;
  parentErrorStateMatcher = new ParentErrorStateMatcher();
  // returnUrl: string;

  reset_password_validation_messages = {
    'email': [
      { type: 'required', message: 'Email is required' },
      { type: 'pattern', message: 'Enter a valid email' }
    ],
    'password': [
      { type: 'required', message: 'Your new password is required' },
      { type: 'minlength', message: 'Password must be at least 8 characters long' },
      { type: 'pattern', message: 'Your password must contain at least one number and one letter' }
    ],
    'confirmPassword': [
      { type: 'required', message: 'You must confirm your new password' },
      { type: 'areEqual', message: 'Password mismatch' }
    ]
  };

  constructor(private route: ActivatedRoute
            , private formBuilder: FormBuilder
            , private accountService: AccountService
            , private toast: ToastrService
            , private router: Router) { }

  ngOnInit() {
    // const id = this.route.snapshot.paramMap.get('code');
    // this.code = this.route.snapshot.paramMap.get('code');
    this.createForms(this.route.snapshot.paramMap.get('code'));
  }

  createForms(code: string): void {

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

    this.resetPasswordForm = this.formBuilder.group({
      email: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
      ])),
      code: new FormControl(code)
    });
  }

}
