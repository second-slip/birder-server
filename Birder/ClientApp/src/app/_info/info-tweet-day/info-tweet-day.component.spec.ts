import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { InfoTweetDayComponent } from './info-tweet-day.component';
import { TweetsService } from '@app/_services/tweets.service';
import { of, throwError } from 'rxjs';
import { TweetDay } from '@app/_models/TweetDay';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

describe('InfoTweetDayComponent', () => {
  let component: InfoTweetDayComponent;
  let fixture: ComponentFixture<InfoTweetDayComponent>;

  let mockTweetsService;
  let mockTweet: TweetDay;
  let mockBird: BirdSummaryViewModel;
  let mockError: ErrorReportViewModel

  beforeEach(async(() => {
    mockTweetsService = jasmine.createSpyObj(['getTweetDay']);

    TestBed.configureTestingModule({
      declarations: [InfoTweetDayComponent],
      providers: [
        { provide: TweetsService, useValue: mockTweetsService }
      ]
    })
      .compileComponents();
  }));


  beforeEach(() => {
    fixture = TestBed.createComponent(InfoTweetDayComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('successfully requests tweet', () => {

    // check DOM displays the correct ng-template...

    it('should load tweet oninit', () => {
      // Arrange
      mockBird = {
        birdId: 1,
        species: 'string',
        englishName: 'string',
        populationSize: 'string',
        btoStatusInBritain: 'string',
        thumbnailUrl: 'string',
        conservationStatus: 'string',
        conservationListColourCode: 'string',
        birderStatus: 'string'
      };

      mockTweet = {
        tweetDayId: 1, songUrl: '', displayDay: new Date(Date.now()),
        creationDate: new Date(Date.now()),
        lastUpdateDate: new Date(Date.now()),
        bird: mockBird
      };

      mockTweetsService.getTweetDay.and.returnValue(of(mockTweet));

      // Act or change
      fixture.detectChanges();

      // Assert
      expect(component).toBeTruthy();
      expect(component.tweet).toEqual(mockTweet);
    });

  });

  describe('error request', () => {

    // check DOM displays the correct ng-template...

    it('should return ErrorViewModel on error', () => {
      // Arrange
      mockError = {
        message: '',
        type: 'string',
        errorNumber: 404,
        serverCustomMessage: 'Try a different search query',
        friendlyMessage: 'string',
        modelStateErrors: []
      }

      mockTweetsService.getTweetDay.and.returnValue(throwError(mockError));

      // Act or change
      component.ngOnInit();

      // Assert
      expect(component).toBeTruthy();
      expect(mockTweetsService.getTweetDay).toHaveBeenCalled();
    });

  });

});
