import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdObservationsListComponent } from './bird-observations-list.component';
import { ObservationService } from '@app/_services/observation.service';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { of } from 'rxjs';
import { ObservationDto } from '@app/_models/ObservationFeedDto';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { UserViewModel } from '@app/_models/UserViewModel';

describe('BirdObservationsListComponent', () => {
  let component: BirdObservationsListComponent;
  let fixture: ComponentFixture<BirdObservationsListComponent>;
  let observations: ObservationViewModel[];
  let bird: BirdSummaryViewModel;
  let user: UserViewModel;
  let query: ObservationDto;

  let mockObservationService;

  beforeEach(async(() => {
    mockObservationService = jasmine.createSpyObj(['getObservationsByBirdSpecies']);

    TestBed.configureTestingModule({
      declarations: [ BirdObservationsListComponent ],
      providers: [
        { provide: ObservationService, useValue: mockObservationService }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdObservationsListComponent);
    component = fixture.componentInstance;

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
  }];



  query = { totalItems: 1, items: observations };





    // fixture.detectChanges();
  });

  it('should create', () => {
    mockObservationService.getObservationsByBirdSpecies.and.returnValue(of(query));
    fixture.detectChanges();
    expect(component.observations.length).toBe(1);
    expect(component).toBeTruthy();
  });
});


function createMockOb(): void {
  // this.observations = [];
  // this.isActived = true;
  let myObj: ObservationViewModel = {
      observationId: 1, locationLatitude: 1, locationLongitude: 1, quantity: 1,
      noteGeneral: 'string', noteHabitat: 'string', noteWeather: 'string',
      noteAppearance: 'string', noteBehaviour: 'string', noteVocalisation: 'string',
      hasPhotos: true, observationDateTime: 'string', creationDate: 'string',
      lastUpdateDate: 'string', birdId: 1,
      bird: null, user: null
  };
  console.log(myObj);
  this.observations.push(myObj);
}