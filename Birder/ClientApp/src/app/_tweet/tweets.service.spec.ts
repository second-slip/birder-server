import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

import { TweetsService } from './tweets.service';


import { TweetDay } from '@app/_models/TweetDay';
import { BirdSummaryViewModel } from '@app/_models/BirdSummaryViewModel';


describe('TweetsService', () => {
  // let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;
  let tweetsService: TweetsService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      // Import the HttpClient mocking services
      imports: [HttpClientTestingModule],
      // Provide the service-under-test and its dependencies
      providers: [
        TweetsService,
        // MessageService
      ]
    });

    // Inject the http, test controller, and service-under-test
    // as they will be referenced by each test.
    // httpClient = TestBed.inject(HttpClient);
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

    beforeEach(() => {
      tweetsService = TestBed.inject(TweetsService);
      expectedTweet = {
        tweetDayId: 1, songUrl: '', displayDay: 'Date | string', creationDate: 'Date | string',
        lastUpdateDate: 'Date | string', bird: bird
      };
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

  });

});
