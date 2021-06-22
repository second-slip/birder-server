import { HttpClientTestingModule } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { ObservationsFetchService } from './observations-fetch.service';

describe('ObservationsFetchService', () => {
  let service: ObservationsFetchService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ HttpClientTestingModule ],
    });
    service = TestBed.inject(ObservationsFetchService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
