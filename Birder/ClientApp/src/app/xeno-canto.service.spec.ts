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

  describe('getSubStringStartPosition', () => {

    it('should return expected index', () => {
      const testString = '0/1/2/3/4/5/';
      const subString = '\/';
      const index = 2;
      const expected = 4;

      let actual = service.getSubStringStartPosition(testString, subString, index);

      expect(actual).toBe(expected);

    });

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
