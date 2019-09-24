import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LockedOutComponent } from './locked-out.component';

describe('LockedOutComponent', () => {
  let component: LockedOutComponent;
  let fixture: ComponentFixture<LockedOutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LockedOutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LockedOutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
