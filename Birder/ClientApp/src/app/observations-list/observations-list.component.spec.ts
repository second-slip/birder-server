import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationsListComponent } from './observations-list.component';

describe('ObservationsListComponent', () => {
  let component: ObservationsListComponent;
  let fixture: ComponentFixture<ObservationsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ObservationsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationsListComponent);
    component = fixture.componentInstance;
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