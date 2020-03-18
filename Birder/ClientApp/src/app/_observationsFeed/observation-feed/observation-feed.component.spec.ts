import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationFeedComponent } from './observation-feed.component';
import { TokenService } from '@app/_services/token.service';
import { of } from 'rxjs';
import { ObservationsFeedService } from '@app/_services/observations-feed.service';
import { ToastrService } from 'ngx-toastr';

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

    TestBed.configureTestingModule({
      declarations: [ ObservationFeedComponent ],
      providers: [
        { provide: ObservationsFeedService, useValue: mockObservationsFeedService },
        { provide: TokenService, useValue: mockTokenService },
        { provide: ToastrService, useValue: mockToastr }
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ObservationFeedComponent);
    component = fixture.componentInstance;
    mockTokenService.getAuthenticatedUserDetails.and.returnValue(of(null));
    //
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
