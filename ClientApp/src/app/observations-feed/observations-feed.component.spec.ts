import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationsFeedComponent } from './observations-feed.component';

describe('ObservationsFeedComponent', () => {
  let component: ObservationsFeedComponent;
  let fixture: ComponentFixture<ObservationsFeedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationsFeedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationsFeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
