import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationsListComponent } from './observations-list.component';

describe('ObservationsListComponent', () => {
  let component: ObservationsListComponent;
  let fixture: ComponentFixture<ObservationsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
