import { TestBed } from '@angular/core/testing';

import { ObservationsFeedService } from './observations-feed.service';

describe('ObservationsFeedService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ObservationsFeedService = TestBed.get(ObservationsFeedService);
    expect(service).toBeTruthy();
  });
});
