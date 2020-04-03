import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { TweetsService } from './tweets.service';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { HttpClient } from '@angular/common/http';
import { TweetDay } from '@app/_models/TweetDay';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';
import { ErrorReportViewModel } from '@app/_models/ErrorReportViewModel';

describe('TweetsService', () => {
  // let httpClient: HttpClient;
  // let httpTestingController: HttpTestingController;
  // let tweetsService: TweetsService;
  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
  let tweetsService: TweetsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      // Import the HttpClient mocking services
      imports: [HttpClientTestingModule],
      // Provide the service-under-test and its dependencies
      providers: [
        TweetsService,
        HttpErrorHandlerService,
        // MessageService
      ]
    });

    // Inject the http, test controller, and service-under-test
    // as they will be referenced by each test.
    httpClient = TestBed.inject(HttpClient);
    httpTestingController = TestBed.inject(HttpTestingController);
    tweetsService = TestBed.inject(TweetsService);
  });

  afterEach(() => {
    // After every test, assert that there are no more pending requests.
    httpTestingController.verify();
  });




  describe('#getTweetDay', () => {
    let expectedTweet: TweetDay;
    let bird: BirdSummaryViewModel;
    let expectedError: ErrorReportViewModel;

    beforeEach(() => {
      tweetsService = TestBed.inject(TweetsService);
      expectedTweet = {
        tweetDayId: 1, displayDay: 'Date | string', creationDate: 'Date | string',
        lastUpdateDate: 'Date | string', bird: bird
      };

      expectedError = { type: 'string', errorNumber: 1, message: 'string', serverCustomMessage: 'string',
        friendlyMessage: 'string', modelStateErrors: [''] };
    });

    it('should return expected TweetDay (called once)', () => {

      tweetsService.getTweetDay().subscribe(
        heroes => expect(heroes).toEqual(expectedTweet, 'should return expected heroes'),
        fail
      );

      // TweetService should have made one request to GET heroes from expected URL
      const req = httpTestingController.expectOne('api/Tweets/GetTweetDay');
      expect(req.request.method).toEqual('GET');

      // Respond with the mock tweet
      req.flush(expectedTweet);
    });

    // This service reports the error but finds a way to let the app keep going.
    it('should turn 404 into an empty heroes result', () => {

      tweetsService.getTweetDay().subscribe(
        heroes => expect(heroes).toEqual(expectedError, 'should return empty heroes array'),
        fail
      );

      const req = httpTestingController.expectOne('api/Tweets/GetTweetDay');

      // respond with a 404 and the error message in the body
      const msg = '';
      req.flush(expectedError);
      // req.flush('s', { status: 404, statusText: 'Not Found' });
    });


  });

});
