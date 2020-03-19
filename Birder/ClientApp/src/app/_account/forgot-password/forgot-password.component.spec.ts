import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotPasswordComponent } from './forgot-password.component';
import { FormBuilder } from '@angular/forms';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { AccountService } from '@app/_services/account.service';
import { of } from 'rxjs';
import { RouterTestingModule } from '@angular/router/testing';

describe('ForgotPasswordComponent', () => {
  let component: ForgotPasswordComponent;
  let fixture: ComponentFixture<ForgotPasswordComponent>;

  let mockAccountService;
  let mockToastr;

  beforeEach(async(() => {
    mockAccountService = jasmine.createSpyObj(['forgotPassword']);
    mockToastr = jasmine.createSpyObj(['info']);

    TestBed.configureTestingModule({
      imports: [ ToastrModule.forRoot(),
        RouterTestingModule.withRoutes([
        // { path: 'login', component: DummyLoginLayoutComponent },
      ])
    ],
      declarations: [ ForgotPasswordComponent ],
      providers: [ FormBuilder,
        { provide: AccountService, useValue: mockAccountService },
        { provide: ToastrService, useValue: mockToastr }
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
