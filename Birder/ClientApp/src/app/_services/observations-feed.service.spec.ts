import { TestBed } from '@angular/core/testing';

import { ObservationsFeedService } from './observations-feed.service';
import { HttpErrorHandlerService } from './http-error-handler.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ObservationsFeedService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      ObservationsFeedService, HttpErrorHandlerService
    ]
  }));

  it('should be created', () => {
    const service: ObservationsFeedService = TestBed.inject(ObservationsFeedService);
    expect(service).toBeTruthy();
  });
});
