import { TestBed } from '@angular/core/testing';

import { XenoCantoService } from './xeno-canto.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';

describe('XenoCantoService', () => {
  let service: XenoCantoService;

  let httpClient: HttpClient;
  let httpTestingController: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule]
    });
    service = TestBed.inject(XenoCantoService);
    httpClient = TestBed.inject(HttpClient);
    httpTestingController = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    // After every test, assert that there are no more pending requests.
    httpTestingController.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });



  describe('formatSearchTerm', () => {

    it('should return formatted string', () => {
      // arrange
      const testString = 'space replaced';
      const expectedString = 'space+replaced';

      // act
      let actual = service.formatSearchTerm(testString);

      // assert
      expect(actual).toEqual(expectedString);
    });
  });

});
