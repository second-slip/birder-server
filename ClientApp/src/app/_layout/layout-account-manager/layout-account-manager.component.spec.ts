import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutAccountManagerComponent } from './layout-account-manager.component';

describe('LayoutAccountManagerComponent', () => {
  let component: LayoutAccountManagerComponent;
  let fixture: ComponentFixture<LayoutAccountManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutAccountManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutAccountManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
