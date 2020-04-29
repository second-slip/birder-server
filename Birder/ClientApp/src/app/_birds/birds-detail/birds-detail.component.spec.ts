import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { BirdsDetailComponent } from './birds-detail.component';
import { RouterTestingModule } from '@angular/router/testing';
import { BirdsService } from '@app/_services/birds.service';
import { FlickrService } from '@app/_services/flickr.service';
import { ActivatedRouteStub } from 'testing/activated-route-stub';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { BirdDetailViewModel } from '@app/_models/BirdDetailViewModel';


describe('BirdsDetailComponent', () => {

  let component: BirdsDetailComponent;
  let fixture: ComponentFixture<BirdsDetailComponent>;

  let activatedRoute: ActivatedRouteStub;

  beforeEach(() => { activatedRoute = new ActivatedRouteStub(); });
  beforeEach(() => activatedRoute.setParamMap({ id: 99999 }));
  let mockBirdsService;
  let mockFlickrService;

  // the `id` value is irrelevant because ignored by service stub
  beforeEach(() => activatedRoute.setParamMap({ id: 99999 }));

  beforeEach(async(() => {
    const routerSpy = createRouterSpy();
    function createRouterSpy() {
      return jasmine.createSpyObj('Router', ['navigate']);
    }
    mockBirdsService = jasmine.createSpyObj(['getBird']);
    mockFlickrService = jasmine.createSpyObj(['getPhotoThumnail']);

    TestBed.configureTestingModule({
      imports: [RouterTestingModule.withRoutes([])],
      declarations: [BirdsDetailComponent],
      providers: [
        { provide: BirdsService, useValue: mockBirdsService },
        { provide: FlickrService, useValue: mockFlickrService },
        // { provide: Router, useValue: routerSpy },
      ],
      schemas: [NO_ERRORS_SCHEMA]
    })
      .compileComponents();
  }));


  beforeEach(() => {
    fixture = TestBed.createComponent(BirdsDetailComponent);
    component = fixture.componentInstance;

    // activatedRoute.setParamMap({ id: 1 });
  });

  // it('should create', () => {
  //   // fixture.detectChanges();
  //   expect(component).toBeTruthy();
  //   expect(component.bird).toBeUndefined();
  //   expect(component.images).toBeUndefined();
  // });

  it('should call getBird', () => {
    const conserveStatus = {
      conservationStatusId: 1, conservationList: '',
      conservationListColourCode: '', description: '', creationDate: 'Date | string',
      lastUpdateDate: 'Date | string', birds: []
    };

    const bird = {
      birdId: 0, class: '', order: '', family: '',
      genus: '', species: '', englishName: '', populationSize: '',
      btoStatusInBritain: '', thumbnailUrl: '',
      birderStatus: '', birdConservationStatus: conserveStatus,
      internationalName: '', category: '', creationDate: 'Date | string',
      lastUpdateDate: 'Date | string'
    };

    mockBirdsService.getBird.and.returnValue(of(bird));

    fixture.detectChanges();
    // // Act or change
    // component.getBird(1);

    // // fixture.detectChanges();

    // Assert
    expect(mockBirdsService.getBird).toHaveBeenCalled();
  });


});



// images = [{ id: 1, url: '' }, { id: 1, url: '' }, { id: 1, url: '' }];

// conserveStatus = {
//   conservationStatusId: 1, conservationList: '',
//   conservationListColourCode: '', description: '', creationDate: 'Date | string',
//   lastUpdateDate: 'Date | string', birds: []
// };

// bird = {
//   birdId: 1, class: '', order: '', family: '',
//   genus: '', species: '', englishName: '', populationSize: '',
//   btoStatusInBritain: '', thumbnailUrl: '', songUrl: '',
//   birderStatus: '', birdConservationStatus: conserveStatus,
//   internationalName: '', category: '', creationDate: 'Date | string',
//   lastUpdateDate: 'Date | string'
// };
// mockBirdsService.getBird.and.returnValue(of(bird));