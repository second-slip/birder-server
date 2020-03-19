import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordComponent } from './reset-password.component';
import { FormBuilder } from '@angular/forms';
import { AccountService } from '@app/_services/account.service';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';

describe('ResetPasswordComponent', () => {
  let component: ResetPasswordComponent;
  let fixture: ComponentFixture<ResetPasswordComponent>;

  let mockAccountService;
  let mockToastr;

  beforeEach(async(() => {
    mockAccountService = jasmine.createSpyObj(['resetPassword']);
    mockToastr = jasmine.createSpyObj(['info']);

    TestBed.configureTestingModule({
      imports: [ToastrModule.forRoot(),
      RouterTestingModule.withRoutes([
        // { path: 'login', component: DummyLoginLayoutComponent },
      ])
      ],
      declarations: [ResetPasswordComponent],
      providers: [FormBuilder,
        { provide: AccountService, useValue: mockAccountService },
        { provide: ToastrService, useValue: mockToastr }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResetPasswordComponent);
    component = fixture.componentInstance;
    mockAccountService.resetPassword.and.returnValue(of(null));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
