import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { AccountService } from '@app/_account/account.service';
//

@Component({
  selector: 'app-confirm-email-resend',
  templateUrl: './confirm-email-resend.component.html',
  styleUrls: ['./confirm-email-resend.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class ConfirmEmailResendComponent implements OnInit {
  requesting: boolean;
  resendConfirmEmailForm: FormGroup;
  // parentErrorStateMatcher = new ParentErrorStateMatcher();

  resend_confirm_email_validation_messages = {
    'email': [
      { type: 'required', message: 'Email is required' },
      { type: 'pattern', message: 'Enter a valid email' }
    ],
  };

  constructor(private formBuilder: FormBuilder
    , private accountService: AccountService) {}

  ngOnInit() {
    this.createForms();
  }

  createForms(): void {
    this.resendConfirmEmailForm = this.formBuilder.group({
      email: new FormControl('', Validators.compose([
        Validators.required,
        Validators.pattern('^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+.[a-zA-Z0-9-.]+$')
      ])),
    });
  }

  onSubmit(value): void {
    this.requesting = true;

    this.accountService.resendEmailConfirmation(value)
      .subscribe(_ => {
        //this.toast.info('A new confirmation email has been sent.', 'Email resent');
        this.requesting = false;
      },
        (_ => {
          this.requesting = false;
          //this.toast.error('A network error occurred.  Please try again later', 'Error');
        }));
  }
}
