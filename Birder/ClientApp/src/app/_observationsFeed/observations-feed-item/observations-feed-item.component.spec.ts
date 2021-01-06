import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationsFeedItemComponent } from './observations-feed-item.component';

describe('ObservationsFeedItemComponent', () => {
  let component: ObservationsFeedItemComponent;
  let fixture: ComponentFixture<ObservationsFeedItemComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ObservationsFeedItemComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationsFeedItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
