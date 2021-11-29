import { TestBed } from '@angular/core/testing';

import { ObservationTopService } from './observation-top.service';

describe('ObservationTopService', () => {
  let service: ObservationTopService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ObservationTopService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
