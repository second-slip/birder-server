import { TestBed } from '@angular/core/testing';


import { ObservationsAnalysisService } from './observations-analysis.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';


describe('ObservationsAnalysisService', () => {
  beforeEach(() => TestBed.configureTestingModule({
    imports: [ HttpClientTestingModule ],
    providers: [
      ObservationsAnalysisService
    ]
  }));

   it('should be created', () => {
    const service: ObservationsAnalysisService = TestBed.inject(ObservationsAnalysisService);
    expect(service).toBeTruthy();
  });
});
