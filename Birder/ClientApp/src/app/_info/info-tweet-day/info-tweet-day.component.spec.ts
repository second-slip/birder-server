import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { InfoTweetDayComponent } from './info-tweet-day.component';
import { TweetsService } from '@app/_services/tweets.service';
import { of } from 'rxjs';
import { TweetDay } from '@app/_models/TweetDay';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';

describe('InfoTweetDayComponent', () => {
  let component: InfoTweetDayComponent;
  let fixture: ComponentFixture<InfoTweetDayComponent>;

  let mockTweetsService;
  let mockTweet: TweetDay;
  let mockBird: BirdSummaryViewModel;

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

  describe('loads tweet on creation', () => {

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

});



// this.observations = [];
// // this.isActived = true;
// let myObj: ObservationViewModel = { observationId: 1, locationLatitude: 1, locationLongitude: 1,
//   quantity: 1,
//   noteGeneral: 'string',
//   noteHabitat: 'string',
//   noteWeather: 'string',
//   noteAppearance: 'string',
//   noteBehaviour: 'string',
//   noteVocalisation: 'string',
//   hasPhotos: true,
//   // SelectedPrivacyLevel: PrivacyLevel;
//   observationDateTime: 'string',
//   creationDate: 'string',
//   lastUpdateDate: 'string',
//   birdId: 1,
//   bird: null,
//   user: null  };
//   console.log(myObj);
// this.observations.push(myObj);