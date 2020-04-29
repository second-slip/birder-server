import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InfoTweetDayComponent } from './info-tweet-day.component';
import { TweetsService } from '@app/_services/tweets.service';
import { of } from 'rxjs';
import { TweetDay } from '@app/_models/TweetDay';



describe('InfoTweetDayComponent', () => {
  let component: InfoTweetDayComponent;
  let fixture: ComponentFixture<InfoTweetDayComponent>;

  let mockTweetsService;
  let heroes: TweetDay;

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

    let myObj = {tweetDayId: 11, songUrl: '', displayDay: new Date(Date.now()), creationDate: new Date(Date.now()),
    lastUpdateDate: new Date(Date.now()),
    bird: {birdId: 1, species: '', englishName: '', populationSize: '', btoStatusInBritain: '',
     thumbnailUrl: '', conservationStatus: '', conservationListColourCode: '', birderStatus: '' } };
    heroes = myObj;

    mockTweetsService.getTweetDay.and.returnValue(of(heroes));
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
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