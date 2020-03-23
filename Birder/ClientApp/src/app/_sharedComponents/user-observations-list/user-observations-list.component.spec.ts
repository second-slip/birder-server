import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserObservationsListComponent } from './user-observations-list.component';

describe('UserObservationsListComponent', () => {
  let component: UserObservationsListComponent;
  let fixture: ComponentFixture<UserObservationsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserObservationsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserObservationsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
