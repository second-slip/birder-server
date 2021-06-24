import { TestBed, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { UsernameValidationService } from './username-validation-service.service';
import { TokenService } from './token.service';
import { HttpResponse } from '@angular/common/http';


describe('UsernameValidationService', () => {
  let usernameValidationService: UsernameValidationService;
  let httpTestingController: HttpTestingController;
  let tokenService: TokenService;
  let mockTokenService;

  beforeEach(() => {
    mockTokenService = jasmine.createSpyObj(['checkIsRecordOwner']);

    TestBed.configureTestingModule({
      // Import the HttpClient mocking services
      imports: [HttpClientTestingModule],
      // Provide the service-under-test and its dependencies
      providers: [
        UsernameValidationService,

        { provide: TokenService, useValue: mockTokenService },
      ]
    });

    httpTestingController = TestBed.inject(HttpTestingController);
    usernameValidationService = TestBed.inject(UsernameValidationService);
    tokenService = TestBed.inject(TokenService);
  });

  afterEach(() => {
    // After every test, assert that there are no more pending requests.
    httpTestingController.verify();
  });


  describe('#checkIfUsernameExists', () => {
    let expectedProfile: boolean;
    //let bird: BirdSummaryViewModel;

    beforeEach(() => {
      usernameValidationService = TestBed.inject(UsernameValidationService);
      expectedProfile = true;
      //   avatar: '', isFollowing: false, isOwnProfile: false, registrationDate: '',
      //   userName: '', followers: [], following: []
      // };
    });

    it('should return.........', () => {
      const username = 'test';
      const expected = true;

      usernameValidationService.checkIfUsernameExists(username).subscribe(
        heroes => expect(heroes).toEqual(expected, 'should return expected value'),
        fail
      );

      // TweetService should have made one request to GET heroes from expected URL
      const req = httpTestingController.expectOne(`api/Account/IsUsernameAvailable?username=${username}`);
      expect(req.request.method).toEqual('GET');

      // Respond with the mock tweet
      req.event(new HttpResponse<boolean>({body: expected}));
      // req.flush(expectedProfile);
    });

    it('should return ErrorReportViewModel if throws 404 error', () => {
      const username = 'test';

      usernameValidationService.checkIfUsernameExists(username).subscribe(
        data => fail('Should have failed with 404 error'),
        (error: ErrorEvent) => {
          expect(error['status']).toEqual('ok');
          // expect(error.message).toContain('Not Found');
          // expect(error.type).toContain('unsuccessful response code');
          // expect(error.friendlyMessage).toContain('An error occurred retrieving data.');
        }
      );

      const req = httpTestingController.expectOne(`api/Account/IsUsernameAvailable?username=${username}`);

      // respond with a 404 and the error message in the body
      const msg = 'deliberate 404 error';
      req.flush(msg, {status: 404, statusText: 'Not Found'});
    });
   });



  describe('#basics', () => {
    it('should be created', () => {
      const service: UsernameValidationService = TestBed.inject(UsernameValidationService);
      expect(service).toBeTruthy();
    });
  });

});
