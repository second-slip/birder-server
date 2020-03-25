import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserObservationsListComponent } from './user-observations-list.component';
import { ObservationService } from '@app/_services/observation.service';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';
import { ObservationDto } from '@app/_models/ObservationFeedDto';

describe('UserObservationsListComponent', () => {
  let component: UserObservationsListComponent;
  let fixture: ComponentFixture<UserObservationsListComponent>;

  let observations: ObservationViewModel[];
  let bird: BirdSummaryViewModel;
  let user: UserViewModel;
  let query: ObservationDto;

  let mockObservationService;

  beforeEach(async(() => {
    mockObservationService = jasmine.createSpyObj(['getObservationsByUser']);

    TestBed.configureTestingModule({
      declarations: [ UserObservationsListComponent ],
      providers: [
        { provide: ObservationService, useValue: mockObservationService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserObservationsListComponent);
    component = fixture.componentInstance;
    // fixture.detectChanges();
    bird = {
      birdId: 1, species: 'string', englishName: 'string', populationSize: 'string',
      btoStatusInBritain: 'string', thumbnailUrl: 'string', songUrl: 'string',
      conservationStatus: 'string', conservationListColourCode: 'string', birderStatus: 'string',
    };

    user = {
      userName: 'string', avatar: 'string',
      defaultLocationLatitude: 1, defaultLocationLongitude: 1
    };

    observations = [{
      observationId: 1, locationLatitude: 1, locationLongitude: 1, quantity: 1,
      noteGeneral: 'string', noteHabitat: 'string', noteWeather: 'string',
      noteAppearance: 'string', noteBehaviour: 'string', noteVocalisation: 'string',
      hasPhotos: true, observationDateTime: '2019-05-06T11:32:03.796', creationDate: '2019-05-06T11:32:03.796',
      lastUpdateDate: '2019-05-06T11:32:03.796', birdId: 1,
      bird: bird, user: user
    }, {
      observationId: 2, locationLatitude: 1, locationLongitude: 1, quantity: 1,
      noteGeneral: 'string', noteHabitat: 'string', noteWeather: 'string',
      noteAppearance: 'string', noteBehaviour: 'string', noteVocalisation: 'string',
      hasPhotos: true, observationDateTime: '2019-05-06T11:32:03.796', creationDate: '2019-05-06T11:32:03.796',
      lastUpdateDate: '2019-05-06T11:32:03.796', birdId: 1,
      bird: bird, user: user
    }];

    query = { totalItems: observations.length, items: observations };

  });

  it('should not have observations after construction', () => {
    // Arrange -- common arrange steps factored out to beforeEach(()

    // fixture.detectChanges() runs ngOnInIt()
    // Assert
    expect(component).toBeTruthy();
    expect(component.observations).toBeUndefined();
  });
});
