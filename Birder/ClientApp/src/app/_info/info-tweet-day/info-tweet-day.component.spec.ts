import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoTweetDayComponent } from './info-tweet-day.component';
import { TweetsService } from '@app/_services/tweets.service';
import { of } from 'rxjs';

describe('InfoTweetDayComponent', () => {
  let component: InfoTweetDayComponent;
  let fixture: ComponentFixture<InfoTweetDayComponent>;

  let mockTweetsService;

  beforeEach(async(() => {
    mockTweetsService  = jasmine.createSpyObj(['getTweetDay']);

    TestBed.configureTestingModule({
      declarations: [ InfoTweetDayComponent ],
      providers: [
        { provide: TweetsService, useValue: mockTweetsService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InfoTweetDayComponent);
    component = fixture.componentInstance;
    mockTweetsService.getTweetDay.and.returnValue(of(null));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
