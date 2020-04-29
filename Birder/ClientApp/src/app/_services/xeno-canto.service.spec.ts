// import { TestBed } from '@angular/core/testing';

// import { XenoCantoService } from './xeno-canto.service';
// import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
// import { HttpClient } from '@angular/common/http';
// import { IXenoCantoResponse, IRecording } from '../_models/IXenoCantoResponse';

// describe('XenoCantoService', () => {
//   let service: XenoCantoService;

//   let httpClient: HttpClient;
//   let httpTestingController: HttpTestingController;

//   beforeEach(() => {
//     TestBed.configureTestingModule({
//       imports: [HttpClientTestingModule]
//     });
//     service = TestBed.inject(XenoCantoService);
//     httpClient = TestBed.inject(HttpClient);
//     httpTestingController = TestBed.inject(HttpTestingController);
//   });

//   afterEach(() => {
//     // After every test, assert that there are no more pending requests.
//     httpTestingController.verify();
//   });

//   it('should be created', () => {
//     expect(service).toBeTruthy();
//   });

//   describe('getRecordings (http get)', () => {
//     let expectedResponse: IXenoCantoResponse;
//     let recordings: IRecording[];

//     beforeEach(() => {
//       // heroService = TestBed.inject(HeroesService);
//       recordings = [{ id: 1, url: 'string' }, { id: 2, url: 'string' }, { id: 3, url: 'string' }];

//       expectedResponse = {
//         numRecordings: 'string', numSpecies: 'string', page: 'string', numPages: 'string',
//         recordings: recordings
//       };
//     });

//     it('should return expected heroes (called once)', () => {

//       service.getRecordings('').subscribe(
//         heroes => expect(heroes).toEqual(expectedResponse, 'should return expected heroes'),
//         fail
//       );

//       // HeroService should have made one request to GET heroes from expected URL
//       const req = httpTestingController.expectOne('/data');
//       expect(req.request.method).toEqual('GET');

//       // Respond with the mock heroes
//       req.flush(expectedResponse);
//     });

//   });

//   describe('getSubStringStartPosition', () => {

//     it('should return expected index', () => {
//       const testString = '0/1/2/3/4/5/';
//       const subString = '\/';
//       const index = 2;
//       const expected = 4;

//       let actual = service.getSubStringStartPosition(testString, subString, index);

//       expect(actual).toBe(expected);

//     });

//   });


//   describe('formatSearchTerm', () => {

//     it('should return formatted string', () => {
//       // arrange
//       const testString = 'space replaced';
//       const expectedString = 'space+replaced';

//       // act
//       let actual = service.formatSearchTerm(testString);

//       // assert
//       expect(actual).toEqual(expectedString);
//     });
//   });

// });
