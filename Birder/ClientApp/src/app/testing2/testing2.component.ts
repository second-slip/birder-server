import { Component, OnInit } from '@angular/core';
import { Observable, of } from 'rxjs';
import { delay, map } from 'rxjs/operators';
import { AsyncValidatorFn, AbstractControl, ValidationErrors, FormGroup, FormBuilder, Validators } from '@angular/forms';
import { UsernameValidationService } from '@app/_services/username-validation-service.service';
import { RestrictedNameValidator } from 'validators/RestrictedNameValidator';

@Component({
  selector: 'app-testing2',
  templateUrl: './testing2.component.html',
  styleUrls: ['./testing2.component.scss']
})
export class Testing2Component implements OnInit {

  frmAsyncValidator: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private usernameService: UsernameValidationService
  ) {
    // this.frmAsyncValidator = this.createForm();
  }

  ngOnInit() {
    this.frmAsyncValidator = this.createForm();
  }

  hasError(field: string, error: string): boolean {
    if (error === 'any' || error === '') {
      return (
        this.frmAsyncValidator.controls[field].dirty &&
        this.frmAsyncValidator.controls[field].invalid
      );
    }

    // this.frmLogin.controls[field].pending;

    return (
      this.frmAsyncValidator.controls[field].dirty &&
      this.frmAsyncValidator.controls[field].hasError(error)
    );
  }

  // @TODO: Touch on Perfomance

  createForm(): FormGroup {
    return this.formBuilder.group({
      username: [
        'chicken', {
          validators: [Validators.maxLength(25),
          Validators.minLength(5),
          Validators.pattern('^(?=.*[a-zA-Z])[a-zA-Z0-9]+$'), // ^(?=.*[a-zA-Z])(?=.*[0-9])[a-zA-Z0-9]+$
          Validators.required,
          RestrictedNameValidator(/birder/i)],
          asyncValidators: [this.usernameService.usernameValidator()],
          updateOn: 'blur'
        }
      ],
      email: [
        // this updates on blur
        'a@b.com',
        {
          validators: [Validators.required],
          // asyncValidators: [this.usernameService.usernameValidator()],
          updateOn: 'blur'
        }
      ]
    });
  }

}
