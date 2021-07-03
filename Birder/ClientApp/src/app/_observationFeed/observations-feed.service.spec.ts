// import { TestBed } from '@angular/core/testing';

// import { ObservationsFeedService } from './observations-feed.service';

// import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
// import { ObservationFeedDto } from '@app/_models/ObservationFeedDto';
// import { ObservationViewModel } from '@app/_models/ObservationViewModel';
// import { ObservationFeedFilter } from '@app/_models/ObservationFeedFilter';


// describe('ObservationsFeedService', () => {
//   let httpTestingController: HttpTestingController;
//   let observationsFeedService: ObservationsFeedService;

//   beforeEach(() => {
//     TestBed.configureTestingModule({
//       // Import the HttpClient mocking services
//       imports: [HttpClientTestingModule],
//       // Provide the service-under-test and its dependencies
//       providers: [
//         ObservationsFeedService,

//         // MessageService
//       ]
//     });

//     httpTestingController = TestBed.inject(HttpTestingController);
//     observationsFeedService = TestBed.inject(ObservationsFeedService);
//   });

//   afterEach(() => {
//     // After every test, assert that there are no more pending requests.
//     httpTestingController.verify();
//   });


//   describe('#getObservationsFeed', () => {
//     let expectedObservationFeedDto: ObservationFeedDto;
//     let observations: ObservationViewModel[];

//     beforeEach(() => {
//       observationsFeedService = TestBed.inject(ObservationsFeedService); // ??????
//       expectedObservationFeedDto = {
//         items: observations, returnFilter: ObservationFeedFilter.Own, totalItems: 0
//       };
//     });

//     it('should return expected ObservationsFeed object (called once)', () => {
//       // Arrange
//       const filter: ObservationFeedFilter = ObservationFeedFilter.Own
//       const pageIndex: number = 1;

//       // Act
//       observationsFeedService.getObservationsFeed(pageIndex, filter).subscribe(
//         heroes => expect(heroes).toEqual(expectedObservationFeedDto, 'should return expected heroes'),
//         fail
//       );

//       // service should have made one request to GET heroes from expected URL
//       const req = httpTestingController.expectOne(`api/ObservationFeed?pageIndex=${pageIndex}&filter=${filter}`);
//       expect(req.request.method).toEqual('GET');

//       // Respond with the mock tweet
//       req.flush(expectedObservationFeedDto);
//     });

//     it('should return ErrorReportViewModel if throws 404 error', () => {
//       // Arrange
//       const filter: ObservationFeedFilter = ObservationFeedFilter.Own
//       const pageIndex: number = 1;

//       observationsFeedService.getObservationsFeed(pageIndex, filter).subscribe(
//         data => fail('Should have failed with 404 error'),
//         (error: any) => {
//           expect(error.errorNumber).toEqual(404);
//           expect(error.message).toContain('Not Found');
//           expect(error.type).toContain('unsuccessful response code');
//           expect(error.friendlyMessage).toContain('An error occurred retrieving data.');
//         }
//       );

//       const req = httpTestingController.expectOne(`api/ObservationFeed?pageIndex=${pageIndex}&filter=${filter}`);

//       // respond with a 404 and the error message in the body
//       const msg = 'deliberate 404 error';
//       req.flush(msg, { status: 404, statusText: 'Not Found' });
//     });

//   });

//   // it('should be created', () => {
//   //   const service: ObservationsFeedService = TestBed.inject(ObservationsFeedService);
//   //   expect(service).toBeTruthy();
//   // });
// });
