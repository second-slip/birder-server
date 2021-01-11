import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowcaseObservationFeedItemComponent } from './showcase-observation-feed-item.component';

describe('ShowcaseObservationFeedItemComponent', () => {
  let component: ShowcaseObservationFeedItemComponent;
  let fixture: ComponentFixture<ShowcaseObservationFeedItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowcaseObservationFeedItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowcaseObservationFeedItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
