import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';

import { TweetsService } from './tweets.service';
import { HttpErrorHandlerService } from './http-error-handler.service';

describe('TweetsService', () => {
  // let mockHttpErrorHandlerService;

  beforeEach(() => TestBed.configureTestingModule({
    // mockHttpErrorHandlerService = jasmine.createSpyObj(['handleHttpError']);

    imports: [ HttpClientTestingModule ],
    providers: [
      TweetsService, HttpErrorHandlerService
      // { provide: HttpErrorHandlerService, useValue: mockHttpErrorHandlerService }
    ]
  }));

  it('should be created', () => {
    const service: TweetsService = TestBed.inject(TweetsService);
    expect(service).toBeTruthy();
  });
});
