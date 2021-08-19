import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResetPasswordComponent } from './reset-password.component';
import { FormBuilder } from '@angular/forms';
import { AccountService } from '@app/_account/account.service';

import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';

describe('ResetPasswordComponent', () => {
  let component: ResetPasswordComponent;
  let fixture: ComponentFixture<ResetPasswordComponent>;

  let mockAccountService;


  beforeEach(async(() => {
    mockAccountService = jasmine.createSpyObj(['resetPassword']);


    TestBed.configureTestingModule({
      imports: [
      RouterTestingModule.withRoutes([
        // { path: 'login', component: DummyLoginLayoutComponent },
      ])
      ],
      declarations: [ResetPasswordComponent],
      providers: [FormBuilder,
        { provide: AccountService, useValue: mockAccountService },

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
