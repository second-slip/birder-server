import { TestBed } from '@angular/core/testing';

import { ObservationService } from './observation.service';

describe('ObservationService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ObservationService = TestBed.inject(ObservationService);
    expect(service).toBeTruthy();
  });
});
