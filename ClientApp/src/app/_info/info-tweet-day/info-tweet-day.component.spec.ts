import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoTweetDayComponent } from './info-tweet-day.component';

describe('InfoTweetDayComponent', () => {
  let component: InfoTweetDayComponent;
  let fixture: ComponentFixture<InfoTweetDayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InfoTweetDayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoTweetDayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
