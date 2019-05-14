import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotPasswordConfirmationComponent } from './forgot-password-confirmation.component';

describe('ForgotPasswordConfirmationComponent', () => {
  let component: ForgotPasswordConfirmationComponent;
  let fixture: ComponentFixture<ForgotPasswordConfirmationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForgotPasswordConfirmationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForgotPasswordConfirmationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
