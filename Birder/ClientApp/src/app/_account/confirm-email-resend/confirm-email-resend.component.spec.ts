import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailResendComponent } from './confirm-email-resend.component';

describe('ConfirmEmailResendComponent', () => {
  let component: ConfirmEmailResendComponent;
  let fixture: ComponentFixture<ConfirmEmailResendComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmEmailResendComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmEmailResendComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
