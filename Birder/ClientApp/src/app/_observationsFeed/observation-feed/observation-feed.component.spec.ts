import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationFeedComponent } from './observation-feed.component';

describe('ObservationFeedComponent', () => {
  let component: ObservationFeedComponent;
  let fixture: ComponentFixture<ObservationFeedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationFeedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationFeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
