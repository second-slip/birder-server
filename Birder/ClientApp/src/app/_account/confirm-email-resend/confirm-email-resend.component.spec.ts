import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailResendComponent } from './confirm-email-resend.component';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '@app/_account/account.service';
import { of } from 'rxjs';

describe('ConfirmEmailResendComponent', () => {
  let component: ConfirmEmailResendComponent;
  let fixture: ComponentFixture<ConfirmEmailResendComponent>;

  let mockAccountService;
  let mockToastr;

  beforeEach(async(() => {
    mockAccountService = jasmine.createSpyObj(['resendEmailConfirmation']);
    mockToastr = jasmine.createSpyObj(['error']);

    TestBed.configureTestingModule({
      declarations: [ ConfirmEmailResendComponent ],
      providers: [ FormBuilder,
        { provide: AccountService, useValue: mockAccountService },
        { provide: ToastrService, useValue: mockToastr }
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
