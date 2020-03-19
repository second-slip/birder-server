import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutAccountManagerComponent } from './layout-account-manager.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('LayoutAccountManagerComponent', () => {
  let component: LayoutAccountManagerComponent;
  let fixture: ComponentFixture<LayoutAccountManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutAccountManagerComponent ],
      schemas: [NO_ERRORS_SCHEMA]
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
