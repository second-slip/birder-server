import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdDetailInfoComponent } from './bird-detail-info.component';

import { BirdDetailViewModel } from '@app/_models/BirdDetailViewModel';
import { ConservationStatus } from '@app/_models/ConservationStatus';

describe('BirdDetailInfoComponent', () => {
  let component: BirdDetailInfoComponent;
  let fixture: ComponentFixture<BirdDetailInfoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BirdDetailInfoComponent]
    })
      .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdDetailInfoComponent);
    component = fixture.componentInstance;

    const conservationStatus: ConservationStatus = {
      conservationStatusId: 0,
      conservationList: 'string',
      conservationListColourCode: 'string',
      description: 'string',
      creationDate: 'string',
      lastUpdateDate: 'string',
      birds: [],
    }
    const bird: BirdDetailViewModel = {
      birdId: 0,
      class: 'string',
      order: 'string',
      family: 'string',
      genus: 'string',
      species: 'string',
      englishName: 'string',
      internationalName: 'string',
      category: 'string',
      populationSize: 'string',
      btoStatusInBritain: 'string',
      thumbnailUrl: 'string',
      creationDate: 'string',
      lastUpdateDate: 'string',
      birdConservationStatus: conservationStatus,
      birderStatus: 'string'
    };

    component.bird = bird;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
