import { TestBed } from '@angular/core/testing';

import { ObservationsAnalysisService } from './observations-analysis.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpErrorHandlerService } from './http-error-handler.service';

describe('ObservationsAnalysisService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      ObservationsAnalysisService, HttpErrorHandlerService
    ]
  }));

  it('should be created', () => {
    const service: ObservationsAnalysisService = TestBed.inject(ObservationsAnalysisService);
    expect(service).toBeTruthy();
  });
});
