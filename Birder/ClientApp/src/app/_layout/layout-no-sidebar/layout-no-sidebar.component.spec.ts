import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutNoSidebarComponent } from './layout-no-sidebar.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('LayoutNoSidebarComponent', () => {
  let component: LayoutNoSidebarComponent;
  let fixture: ComponentFixture<LayoutNoSidebarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LayoutNoSidebarComponent ],
      schemas:  [NO_ERRORS_SCHEMA]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutNoSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
