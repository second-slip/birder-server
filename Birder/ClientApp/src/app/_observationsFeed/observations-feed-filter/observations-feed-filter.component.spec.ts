import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationsFeedFilterComponent } from './observations-feed-filter.component';

describe('ObservationsFeedFilterComponent', () => {
  let component: ObservationsFeedFilterComponent;
  let fixture: ComponentFixture<ObservationsFeedFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationsFeedFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationsFeedFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
