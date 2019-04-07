import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationDetailComponent } from './observation-detail.component';

describe('ObservationDetailComponent', () => {
  let component: ObservationDetailComponent;
  let fixture: ComponentFixture<ObservationDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
