import { TestBed } from '@angular/core/testing';

import { BirdsService } from './birds.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpErrorHandlerService } from './http-error-handler.service';

describe('BirdsService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      BirdsService, HttpErrorHandlerService
    ]
  }));

  it('should be created', () => {
    const service: BirdsService = TestBed.inject(BirdsService);
    expect(service).toBeTruthy();
  });
});
