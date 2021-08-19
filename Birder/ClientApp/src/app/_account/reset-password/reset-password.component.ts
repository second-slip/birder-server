import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
// 
import { ResetPasswordViewModel } from '@app/_models/ResetPasswordViewModel';
import { AccountService } from '@app/_account/account.service';
import { ParentErrorStateMatcher, PasswordValidator } from '@app/_validators';


@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ResetPasswordComponent implements OnInit {
  matching_passwords_group: FormGroup;
  resetPasswordForm: FormGroup;
  errorReport: any;
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
      code: new FormControl(code),
      matching_passwords: this.matching_passwords_group
    });
  }

  onSubmit(value): void {

    const viewModelObject = <ResetPasswordViewModel>{
      email: value.email,
      password: value.matching_passwords.password,
      confirmPassword: value.matching_passwords.confirmPassword,
      code: value.code
    };

    this.accountService.resetPassword(viewModelObject)
    .subscribe((resp: any) => {
      console.log(resp)
        //  this.unsuccessful = false;
        //  this.changePasswordForm.reset();
        //  this.toast.success('Your changed your password', 'Success');
         this.router.navigate(['/reset-password-confirmation']);
       },
      (error: any) => {
        // if (error.status === 400) { }
        this.errorReport = error;
        // this.unsuccessful = true;
        // this.toast.error('Your password could not be changed', 'Error');
      });
  }

}
