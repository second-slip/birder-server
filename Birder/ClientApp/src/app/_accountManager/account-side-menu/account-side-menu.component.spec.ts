import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountSideMenuComponent } from './account-side-menu.component';

describe('AccountSideMenuComponent', () => {
  let component: AccountSideMenuComponent;
  let fixture: ComponentFixture<AccountSideMenuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountSideMenuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountSideMenuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
