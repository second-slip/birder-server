import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BirdObservationsListComponent } from './bird-observations-list.component';
import { ObservationService } from '@app/_services/observation.service';
import { ObservationViewModel } from '@app/_models/ObservationViewModel';
import { of, throwError } from 'rxjs';
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

    // move this to each test case to make the 'Arrange' step explicit

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

    // mockObservationService.getObservationsByBirdSpecies.and.returnValue(of(query));
  });

  it('should not have observations after construction', () => {
    // Arrange -- common arrange steps factored out to beforeEach(()

    // fixture.detectChanges() runs ngOnInIt()
    // Assert
    expect(component).toBeTruthy();
    expect(component.observations).toBeUndefined();
  });

  it('should load observations on ngOnInIt', () => {
    // Arrange -- common arrange steps factored out to beforeEach(()
    mockObservationService.getObservationsByBirdSpecies.and.returnValue(of(query));

    // Act or change
    fixture.detectChanges();

    // Assert
    expect(component).toBeTruthy();
    expect(component.totalItems).toBe(2);
    expect(component.observations.length).toBe(2);
    expect(component.observations[0].birdId === 1).toBeTrue();
  });

  it('should call change page', () => {
    // Arrange -- common arrange steps factored out to beforeEach(()
    mockObservationService.getObservationsByBirdSpecies.and.returnValue(of(query));

    // Act or change
    component.changePage();

    // Assert
    expect(mockObservationService.getObservationsByBirdSpecies).toHaveBeenCalled();
    expect(component.totalItems).toBe(2);
    expect(component.observations.length).toBe(2);
    expect(component.observations[0].birdId === 1).toBeTrue();
  });

  it('should log message in console on error', () => {
    // Arrange
    mockObservationService.getObservationsByBirdSpecies.and.returnValue(throwError('error'));
    spyOn(window.console, 'log');

    // Act (or change)
    component.ngOnInit();  // or component.changePage();  or fixture.detectChanges();

    // Assert
    expect(window.console.log).toHaveBeenCalledWith('bad request');
    expect(mockObservationService.getObservationsByBirdSpecies).toHaveBeenCalled();
  });
});
