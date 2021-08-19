import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailResendComponent } from './confirm-email-resend.component';
import { FormBuilder } from '@angular/forms';

import { AccountService } from '@app/_account/account.service';
import { of } from 'rxjs';

describe('ConfirmEmailResendComponent', () => {
  let component: ConfirmEmailResendComponent;
  let fixture: ComponentFixture<ConfirmEmailResendComponent>;

  let mockAccountService;


  beforeEach(async(() => {
    mockAccountService = jasmine.createSpyObj(['resendEmailConfirmation']);
    

    TestBed.configureTestingModule({
      declarations: [ ConfirmEmailResendComponent ],
      providers: [ FormBuilder,
        { provide: AccountService, useValue: mockAccountService },
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmEmailResendComponent);
    component = fixture.componentInstance;
    mockAccountService.resendEmailConfirmation.and.returnValue(of(null));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
