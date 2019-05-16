import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountManagerAvatarComponent } from './account-manager-avatar.component';

describe('AccountManagerAvatarComponent', () => {
  let component: AccountManagerAvatarComponent;
  let fixture: ComponentFixture<AccountManagerAvatarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountManagerAvatarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountManagerAvatarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
