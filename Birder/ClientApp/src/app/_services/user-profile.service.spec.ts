import { TestBed } from '@angular/core/testing';

import { UserProfileService } from './user-profile.service';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { UserProfileViewModel } from '@app/_models/UserProfileViewModel';


describe('UserProfileService', () => {
  let httpTestingController: HttpTestingController;
  let userProfileService: UserProfileService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      // Import the HttpClient mocking services
      imports: [HttpClientTestingModule],
      // Provide the service-under-test and its dependencies
      providers: [
        UserProfileService,

      ]
    });

    // Inject the http, test controller, and service-under-test
    // as they will be referenced by each test.
    // httpClient = TestBed.inject(HttpClient);
    httpTestingController = TestBed.inject(HttpTestingController);
    userProfileService = TestBed.inject(UserProfileService);
  });

  afterEach(() => {
    // After every test, assert that there are no more pending requests.
    httpTestingController.verify();
  });

  it('should be created', () => {
    const service: UserProfileService = TestBed.inject(UserProfileService);
    expect(service).toBeTruthy();
  });

  describe('#getUserProfile', () => {
    let expectedProfile: UserProfileViewModel;
    //let bird: BirdSummaryViewModel;

    beforeEach(() => {
      userProfileService = TestBed.inject(UserProfileService);
      expectedProfile = {
        avatar: '', isFollowing: false, isOwnProfile: false, registrationDate: '',
        userName: '', followersCount: 0, followingCount: 0
      };
    });

    it('should return expected UserProfile (called once)', () => {
      const username = 'test';

      userProfileService.getUserProfile(username).subscribe(
        heroes => expect(heroes).toEqual(expectedProfile, 'should return expected user profile'),
        fail
      );

      // TweetService should have made one request to GET heroes from expected URL
      const req = httpTestingController.expectOne(`api/UserProfile?requestedUsername=${username}`);
      expect(req.request.method).toEqual('GET');

      // Respond with the mock tweet
      req.flush(expectedProfile);
    });

    // error is now caught by the error interceptor, not in the service...
    
  //   it('should return ErrorReportViewModel if throws 404 error', () => {
  //     const username = 'test';

  //     userProfileService.getUserProfile(username).subscribe(
  //       data => fail('Should have failed with 404 error'),
  //       (error: any) => {
  //         expect(error.errorNumber).toEqual(404);
  //         expect(error.message).toContain('Not Found');
  //         expect(error.type).toContain('unsuccessful response code');
  //         expect(error.friendlyMessage).toContain('An error occurred retrieving data.');
  //       }
  //     );

  //     const req = httpTestingController.expectOne(`api/UserProfile?requestedUsername=${username}`);

  //     // respond with a 404 and the error message in the body
  //     const msg = 'deliberate 404 error';
  //     req.flush(msg, {status: 404, statusText: 'Not Found'});
  //   });
  });

});
