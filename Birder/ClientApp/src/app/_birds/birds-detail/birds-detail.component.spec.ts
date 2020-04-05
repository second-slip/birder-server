import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BirdsDetailComponent } from './birds-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { BirdsService } from '@app/_services/birds.service';
import { of } from 'rxjs';
import { TestbedHarnessEnvironment } from '@angular/cdk/testing/testbed';
import { HarnessLoader } from '@angular/cdk/testing';
import { BirdDetailViewModel } from '@app/_models/BirdDetailViewModel';
import { Router } from '@angular/router';
import { ActivatedRouteStub } from 'testing/activated-route-stub';
import { FlickrService } from '@app/_services/flickr.service';
import { FlickrUrlsViewModel } from '@app/_models/FlickrUrlsViewModel';
import { ConservationStatus } from '@app/_models/ConserverationStatus';

// let loader: HarnessLoader;

describe('BirdsDetailComponent', () => {
  let component: BirdsDetailComponent;
  let fixture: ComponentFixture<BirdsDetailComponent>;
  let router: Router;

  let bird: BirdDetailViewModel;
  let conserveStatus: ConservationStatus;
  let images: FlickrUrlsViewModel[];

  let activatedRoute: ActivatedRouteStub;

  let mockBirdsService;
  let mockFlickrService;

  beforeEach(async(() => {
    mockBirdsService = jasmine.createSpyObj(['getBird', 'getObservations']);
    mockFlickrService = jasmine.createSpyObj(['getPhotoThumnail']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])
      ],
      declarations: [BirdsDetailComponent],
      providers: [
        { provide: BirdsService, useValue: mockBirdsService },
        { provide: FlickrService, useValue: mockFlickrService }
      ]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsDetailComponent);
    router = TestBed.inject(Router);
    component = fixture.componentInstance;
    // fixture.detectChanges();

    images = [{ id: 1, url: 'string' }, { id: 1, url: 'string' }, { id: 1, url: 'string' }];

    conserveStatus = {
      conservationStatusId: 1, conservationList: 'string',
      conservationListColourCode: 'string', description: 'string', creationDate: 'Date | string',
      lastUpdateDate: 'Date | string', birds: []
    };

    bird = {
      birdId: 1, class: 'string', order: 'string', family: 'string',
      genus: 'string', species: 'string', englishName: 'string', populationSize: 'string',
      btoStatusInBritain: 'string', thumbnailUrl: 'string', songUrl: 'string',
      birderStatus: 'string', birdConservationStatus: conserveStatus,
      internationalName: 'string', category: 'string', creationDate: 'Date | string',
      lastUpdateDate: 'Date | string'
    };
    mockBirdsService.getBird.and.returnValue(of(bird));
  });

  it('should create', () => {
    expect(component).toBeTruthy();
    expect(component.bird).toBeUndefined();
    expect(component.images).toBeUndefined();
  });
});
