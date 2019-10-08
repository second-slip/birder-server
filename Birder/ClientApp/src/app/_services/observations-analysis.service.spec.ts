import { TestBed } from '@angular/core/testing';

import { ObservationsAnalysisService } from './observations-analysis.service';

describe('ObservationsAnalysisService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ObservationsAnalysisService = TestBed.get(ObservationsAnalysisService);
    expect(service).toBeTruthy();
  });
});
