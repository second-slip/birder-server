import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotPasswordComponent } from './forgot-password.component';
import { FormBuilder } from '@angular/forms';

import { AccountService } from '@app/_account/account.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

describe('ForgotPasswordComponent', () => {
  let component: ForgotPasswordComponent;
  let fixture: ComponentFixture<ForgotPasswordComponent>;

  let mockAccountService;

  beforeEach(async(() => {
    mockAccountService = jasmine.createSpyObj(['forgotPassword']);


    TestBed.configureTestingModule({
      declarations: [ ForgotPasswordComponent ],
      providers: [ FormBuilder,
        { provide: AccountService, useValue: mockAccountService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForgotPasswordComponent);
    component = fixture.componentInstance;
    mockAccountService.forgotPassword.and.returnValue(of(null));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
