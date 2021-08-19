import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationFeedComponent } from './observation-feed.component';
import { TokenService } from '@app/_services/token.service';
import { of } from 'rxjs';
import { ObservationsFeedService } from '@app/_observationFeed/observations-feed.service';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


describe('ObservationFeedComponent', () => {
  let component: ObservationFeedComponent;
  let fixture: ComponentFixture<ObservationFeedComponent>;
  let mockTokenService;
  let mockObservationsFeedService;


  beforeEach(async(() => {
    mockTokenService = jasmine.createSpyObj(['getAuthenticatedUserDetails']);
    mockObservationsFeedService = jasmine.createSpyObj(['getObservationsFeed']);




    TestBed.configureTestingModule({

      declarations: [ ObservationFeedComponent ],
      providers: [
        { provide: ObservationsFeedService, useValue: mockObservationsFeedService },
        { provide: TokenService, useValue: mockTokenService },

      ]
    })
    .compileComponents();
  }));

  // see: https://angular.io/guide/testing#component-with-async-service

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationFeedComponent);
    component = fixture.componentInstance;
    mockTokenService.getAuthenticatedUserDetails.and.returnValue(of(null));
    mockObservationsFeedService.getObservationsFeed.and.returnValue(of(null));
    //
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
