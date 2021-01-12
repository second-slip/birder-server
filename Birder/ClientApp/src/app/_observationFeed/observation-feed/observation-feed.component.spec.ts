import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationFeedComponent } from './observation-feed.component';
import { TokenService } from '@app/_services/token.service';
import { of } from 'rxjs';
import { ObservationsFeedService } from '@app/_services/observations-feed.service';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


describe('ObservationFeedComponent', () => {
  let component: ObservationFeedComponent;
  let fixture: ComponentFixture<ObservationFeedComponent>;
  let mockTokenService;
  let mockObservationsFeedService;
  let mockToastr;

  beforeEach(async(() => {
    mockTokenService = jasmine.createSpyObj(['getAuthenticatedUserDetails']);
    mockObservationsFeedService = jasmine.createSpyObj(['getObservationsFeed']);
    mockToastr = jasmine.createSpyObj(['info']);

    // See https://github.com/scttcper/ngx-toastr/issues/339 for ggod setup of mock Toastr objects

    TestBed.configureTestingModule({
      imports: [ToastrModule.forRoot()],
      declarations: [ ObservationFeedComponent ],
      providers: [
        { provide: ObservationsFeedService, useValue: mockObservationsFeedService },
        { provide: TokenService, useValue: mockTokenService },
        { provide: ToastrService, useValue: mockToastr }
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
