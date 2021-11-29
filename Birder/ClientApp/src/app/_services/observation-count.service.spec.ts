import { TestBed } from '@angular/core/testing';

import { ObservationCountService } from './observation-count.service';

describe('ObservationCountService', () => {
  let service: ObservationCountService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ObservationCountService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
