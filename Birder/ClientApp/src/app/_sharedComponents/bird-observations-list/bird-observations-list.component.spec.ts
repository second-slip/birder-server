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
      declarations: [BirdObservationsListComponent],
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
    }, {
      observationId: 2, locationLatitude: 1, locationLongitude: 1, quantity: 1,
      noteGeneral: 'string', noteHabitat: 'string', noteWeather: 'string',
      noteAppearance: 'string', noteBehaviour: 'string', noteVocalisation: 'string',
      hasPhotos: true, observationDateTime: '2019-05-06T11:32:03.796', creationDate: '2019-05-06T11:32:03.796',
      lastUpdateDate: '2019-05-06T11:32:03.796', birdId: 1,
      bird: bird, user: user
    }];

    query = { totalItems: 2, items: observations };

    mockObservationService.getObservationsByBirdSpecies.and.returnValue(of(query));
    // fixture.detectChanges();
  });

  it('should not have observations after construction', () => {
    // fixture.detectChanges() runs ngOnInIt()
    expect(component).toBeTruthy();
    expect(component.observations).toBeUndefined();
  });

  it('should load observations after ngOnInIt', () => {
    // mockObservationService.getObservationsByBirdSpecies.and.returnValue(of(query));
    fixture.detectChanges();
    expect(component).toBeTruthy();
    expect(component.totalItems).toBe(2);
    expect(component.observations.length).toBe(2);
    expect(component.observations[0].birdId === 1).toBeTrue();
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