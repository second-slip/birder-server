import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationFeedItemComponent } from './observation-feed-item.component';

describe('ObservationsFeedItemComponent', () => {
  let component: ObservationFeedItemComponent;
  let fixture: ComponentFixture<ObservationFeedItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ObservationFeedItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationFeedItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
